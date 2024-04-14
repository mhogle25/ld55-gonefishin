using System.Collections.Generic;
using Godot;

namespace BFO.G.GoneFishin;

public partial class GameManager : Node 
{
	[Export] private Godot.Collections.Array<PairIdPrefab> summonSprites = new();

	private readonly Dictionary<string, PackedScene> summonSpritesByName = new();

	public override void _Ready()
	{
		this.summonSprites.SetupDictionary(this.summonSpritesByName);
	}

	public AnimatedSprite2D InstantiateSummonSprite(string id) =>
		this.summonSpritesByName[id].Instantiate<AnimatedSprite2D>();
		
	
}
