extends MarginContainer
@export var quote_default: String = ""
@export var fade_speed = 30
@export var author_delay = 800
@export var end_timer : int
var num = 0
var num2 = 0
var words = ""
var author = ""
var ending: bool = false
@onready var anim = $AnimationPlayer
var exit : bool = false
signal kys(anim_time)

# Called when the node enters the scene tree for the first time.
func _ready():
	self.visible = false ####exists invisibly in scene by default
	exit = false
	#start(quote_default)
	

func start(the_quote, start_delay: int = 3):
	self.visible = true
	anim.play("fade_in")
	await get_tree().create_timer(3).timeout
	roll_quote(the_quote)
	
func _process(delta):
	if author and words and !ending:
		update_bbcode(words, author, delta*fade_speed * num, delta*fade_speed / 2 * num2)
		num += 1
		if num > author_delay:
		
			num2 += 1
	elif author and words and ending and num > 0:
	
		update_bbcode(words, author, delta*fade_speed * num, delta*fade_speed / 2 * num2)
		num -= 1
		num2 = num
	elif ending and !exit:
		kys.emit(3)
		
		
	
	
func update_bbcode(word, autho, length: int = 0, length2: int = 0 ):
	$RichTextLabel.bbcode_text = str("[center][fade start=0 length=", length, "]", word, "[/fade][/center][center][fade start=0 length=", length2, "]" , autho)
	

func roll_quote(quote : String, length : int = 0, length2: int = 0, til_end: int = 10):
	
	
	var split = quote.split("-") ####[quote, author]
	words = split[0]
	author = split[1]
	author = str("\n - [i]", author, "[/i]")
	$RichTextLabel.bbcode_text = str("[center][fade start=0 length=", length, "]", words, "[/fade][fade start=0 length=", length2, "]" , author, "[/center]")
	await get_tree().create_timer(til_end).timeout
	ending = true

func re_init():
	num = 0
	num2 = 0
	words = ""
	author = ""
	ending = false
	exit = false

func _on_kys(time):
	exit = true
	$RichTextLabel.clear()
	anim.play("fade_out")
	await get_tree().create_timer(time).timeout
	self.visible = false
	re_init()
