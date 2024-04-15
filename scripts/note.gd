extends Area2D


@export var speed : float = 195 * Global.bpm_debug  ###sync to bpm, currently 60bpm by default or 1bps, multiply by (Global.somedict[song_title]["GetBPM"] / 60)
@onready var animate = $AnimationPlayer
<<<<<<< Updated upstream
signal hit
=======
var selectedhsv : Color

#var perf_height = 780
var has_beenhit : bool = false
signal hit(perf_bound)
signal missed 
>>>>>>> Stashed changes


# Called when the node enters the scene tree for the first time.
func _ready():
	animate.speed_scale = Global.bpm_debug
	animate.play("bob")
	var rand_hsv = Color.from_hsv((randi() % 12) / 12.0, 1, 1) ####pseudo-random color picker
	self.position.x = randi_range(Global.upperboundary, Global.lowerboundary)
	$NoteSprite.modulate = rand_hsv ####pseudo-random color picker
	


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	self.position.y += delta * speed ###most basic of sliding movement lol
<<<<<<< Updated upstream
	if self.position.y >= 1500: ###idunno thats probably off the screen
		self.queue_free()


func _on_hit():
	$ParticleSpawn.go_spawn.emit()
=======
	if self.position.y >= 850 and !has_beenhit: 
		$NoteSprite.set_self_modulate(Color.DIM_GRAY)
		missed.emit() #### this signals a note has been missed

	if self.position.y >= 1500: ###idunno thats probably off the screen
		
		queue_free()
		

func enter(upbound, lowbound, tempo):
	$PerfectText.visible = false
	$NoteSprite/GoodText.visible = false
	var animation = $AnimationPlayer
	animation.speed_scale = tempo
	animation.play("bob")
	var rand_hsv = Color.from_hsv((randi() % 12) / 12.0, 1, 1) ####pseudo-random color picker
	selectedhsv = rand_hsv
	self.position.x = randi_range(upbound, lowbound)
	$NoteSprite.modulate = rand_hsv ####pseudo-random color picker
	
	
func _on_hit(bound, x_ax, y_ax):
	$ParticleSpawn.go_spawn.emit(selectedhsv)
>>>>>>> Stashed changes
	animate.play("hit_feedback")
	self.set_collision_layer_value(2, false)
	var kill_timer := Timer.new()
	kill_timer.name = "kill_note_timer"
	#kill_timer.set_wait_time(1) ###get from global variable assigned to each song in a dict, pull from current song , set to quarter notes by default (*2 for half notes, /2 for eighth notes if you're a maniac)
	kill_timer.one_shot = true
	kill_timer.autostart = true
	add_child(kill_timer)
	kill_timer.timeout.connect(kill_timer_timeout)
<<<<<<< Updated upstream
=======
	###grab vertical height hit
	var height_hit = self.global_position.y
	var horizontal_hit = self.global_position.x
	has_beenhit = true
	
	if height_hit > y_ax - bound && height_hit < y_ax + bound and horizontal_hit < x_ax + bound and horizontal_hit > x_ax - bound:
		$PerfectText.visible = true
		
	else:
		print("height hit: ", height_hit, "vs intended at :", y_ax, "\n horiz hit: ", horizontal_hit, "vs intended :", x_ax)
		$NoteSprite/GoodText.visible = true
	
>>>>>>> Stashed changes
	
	
func kill_timer_timeout():
	self.queue_free()










