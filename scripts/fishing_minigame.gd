extends Node2D
@onready var cursor = %cursor_beatbox
@onready var cursor_col = %cursor_collision
@onready var cursor_physics = $PhysicsCursor
@onready var cursor_static = $CursorStatic
var upperbound : int = 1800 ###pixel dependent, will change when target resolution is decided
var lowerbound : int = 1400 ###pixel dependent, will change when target resolution is decided
@export var spawn_timer: Timer
var score_rolling = false
var num_note : int = 1
@onready var difficulty : Dictionary = option.get_difficulty()
@onready var spawn_diff = difficulty["NoteSpawnDelay"]

var combo : int = 0
var temp_score : int = 0
var total_score : int = 0
@export var perfect_bounds : int = 20

var bpm_mod = float(1.0 / 60.0)
var splash_sound = preload("res://assets/sounds/splash_reverb.wav")
var scratch_sound = preload("res://assets/sounds/scratch.wav")
var note = preload("res://assets/game_objects/note.tscn")

signal end_minigame(total_score: int)

# Called when the node enters the scene tree for the first time.
func _ready():
	if option.get_limit_mouse_movement():
		upperbound = 1625
		lowerbound = 1575
	%Score.text = str("Score: ", total_score)


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
			note_hit[0].hit.emit(perfect_bounds, cursor_col.global_position.x, cursor_col.global_position.y)
			combo += 1
			temp_score += 10
			$effectssounds.set_stream(splash_sound)
			$effectssounds.set_pitch_scale(randf_range(0.9, 1.1))
			$effectssounds.play()
		elif combo != 0:
			$effectssounds.set_stream(scratch_sound)
			$effectssounds.set_pitch_scale(randf_range(0.9, 1.1))
			$effectssounds.play()
			time_score_roll()
			total_score += (temp_score * combo)
			temp_score = 0
			combo = 0 ###reset combo on a misclick
		

	set_combo_meter()
	if score_rolling:
		%Score.text = str("Score: ", randi_range(0000000, 9999999))

func begin(song: String):
	var mp3data = music.music_data
	num_note = 1
	combo = 0
	temp_score = 0

	total_score = 0
	$AnimationPlayer.play("fade_in_minigame")
	%Score.text = str("Score: ", total_score)
	if song in mp3data.keys():
		bpm_mod = float(mp3data[song]["BPM"] / float(60))
		music.bpm_active = bpm_mod
		$AudioStreamPlayer.set_stream(load(mp3data[song]["path"]))
		#$AudioStreamPlayer.play()
	#
	#spawn_timer.start()
	Input.set_mouse_mode(Input.MOUSE_MODE_HIDDEN)
	##Create Cooldown Rate for spawning notes at a given BPM
	#spawn_timer.set_wait_time(float(spawn_diff/bpm_mod)) ###defaults to quarter notes of active bpm

	
	
func end():
	#re-enable mouse cursor
	Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
	
	spawn_timer.stop()
	total_score += (temp_score * combo)
	%Score.text = str("Score: ", total_score)
	$AnimationPlayer.play("fade_out_minigame")

	end_minigame.emit(total_score) ### useful signal!

	score_rolling = false
	num_note = 1
	combo = 0
	temp_score = 0

	total_score = 0

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
	await get_tree().create_timer(randi_range(1,3)).timeout
	score_rolling = false
	%Score.text = str("Score: ", total_score)
	

	
func set_combo_meter():

	if combo > 1 and combo <10:
		%Combo.bbcode_text = str("+ ", temp_score, " X", combo)
	elif combo >= 10 and combo <=20: 
		%Combo.bbcode_text = str("[shake rate=20.0 level=5 connected=1]+ ",temp_score , " X ", combo, "[/shake]")
	elif combo > 20 and combo <= 35:
		%Combo.bbcode_text = str("[shake rate=25.0 level=20 connected=0]+ ",temp_score , " X " , combo, "[/shake]")
	elif combo > 35:
		%Combo.bbcode_text = str("[rainbow freq=1.0 sat=0.8 val=0.8][shake rate=25.0 level=30 connected=0]+ ",temp_score , " X ", combo, "[/shake][/rainbow]")
	else:
		%Combo.clear()



func _on_animation_player_animation_finished(anim):
	if anim == "fade_in_minigame":
		$AudioStreamPlayer.play()

		spawn_timer.start()
		Input.set_mouse_mode(Input.MOUSE_MODE_HIDDEN)
	#Create Cooldown Rate for spawning notes at a given BPM
		spawn_timer.set_wait_time(float(spawn_diff/bpm_mod)) ###defaults to quarter notes of active bpm

