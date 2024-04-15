extends MarginContainer
var main = "res://scenes/menus/main_menu.tscn"
var master_AudioIndex = AudioServer.get_bus_index("Master")
var music_AudioIndex = AudioServer.get_bus_index("Music")
@onready var streamplay = $AudioStreamPlayer


func _ready():
	%Music_HSlider.value = option.music_vol 
	%Master_HSlider.value = option.master_vol 
	%MouseMovement.button_pressed = option.limit_mouse_movement
	%FullscreenToggle.button_pressed = option.fullscreen
	%DifficultySlider.value = option.difficulty_selected




func _on_back_pressed():
	get_tree().change_scene_to_file(main)


func _on_master_h_slider_value_changed(value):
	
	%MasterBar.value = value * 100
	option.master_vol = value
	AudioServer.set_bus_volume_db(master_AudioIndex,linear_to_db(value))
	streamplay.set_pitch_scale(randf_range(0.8, 1.1))
	streamplay.play()


func _on_music_h_slider_value_changed(value):
	%MusicBar.value = value * 100
	option.music_vol = value
	AudioServer.set_bus_volume_db(music_AudioIndex,linear_to_db(value))

func _on_mouse_movement_toggled(toggled_on):
	option.limit_mouse_movement = toggled_on
	streamplay.set_pitch_scale(randf_range(0.8, 1.1))
	streamplay.play()





func _on_difficulty_slider_drag_ended(value_changed):
	
	if value_changed:
		option.difficulty_selected = %DifficultySlider.value
		streamplay.set_pitch_scale(randf_range(0.8, 1.1))
		streamplay.play()



func _on_fullscreen_toggle_toggled(toggled_on):
	option.fullscreen = toggled_on
	if toggled_on:
		DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_EXCLUSIVE_FULLSCREEN)
	elif !toggled_on:
		DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_WINDOWED)

		
