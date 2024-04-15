extends Area2D

@onready var rect = $CollisionShape2D.shape.get_rect().size

var summon_display_prefab = load("res://assets/game_objects/summon_display.tscn")

func setup(summon_displays):
	for summon_display in summon_displays: 
		add_single(summon_display)

func add_single(summon_display):
	add_child(summon_display)
	var magnitudex = rect.x/2
	var magnitudey = rect.y/2
	var x = randf_range(-magnitudex,magnitudex)
	var y = randf_range(-magnitudey,magnitudey)
	summon_display.position = Vector2(x,y)

