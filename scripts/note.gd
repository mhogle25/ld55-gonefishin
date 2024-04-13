extends Area2D


@export var speed : float = 195 * Global.bpm_debug  ###sync to bpm, currently 60bpm by default or 1bps, multiply by (Global.somedict[song_title]["GetBPM"] / 60)
@onready var animate = $AnimationPlayer
signal hit
signal be_free


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
	if self.position.y >= 1500: ###idunno thats probably off the screen
		queue_free()


func _on_hit():
	$ParticleSpawn.go_spawn.emit()
	animate.play("hit_feedback")
	self.set_collision_layer_value(2, false)
	var kill_timer := Timer.new()
	kill_timer.name = "kill_note_timer"
	#kill_timer.set_wait_time(1) ###get from global variable assigned to each song in a dict, pull from current song , set to quarter notes by default (*2 for half notes, /2 for eighth notes if you're a maniac)
	kill_timer.one_shot = true
	kill_timer.autostart = true
	add_child(kill_timer)
	kill_timer.timeout.connect(kill_timer_timeout)
	
	
func kill_timer_timeout():
	$ParticleSpawn.kill.emit()






func _on_particle_spawn_free_note():
	queue_free()
