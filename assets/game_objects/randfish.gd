extends Sprite2D
@export var min_speed = 50
@export var max_speed = 200
var path : String = "res://assets/art/"
var possible_sprites: Array = ["amon.png", "bifrons.png", "eligos.png", "orobas.png", "orb.png"]
var speed : int = 100

func _ready():
	speed = randi_range(min_speed, max_speed)
	self.scale = Vector2(0.6,0.6)
	self.texture = load(str(path + possible_sprites[randi_range(0, len(possible_sprites) - 1)]))
	self.position.x = randi_range(-900, 900) #randomly throw somewhere on screen

func _process(delta):
	self.position.y += speed * delta
	if self.position.y > 2000:
		queue_free()
