extends Sprite2D

@export var temp_demon_sprite: AnimatedSprite2D
@export var fade_rate = 0.01

signal onFadedIn

var demon_sprite: AnimatedSprite2D
var increasing: bool = true

#Substitute click event with just calling function
func _input(event):
	if event is InputEventMouseButton and event.pressed and event.button_index == MOUSE_BUTTON_LEFT:
		if get_rect().has_point(to_local(event.position)):
			setup_demon(temp_demon_sprite);

func _process(delta):
#if fading in (var), add value to the a/alpha.
	if demon_sprite == null:
		return

	if demon_sprite.modulate.a >= 1:
		print ("faded")
		onFadedIn.emit()
		return

	if increasing:
		print ("fading in")
		demon_sprite.modulate.a += fade_rate * delta

func setup_demon(sprite: AnimatedSprite2D):
	demon_sprite = sprite
	demon_sprite.modulate.a = 0
	increasing = true
	print ("Get Ryaned setup")
	pass
