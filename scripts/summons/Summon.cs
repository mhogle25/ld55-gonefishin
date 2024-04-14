using Godot;
using Newtonsoft.Json;

namespace BFO.G.GoneFishin;

public abstract class Summon 
{
	[JsonProperty] protected string spriteId = string.Empty;
	
	public AnimatedSprite2D InstantiateSprite(GameManager game) => 
		game.(this.spriteId);
	
	public AnimatedSprite2D InstantiateThumbnail(GameManager game) =>
		game.InstantiateSummonSprite(this.spriteId);
}