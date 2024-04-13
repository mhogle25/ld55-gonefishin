extends Area2D


@export var speed : float = 195 * Global.bpm_debug  ###sync to bpm, currently 60bpm by default or 1bps, multiply by (Global.somedict[song_title]["GetBPM"] / 60)
@onready var animate = $AnimationPlayer
signal hit

# Called when the node enters the scene tree for the first time.
func _ready():
	animate.speed_scale = Global.bpm_debug
	animate.play("bob")
	self.position.x = randi_range(Global.upperboundary, Global.lowerboundary)
	$NoteSprite.modulate = Color.from_hsv((randi() % 12) / 12.0, 1, 1) ####pseudo-random color picker


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	self.position.y += delta * speed ###most basic of sliding movement lol
	if self.position.y >= 1500: ###idunno thats probably off the screen
		self.queue_free()


func _on_hit():
	
	self.set_collision_layer_value(2, false)
	self.queue_free() ###want to animate a cute pop function before removing

