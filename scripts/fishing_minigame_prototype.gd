extends Node2D
@export_enum("Soft Stars", "Other songs here") var song_toplay : String
@onready var cursor = %cursor_beatbox
@onready var cursor_physics = $PhysicsCursor
@onready var cursor_static = $CursorStatic
var upperbound : int = 1800 ###pixel dependent, will change when target resolution is decided
var lowerbound : int = 1400 ###pixel dependent, will change when target resolution is decided
@onready var spawn_timer := Timer.new()
var score_rolling = false
var num_note : int = 1
@onready var difficulty : Dictionary = option.difficulty_storage[option.difficulty_selected]
@onready var spawn_diff = difficulty["NoteSpawnDelay"]
var combo : int = 0
var temp_score : int = 0
var total_score : int = 0

var bpm_mod = float(1.0 / 60.0)

var note = preload("res://assets/game_objects/note.tscn")

signal end_minigame

##signal start_minigame 



#signal song_isplaying(song : String) ##o7


# Called when the node enters the scene tree for the first time.
func _ready():
	begin("Soft Stars")
	if option.limit_mouse_movement:
		upperbound = 1625
		lowerbound = 1575

func _process(_delta):
	cursor_static.position.x = get_global_mouse_position().x
	cursor.position.x = cursor_physics.global_position.x

	if cursor.position.x >= upperbound:
		cursor.position.x = upperbound
		
	elif cursor.position.x <= lowerbound:
		cursor.position.x = lowerbound
	#
	if Input.is_action_just_pressed("rhythm"):
		var note_hit = cursor.get_overlapping_areas()
		if note_hit:
			note_hit[0].hit.emit()
			combo += 1
			temp_score += 10
		else:
			time_score_roll()
			total_score += (temp_score * combo)
			temp_score = 0
			combo = 0 ###reset combo on a misclick
		
	#if Input.is_action_just_pressed("rightclick"): ###For DEBUG PURPOSE ONLY
		#spawn_note()
	set_combo_meter()
	if score_rolling:
		%Score.text = str("Score: ", randi_range(0000000, 9999999))

func begin(song: String):
	var mp3data = music.music_data

	if song in mp3data.keys():
		bpm_mod = float(mp3data[song]["BPM"] / float(60))
		music.bpm_active = bpm_mod
		$AudioStreamPlayer.set_stream(load(mp3data[song]["path"]))
		$AudioStreamPlayer.play()
		#song_isplaying.emit(song)


	spawn_timer.one_shot = false
	#spawn_timer.autostart = true
	add_child(spawn_timer)
	spawn_timer.timeout.connect(spawn_timer_timeout)
	spawn_timer.start()
	Input.set_mouse_mode(Input.MOUSE_MODE_HIDDEN)
	#Create Cooldown Rate for spawning notes at a given BPM
	spawn_timer.set_wait_time(float(spawn_diff/bpm_mod)) ###defaults to quarter notes of active bpm
	##start_minigame.emit() ### may be unneccessary signal
	
	
func end():
	#re-enable mouse cursor
	Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)

	spawn_timer.one_shot = true
	spawn_timer.stop()
	end_minigame.emit() ### useful signal!


func hit_lowerbound():
	cursor.position.x = lowerbound

func hit_upperbound():
	cursor.position.x = upperbound

func spawn_note():
	var note_instance = note.instantiate()
	note_instance.enter(upperbound, lowerbound, bpm_mod)
	note_instance.name = str("Note ", num_note)
	$NoteSpawner.add_child(note_instance)
	num_note += 1 ###Could be used to easily track accuracy of player, overall notes hit out of total
	
func spawn_timer_timeout():
	spawn_note()
	spawn_timer.set_wait_time(float(spawn_diff/bpm_mod) / randi_range(1, 2) * randi_range(1,2))

func time_score_roll():
	score_rolling = true
	await get_tree().create_timer(randi_range(2,4)).timeout
	score_rolling = false
	%Score.text = str("Score: ", total_score)
	
	#add_child(rolling_timer)
	


	
	

func set_combo_meter():

	if combo > 1 and combo <10:
		%Combo.bbcode_text = str("+ ", temp_score, " X", combo)
	elif combo >= 10 and combo <=20: 
		##%Combo.bbcode_text = str("[rainbow freq=1.0 sat=0.8 val=0.8]+ ",temp_score , " X ", combo, "[/rainbow]")
		##%Combo.bbcode_text = str("[wave amp=50.0 freq=5.0 connected=1]+ ",temp_score , " X ", combo, "[/wave]")
		%Combo.bbcode_text = str("[shake rate=20.0 level=5 connected=1]+ ",temp_score , " X ", combo, "[/shake]")
	elif combo > 20 and combo <= 35:
		%Combo.bbcode_text = str("[shake rate=25.0 level=20 connected=0]+ ",temp_score , " X " , combo, "[/shake]")
	elif combo > 35:
		%Combo.bbcode_text = str("[rainbow freq=1.0 sat=0.8 val=0.8][shake rate=25.0 level=30 connected=0]+ ",temp_score , " X ", combo, "[/shake][/rainbow]")
	else:
		%Combo.clear()
	#%Score.text = str("Score: ", total_score)