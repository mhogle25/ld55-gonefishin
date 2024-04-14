extends Node
var music_vol : float = 1.0
var master_vol : float = 1.0
var limit_mouse_movement : bool = false
var difficulty_selected : int = 1
var difficulty_storage : Dictionary = {0 : {"Setting": "Easy", "NoteSpawnDelay" : 2.0}, 
								1 : {"Setting": "Medium", "NoteSpawnDelay" : 1.0}, 
									2: {"Setting": "Hard", "NoteSpawnDelay" : 0.5}}  
var fullscreen: bool = false

# Called when the node enters the scene tree for the first time.
#func _ready():
	#pass # Replace with function body.
#
#
## Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
	#pass