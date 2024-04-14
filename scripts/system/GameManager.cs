using System.Collections.Generic;
using Godot;

namespace BFO.G.GoneFishin;

public partial class GameManager : Node 
{
	[Export] private Godot.Collections.Array<PairIdPrefab> catchSprites = new();

	private readonly Dictionary<string, PackedScene> catchSpritesByName = new();

	public override void _Ready()
	{
		this.catchSprites.SetupDictionary(this.catchSpritesByName);
	}

	public AnimatedSprite2D InstantiateCatchSprite(string id) =>
		this.catchSpritesByName[id].Instantiate<AnimatedSprite2D>();
		
	
}
