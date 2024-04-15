extends MarginContainer
var options = "res://scenes/menus/options_menu.tscn"




func _on_quit_pressed():
	get_tree().quit() # Replace with function body.


func _on_options_pressed():
	get_tree().change_scene_to_file(options) # Replace with function body.


func _on_new_game_pressed():
	get_tree().change_scene_to_file("res://assets/game_objects/fishing_minigame_prototype.tscn")
	pass


func _on_load_game_pressed():
	##grab save state
	pass
