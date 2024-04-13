extends Marker2D

@export var particle_texture : Texture
signal go_spawn
signal stop_spawn
var spawning : bool = false
var particles_storage : Array
var SPEED = 100.0
var radius : int = 6
var spawn_limit: int = 30
var spawn_count: int = 0
@export var spawn_margin : int = 20
var spawn_timer = 0
@export var spawn_rate : float = 0.05


# Called when the node enters the scene tree for the first time.
func _ready():
	
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if spawning and spawn_timer < 0 and spawn_count < spawn_limit :
		create_particles()
		spawn_count += 1
		spawn_timer = spawn_rate
	elif spawning:
		stop_spawn.emit()
	spawn_timer -= 32*delta
	for col in particles_storage:
		var trans = PhysicsServer2D.body_get_state(col[0], PhysicsServer2D.BODY_STATE_TRANSFORM)
		trans.origin = trans.origin - global_position
		RenderingServer.canvas_item_set_transform(col[1], trans)
		
		


func create_particles():
	
	var ps = PhysicsServer2D
	var rs = RenderingServer
	
	##Set position
	var trans = global_transform
	trans.origin += Vector2(randi_range(-spawn_margin,spawn_margin), randi_range(-spawn_margin,spawn_margin))
	
	##PhysicsBody
	var part_col = ps.body_create()
	ps.body_set_mode(part_col, ps.BODY_MODE_RIGID)
	ps.body_set_space(part_col, get_world_2d().space)
	
	##Create Collision Shape (circle)
	var shape = ps.circle_shape_create()
	
	ps.shape_set_data(shape, radius) ##set circle radius
	
	##add collider to rigid body	
	ps.body_add_shape(part_col, shape, Transform2D.IDENTITY)
	
	##set collision layers to 5, reserved for particles
	ps.body_set_collision_layer(part_col, 5)
	ps.body_set_collision_mask(part_col, 5)
	
	##set parameters for physics
	ps.body_set_state(part_col, ps.BODY_STATE_LINEAR_VELOCITY, Vector2(randf_range(-1,1), randf_range(-1,1)).normalized() * SPEED)
	ps.body_set_param(part_col, ps.BODY_PARAM_GRAVITY_SCALE, 0.01)
	ps.body_set_state(part_col, ps.BODY_STATE_TRANSFORM, trans)
	
	##Create visual canvas item
	var particle = rs.canvas_item_create()
	
	##Parent child
	rs.canvas_item_set_parent(particle, get_canvas_item())
	
	##Set transform
	rs.canvas_item_set_transform(particle,trans)
	
	##Create rectangle to contain texture
	var rect = Rect2()
	rect.position = Vector2(-radius, -radius)
	rect.size = particle_texture.get_size()/spawn_margin*radius
	
	rs.canvas_item_add_texture_rect(particle, rect, particle_texture)
	
	##Color texture ##random color again idk what we're going for palette wise yet
	rs.canvas_item_set_self_modulate(particle, Color.from_hsv((randi() % 12) / 12.0, 1, 1))
	
	##add to storage array
	particles_storage.append([part_col, particle])
	
	
func kill_particles():
	if particles_storage:
		for part in particles_storage:
			PhysicsServer2D.free_rid(part[0])
			RenderingServer.free_rid(part[1])
			particles_storage.erase(part)
			


func _on_go_spawn():
	spawning = true


func _on_stop_spawn():
	spawning = false # Replace with function body.
