using System.Collections.Generic;
using Godot;

namespace BFO.G.GoneFishin;

public partial class GameManager : Node 
{
	[Export] private Godot.Collections.Array<SummonInfo> summonInfo = new();
	[Export] private Godot.Collections.Array<FishInfo> fishInfo = new();
	[Export] private SummonInfo bossInfo = new();
	[Export] private int fishToSummonRatio = 10;
	
	public RandomNumberGenerator rng = new();
	
	private int summonInfoIndex = 0;
	
	private GameCtx ctx = null;
	
	public override void _Ready()
	{
		this.ctx = GetNode<GameCtx>(GameCtx.PATH);
	}
	
	public void BeginNextSummoning() 
	{
		if (this.summonInfoIndex <= this.summonInfo.Count) 
		{
			SummonInfo summonInfo = this.summonInfoIndex < this.summonInfo.Count ? GetNextSummonInfo() : this.bossInfo;
			// Do summoning stuff
			this.ctx.IncrementSummonCount();
			return;
		}
		
		if (this.rng.RandiRange(1, this.fishToSummonRatio) == 1)
		{
			SummonInfo summonInfo = GetSummonInfo();
			// Do summoning stuff
			return;
		}
		
		FishInfo fishInfo = GetFishInfo();
		// Do summoning stuff
	}

	private SummonInfo GetNextSummonInfo() => 
		this.summonInfo[this.summonInfoIndex++];
		
	private FishInfo GetFishInfo() => 
		this.fishInfo[this.rng.RandiRange(0, this.fishInfo.Count - 1)];
		
	private SummonInfo GetSummonInfo() =>
		this.summonInfo[this.rng.RandiRange(0, this.summonInfo.Count - 1)];
}
