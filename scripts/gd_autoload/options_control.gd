extends Node

var difficulty_storage : Dictionary = {0 : {"Setting": "Easy", "NoteSpawnDelay" : 2.0}, 
								1 : {"Setting": "Medium", "NoteSpawnDelay" : 1.0}, 
									2: {"Setting": "Hard", "NoteSpawnDelay" : 0.5}}  
func get_music_vol():
	return GameCtx.GetMusicVol()
func set_music_vol(value):
	GameCtx.SetMusicVol(value)

func get_master_vol():
	return GameCtx.GetMasterVol()
func set_master_vol(value):
	GameCtx.SetMasterVol(value)

func get_limit_mouse_movement():
	return GameCtx.GetLimitMouseMovement()
func set_limit_mouse_movement(value):
	GameCtx.SetLimitMouseMovement(value)

func get_difficulty_selected():
	return GameCtx.GetDifficultySelected()
func set_difficulty_selected(value):
	GameCtx.SetDifficultySelected(value)

func get_fullscreen():
	return GameCtx.GetFullscreen()
func set_fullscreen(value):
	GameCtx.SetFullscreen(value)

func get_difficulty():
	return difficulty_storage[get_difficulty_selected()]


