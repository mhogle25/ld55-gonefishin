using System.Collections.Generic;
using Godot;

namespace BFO.G.GoneFishin;

public partial class GameManager : Node 
{
	[Export] private Godot.Collections.Array<SummonInfo> summonInfo = new();

	private readonly Dictionary<string, SummonInfo> summonInfosByName = new();
	
	private GameCtx ctx = null;
	private Node musicCtx = null;
	
	public override void _Ready()
	{
		this.ctx = GetNode<GameCtx>(GameCtx.PATH);
		this.summonInfo.SetupDictionary(this.summonInfosByName);
	}

	public AnimatedSprite2D InstantiateSummonSprite(string id) =>
		this.summonInfosByName[id].SpritePrefab.Instantiate<AnimatedSprite2D>();
		
	private string GetSongId(string id) => 
		this.summonInfosByName[id].SongId;
}
