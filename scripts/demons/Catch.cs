using Godot;
using Newtonsoft.Json;

namespace BFO.G.GoneFishin;

public abstract class Catch 
{
	[JsonProperty] protected string spriteId = string.Empty;
	
	public AnimatedSprite2D InstantiateSprite(GameManager game) => 
		game.InstantiateCatchSprite(this.spriteId);
	
	public AnimatedSprite2D InstantiateThumbnail(GameManager game) =>
		game.InstantiateCatchSprite(this.spriteId);
}