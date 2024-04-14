extends Node2D
@export var velocity : int = 30
@export var max_v_displacement : int
@export var max_h_displacement : int
@onready var iris = %IrisBody

# Called when the node enters the scene tree for the first time.
func _ready():
	max_h_displacement = $h_bound.position.x
	max_v_displacement = $v_bound.position.y
	

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	corral_iris(max_h_displacement, max_v_displacement, delta)
		
func corral_iris(h_dis, v_dis, del):
	var mouse_pos = get_global_mouse_position()
	iris.global_position = iris.global_position.move_toward(mouse_pos, velocity * del)
	if  iris.position.x > h_dis:
		iris.position.x = h_dis
	if iris.position.x < - h_dis:
		iris.position.x = - h_dis
	if iris.position.y > v_dis:
		iris.position.y = v_dis
	if iris.position.y < - v_dis:
		iris.position.y = -v_dis

