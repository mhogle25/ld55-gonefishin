extends Node


@onready var spawnArea = $CollisionShape2D.shape.extents
@onready var origin = $CollisionShape2D.global_position - spawnArea

@export var summon_displays_debug: Array[Node2D] = []

#func random_pos():
	#var x = randf_range(origin.x, spawnArea.x)
	#var y = randf_range(origin.y, spawnArea.y)
	#return Vector2(x,y)


var summon_display_prefab = load("res://assets/game_objects/summon_display.tscn")

func setup(summon_displays):
	for summon_display in summon_displays: 
		add_child(summon_display)
		var x = randf_range(origin.x, spawnArea.x)
		var y = randf_range(origin.y, spawnArea.y)
		#random_pos()
		summon_display.position = Vector2(x,y)
		#print("you just got ryaned")


func _ready():
	setup(summon_displays_debug)

# Called when the node enters the scene tree for the first time.
#func _ready():
	


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
	#pass
