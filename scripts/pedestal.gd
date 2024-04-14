extends Sprite2D

@export var fade_rate = 0.2
@export var demon_position_offset := Vector2(0, 0)

signal on_faded_in

enum State { INVISIBLE, FADING_IN, FADED }

var demon_display = null
var run_on_faded_in := false
var state := State.INVISIBLE

func _process(delta):
	#if fading in (var), add value to the a/alpha.
	if demon_display == null || state != State.FADING_IN:
		return

	if demon_display.GetSprite().modulate.a >= 1:
		if run_on_faded_in:
			on_faded_in.emit()
		state = State.FADED
		return

	demon_display.GetSprite().modulate.a += fade_rate * delta


func setup_demon(display, run_on_faded_in_event: bool):
	demon_display = display
	add_child(demon_display)
	demon_display.position = demon_position_offset
	demon_display.GetSprite().modulate.a = 0
	run_on_faded_in = run_on_faded_in_event
	state = State.FADING_IN
