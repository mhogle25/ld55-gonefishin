using System.Collections.Generic;
using Godot;

namespace BFO.G.GoneFishin;

public partial class GameManager : Node 
{
	[Export] private Godot.Collections.Array<SummonInfo> demonInfos = new();
	[Export] private Godot.Collections.Array<FishInfo> fishInfos = new();
	[Export] private SummonInfo bossInfo = new();
	[Export] private int fishToSummonRatio = 10;
	[Export] private Godot.Collections.Array<Sprite2D> pedestals = new();
	
	private readonly Dictionary<string, SummonInfo> summonInfoBank = new();
	
	public RandomNumberGenerator rng = new();
	
	private int demonInfoIndex = 0;
	
	private GameCtx ctx = null;
	
	public override void _Ready()
	{
		if (this.demonInfos.Count != this.pedestals.Count) 
		{
			BFCtx.PrintErr("[GameManager:_Ready] All referenced pedestals must have a corresponding demon info, by index");
			return;
		}
		
		this.ctx = GetNode<GameCtx>(GameCtx.PATH);
		this.demonInfoIndex = this.ctx.GetDemonCount();
		
		if (this.demonInfoIndex > this.demonInfos.Count || this.demonInfoIndex < 0)
		{
			BFCtx.PrintErr("[GameManager:_Ready] Demon info index in save file was out of range.");
			return;
		}
		
		this.demonInfos.SetupDictionary(this.summonInfoBank);
		this.fishInfos.SetupDictionary(this.summonInfoBank);
		
		// Setup the pedestals
		for (int i = 0; i < this.demonInfoIndex; i++) 
		{
			SummonInfo demonInfo = this.demonInfos[i];
			Sprite2D pedestal = this.pedestals[i];
			
			
		}
	}
	
	public SummonInfo GetSummonInfo(string id) => this.summonInfoBank[id];
	
	public void BeginNextSummoning() 
	{
		if (this.demonInfoIndex <= this.summonInfoBank.Count) 
		{
			SummonInfo demonInfo = this.demonInfoIndex < this.demonInfos.Count ? GetNextDemonInfo() : this.bossInfo;
			// Do summoning stuff
			this.ctx.IncrementDemonCount();
			return;
		}
		
		if (this.rng.RandiRange(1, this.fishToSummonRatio) == 1)
		{
			SummonInfo demonInfo = GetRandDemonInfo();
			// Do summoning stuff
			return;
		}
		
		FishInfo fishInfo = GetRandFishInfo();
		// Do summoning stuff
	}

	private SummonInfo GetNextDemonInfo() => 
		this.demonInfos[this.demonInfoIndex++];
		
	private FishInfo GetRandFishInfo() => 
		this.fishInfos[this.rng.RandiRange(0, this.fishInfos.Count - 1)];
		
	private SummonInfo GetRandDemonInfo() =>
		this.demonInfos[this.rng.RandiRange(0, this.demonInfos.Count - 1)];
}
