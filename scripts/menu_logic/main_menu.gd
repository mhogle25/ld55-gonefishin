extends MarginContainer
var options = preload("res://scenes/menus/options_menu.tscn")
var newgame_scene = preload("res://assets/game_objects/summoning_game.tscn")


func _ready():
	$AnimationPlayer.play("bob_mc")


func _on_quit_pressed():
	get_tree().quit() # Replace with function body.


func _on_options_pressed():
	get_tree().change_scene_to_packed(options) # Replace with function body.


func _on_new_game_pressed():
	$AudioStreamPlayer.play()

	%QuoteDisplay.start(false, newgame_scene) ####needs to wipe save state
	


func _on_load_game_pressed():
	get_tree().change_scene_to_file("res://assets/game_objects/summoning_game.tscn")




