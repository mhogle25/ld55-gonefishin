extends Node

var bpm_active = float(90.0 / 60) #####for testing bpm speed scaling, technically units of beats per second, applied as a speed multiplier to animations looping on one second
var music_data : Dictionary
## Called when the node enters the scene tree for the first time.
func _ready():
	music_data = gather_music_data("res://assets/music/")



func gather_music_data(path: String) -> Dictionary:
	var data : Dictionary = {}
	var songs : Array = []
	var dir = DirAccess.open(path)
	
	if dir:
		dir.list_dir_begin()
		var filename = dir.get_next()
		while filename != "":

			if ".mp3" in filename and "import" not in filename:
				songs.append(filename)
				#print("Found File : " + filename)
			filename = dir.get_next()
	else:
		print("An error occured when trying to access the path for " + path)
	
	for song in songs:
		
		
		var song_info : Array = song.get_basename().split(" - ") ##eg. BIG BASS - 190

		data[song_info[0]] = {"BPM": float(song_info[1]), "path": str(path+song)}
	
	return data
