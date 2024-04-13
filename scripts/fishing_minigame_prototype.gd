extends Node2D
@export_enum("Soft Stars", "Other songs here") var song_toplay : String
@onready var cursor = $cursor_beatbox
var upperbound : int = 1800 ###pixel dependent, will change when target resolution is decided
var lowerbound : int = 1400 ###pixel dependent, will change when target resolution is decided
@onready var spawn_timer := Timer.new()
var num_note : int = 1


var bpm_mod = float(1.0 / 60.0)

var note = preload("res://assets/game_objects/note.tscn")

signal start_minigame

signal end_minigame

signal song_isplaying(song : String)


# Called when the node enters the scene tree for the first time.
func _ready():

	
	begin("Soft Stars")

func _process(_delta):
	
	cursor.position.x = get_global_mouse_position().x

	if cursor.position.x >= upperbound:
		cursor.position.x = upperbound
		
	elif cursor.position.x <= lowerbound:
		cursor.position.x = lowerbound
	#
	if Input.is_action_just_pressed("rhythm"):
		var note_hit = cursor.get_overlapping_areas()
		if note_hit:
			note_hit[0].hit.emit()
		
	if Input.is_action_just_pressed("rightclick"): ###For DEBUG PURPOSE ONLY
		spawn_note()

func begin(song: String):
	var mp3data = music.music_data

	if song in mp3data.keys():
		bpm_mod = float(mp3data[song]["BPM"] / float(60))
		music.bpm_active = bpm_mod
		$AudioStreamPlayer.set_stream(load(mp3data[song]["path"]))
		$AudioStreamPlayer.play()
		song_isplaying.emit(song)


	spawn_timer.one_shot = false
	#spawn_timer.autostart = true
	add_child(spawn_timer)
	spawn_timer.timeout.connect(spawn_timer_timeout)
	spawn_timer.start()
	Input.set_mouse_mode(Input.MOUSE_MODE_HIDDEN)
	#Create Cooldown Rate for spawning notes at a given BPM
	spawn_timer.set_wait_time(float(1/bpm_mod)) ###defaults to quarter notes of active bpm
	start_minigame.emit() ### may be unneccessary signal
	
func end():
	#re-enable mouse cursor
	Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
	#Create Cooldown Rate for spawning notes at a given BPM
	spawn_timer.one_shot = true
	spawn_timer.stop()
	end_minigame.emit() ### useful signal!


func hit_lowerbound():
	cursor.position.x = lowerbound


func hit_upperbound():
	cursor.position.x = upperbound

	

	
func spawn_note():
	var note_instance = note.instantiate()
	note_instance.enter(upperbound, lowerbound)
	note_instance.name = str("Note ", num_note)
	$NoteSpawner.add_child(note_instance)
	num_note += 1 ###Could be used to easily track accuracy of player, overall notes hit out of total
	
func spawn_timer_timeout():
	spawn_note()
	spawn_timer.set_wait_time(float(1/bpm_mod) / randi_range(1, 2) * randi_range(1,2))

### func signal receieved from controller to "begin minigame":
	#begin()
