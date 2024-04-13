extends Area2D
signal hit

@export var speed : int = 250 ###sync to bpm


# Called when the node enters the scene tree for the first time.
func _ready():
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

