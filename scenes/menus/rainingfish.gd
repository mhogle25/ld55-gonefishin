extends Marker2D
@export var spawn_rate: float = 1.5
var fish = preload("res://assets/game_objects/randfish.tscn")

func _ready():
	var fish_inst = fish.instantiate()
	add_child(fish_inst)
	var spawnfish_timer := Timer.new()
	spawnfish_timer.name = "newfishspawn_timer"
	spawnfish_timer.set_wait_time(spawn_rate)
	spawnfish_timer.one_shot = false
	spawnfish_timer.autostart = true
	add_child(spawnfish_timer)
	spawnfish_timer.timeout.connect(spawnfish_timer_timeout)
	
	
func spawnfish_timer_timeout():
	var fish_inst = fish.instantiate()
	add_child(fish_inst)

