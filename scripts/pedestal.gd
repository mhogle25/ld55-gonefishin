extends Sprite2D

@export var fade_rate = 0.2
@export var demon_position_offset := Vector2(0, 0)

signal on_faded_in

enum State { INVISIBLE, FADING_IN, FADED }

var demon_sprite: AnimatedSprite2D = null
var run_on_faded_in := false
var state := State.INVISIBLE

func _process(delta):
	#if fading in (var), add value to the a/alpha.
	if demon_sprite == null || state != State.FADING_IN:
		return

	if demon_sprite.modulate.a >= 1:
		if run_on_faded_in:
			on_faded_in.emit()
		state = State.FADED
		return

	demon_sprite.modulate.a += fade_rate * delta


func setup_demon(sprite: AnimatedSprite2D, run_on_faded_in_event: bool):
	demon_sprite = sprite
	add_child(demon_sprite)
	demon_sprite.position = demon_position_offset
	demon_sprite.modulate.a = 0
	run_on_faded_in = run_on_faded_in_event
	state = State.FADING_IN
