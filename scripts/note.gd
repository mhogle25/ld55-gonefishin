extends Area2D
signal hit
var speed : int = 250


# Called when the node enters the scene tree for the first time.
func _ready():
	self.position.x = randi_range(Global.upperboundary, Global.lowerboundary)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	self.position.y += delta * speed ###most basic of sliding movement lol


func _on_hit():
	$NoteSprite.modulate = Color.from_hsv((randi() % 12) / 12.0, 1, 1) ####pseudo-random color picker
	self.set_collision_layer_value(2, false)

