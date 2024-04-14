extends MarginContainer
var main = "res://scenes/menus/main_menu.tscn"



# Called when the node enters the scene tree for the first time.
func _ready():
	%Music_HSlider.value = option.music_vol * 100 
	%Master_HSlider.value = option.master_vol * 100
	%MouseMovement.button_pressed = option.limit_mouse_movement
	%DifficultySlider.value = option.difficulty
	# Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
	#pass


func _on_back_pressed():
	get_tree().change_scene_to_file(main)


func _on_master_h_slider_value_changed(value):
	%MasterBar.value = value
	option.master_vol = float(value/100)


func _on_music_h_slider_value_changed(value):
	%MusicBar.value = value
	option.music_vol = float(value/100)

func _on_mouse_movement_toggled(toggled_on):
	option.limit_mouse_movement = toggled_on





func _on_difficulty_slider_drag_ended(value_changed):
	
	if value_changed:
		option.difficulty = %DifficultySlider.value # Replace with function body.
