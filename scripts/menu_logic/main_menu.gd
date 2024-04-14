extends MarginContainer
var options = "res://scenes/menus/options_menu.tscn"

# Called when the node enters the scene tree for the first time.
#func _ready():
	#pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
	#pass


func _on_quit_pressed():
	get_tree().quit() # Replace with function body.


func _on_options_pressed():
	get_tree().change_scene_to_file(options) # Replace with function body.


func _on_new_game_pressed():
	##get_tree().change_scene_to_file(the first one)
	pass


func _on_load_game_pressed():
	###pls teach me how save states work
	pass # Replace with function body.
