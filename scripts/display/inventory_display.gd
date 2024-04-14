extends Node


var spawnArea = $CollisionShape2D.shape.extents
var origin = $CollisionShape2D.global_position - spawnArea

func random_pos():
	var x = randf_range(origin.x, spawnArea.x)
	var y = randf_range(origin.y, spawnArea.y)
	return Vector2(x,y)


var summon_display_prefab = load("res://assets/game_objects/summon_display.tscn")

func setup(summon_infos):
	for info in summon_infos: 
		var summon_display = summon_display_prefab.instantiate()
		summon_display.setup(info.InstantiateSprite(),info.GetName())
		add_child(summon_display)
		random_pos()
		summon_display.position = Vector2(x,y)



# Called when the node enters the scene tree for the first time.
#func _ready():
	#pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
	#pass
