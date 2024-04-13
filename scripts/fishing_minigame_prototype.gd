extends Node2D
@onready var cursor = $cursor_beatbox
var upperbound : int = Global.upperboundary ###pixel dependent, will change when target resolution is decided
var lowerbound : int = Global.lowerboundary ###pixel dependent, will change when target resolution is decided
var num_note : int = 1
signal hit_upperboundary
signal hit_lowerboundary
signal spawn_note
var note = preload("res://assets/game_objects/note.tscn")




# Called when the node enters the scene tree for the first time.
func _ready():
	#make the mouse disappear
	Input.set_mouse_mode(Input.MOUSE_MODE_HIDDEN)
	#Create Cooldown Rate for spawning notes at a given BPM
	var spawn_timer := Timer.new()
	spawn_timer.name = "NOTE_RATE"
	spawn_timer.set_wait_time(2) ###.set_wait_time(Global.somedict[song_title]["GetBPM"] / 60) beats per second, get from global variable assigned to each song in a dict, pull from current song
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
			note_hit[0].hit.emit()
		

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
