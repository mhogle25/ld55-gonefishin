extends Node2D
@onready var cursor = $cursor_beatbox
var vertical_range : int = 100
signal hit_upperboundary
signal hit_lowerboundary

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	
	cursor.position.y = get_global_mouse_position().y
	print(cursor.position.y)

