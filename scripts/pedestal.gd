extends Sprite2D

@export var fade_rate = 0.2
@export var demon_position_offset := Vector2(0, 0)

enum State { INVISIBLE, FADING_IN, VISIBLE }

var demon_display = null
var state := State.INVISIBLE

func _ready():
	z_index = int(position.y)

func _process(delta):
	#if fading in (var), add value to the a/alpha.
	if demon_display == null || state != State.FADING_IN:
		return

	if demon_display.GetSprite().modulate.a >= 1:
		state = State.VISIBLE
		return

	demon_display.GetSprite().modulate.a += fade_rate * delta


func setup_demon(display):
	demon_display = display
	add_child(demon_display)
	demon_display.position = demon_position_offset
	demon_display.GetSprite().modulate.a = 0
	state = State.FADING_IN
