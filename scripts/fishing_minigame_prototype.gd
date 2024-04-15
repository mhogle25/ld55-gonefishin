extends Node2D
<<<<<<< Updated upstream
@onready var cursor = $cursor_beatbox
var upperbound : int = Global.upperboundary ###pixel dependent, will change when target resolution is decided
var lowerbound : int = Global.lowerboundary ###pixel dependent, will change when target resolution is decided
=======
@export_enum("Soft Stars", "Other songs here") var song_toplay : String
@onready var cursor = %cursor_beatbox
@onready var cursor_col = %cursor_collision
@onready var cursor_physics = $PhysicsCursor
@onready var cursor_static = $CursorStatic
var upperbound : int = 1800 ###pixel dependent, will change when target resolution is decided
var lowerbound : int = 1400 ###pixel dependent, will change when target resolution is decided
@onready var spawn_timer := Timer.new()
var score_rolling = false
>>>>>>> Stashed changes
var num_note : int = 1
var note = preload("res://assets/game_objects/note.tscn")
signal hit_upperboundary
signal hit_lowerboundary
signal spawn_note





# Called when the node enters the scene tree for the first time.
func _ready():
	#make the mouse disappear
	Input.set_mouse_mode(Input.MOUSE_MODE_HIDDEN)
	#Create Cooldown Rate for spawning notes at a given BPM
	var spawn_timer := Timer.new()
	spawn_timer.name = "NOTE_RATE"
	spawn_timer.set_wait_time(float(1/Global.bpm_debug)) ###get from global variable assigned to each song in a dict, pull from current song , set to quarter notes by default (*2 for half notes, /2 for eighth notes if you're a maniac)
	spawn_timer.one_shot = false
	spawn_timer.autostart = true
	add_child(spawn_timer)
	spawn_timer.timeout.connect(spawn_timer_timeout)



func _process(delta):

	cursor.position.x = get_global_mouse_position().x

	if cursor.position.x >= upperbound:
		hit_upperboundary.emit()
	elif cursor.position.x <= lowerbound:
		hit_lowerboundary.emit()
	#
	if Input.is_action_just_pressed("rhythm"):
		var note_hit = cursor.get_overlapping_areas()
		if note_hit:
<<<<<<< Updated upstream
			note_hit[0].hit.emit()
=======
			note_hit[0].hit.emit(perfect_bounds, cursor_col.global_position.x, cursor_col.global_position.y)
			combo += 1
			temp_score += 10
		else:
			time_score_roll()
			total_score += (temp_score * combo)
			temp_score = 0
			combo = 0 ###reset combo on a misclick
>>>>>>> Stashed changes
		

	if Input.is_action_just_pressed("rightclick"): ###For DEBUG PURPOSE ONLY
		spawn_note.emit()


func _on_hit_lowerboundary():
	cursor.position.x = lowerbound

func _on_hit_upperboundary():
	cursor.position.x = upperbound




func _on_spawn_note():

	var note_instance = note.instantiate()
	note_instance.name = str("Note ", num_note)
	$NoteSpawner.add_child(note_instance)
	num_note += 1 ###Could be used to easily track accuracy of player, overall notes hit out of total
	
	
func spawn_timer_timeout():
	spawn_note.emit()
