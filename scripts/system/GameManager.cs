using System.Collections.Generic;
using Godot;

namespace BFO.G.GoneFishin;

public partial class GameManager : Node 
{
	public enum SummonType 
	{
		Demon,
		Boss,
		Any
	}
	
	[Export] private Godot.Collections.Array<SummonInfo> demonInfos = new();
	[Export] private Godot.Collections.Array<SummonInfo> fishInfos = new();
	[Export] private SummonInfo bossInfo = new();
	[Export] private int fishToSummonRatio = 10;
	[Export] private Godot.Collections.Array<Sprite2D> pedestals = new();
	
	private readonly Dictionary<string, SummonInfo> summonInfoBank = new();
	public RandomNumberGenerator rng = new();
	private SummonInfo currentSummon = null;
	private SummonType summonType = SummonType.Demon;
	
	private GameCtx ctx = null;
	
	public override void _Ready()
	{
		if (this.demonInfos.Count != this.pedestals.Count) 
		{
			BFCtx.PrintErr("[GameManager:_Ready] All referenced pedestals must have a corresponding demon info, by index");
			return;
		}
		
		this.ctx = GetNode<GameCtx>(GameCtx.PATH);
		int demonCount = this.ctx.GetDemonCount();
		
		if (demonCount > this.demonInfos.Count || demonCount < 0)
		{
			BFCtx.PrintErr("[GameManager:_Ready] Demon info index in save file was out of range.");
			return;
		}
		
		this.demonInfos.SetupDictionary(this.summonInfoBank);
		this.fishInfos.SetupDictionary(this.summonInfoBank);
		
		Initialize();
	}
	
	public void Initialize() 
	{
		// Setup the pedestals
		for (int i = 0; i < this.ctx.GetDemonCount(); i++) 
		{
			SummonInfo demonInfo = this.demonInfos[i];
			Sprite2D pedestal = this.pedestals[i];
			SetupPedestal(pedestal, InstantiateSummonDisplay(demonInfo), false);
		}
	}
	
	public SummonInfo GetSummonInfo(string id) => this.summonInfoBank[id];
	
	public void BeginNextSummoning() 
	{
		int demonCount = this.ctx.GetDemonCount();
		if (demonCount <= this.summonInfoBank.Count) 
		{
			this.currentSummon = demonCount < this.demonInfos.Count ? GetNextDemonInfo(demonCount) : GetBossInfo();
			// Do summoning stuff
			return;
		}
		
		this.summonType = SummonType.Any;
		
		if (this.rng.RandiRange(1, this.fishToSummonRatio) == 1)
		{
			this.currentSummon = GetRandDemonInfo();
			// Do summoning stuff
			return;
		}
		
		SummonInfo fishInfo = GetRandFishInfo();
		this.currentSummon = fishInfo;
		// Do summoning stuff
	}
	
	public void HandleSummoningCompleted(int notesHit, int noteCount) 
	{
		// Success check
		if (notesHit == noteCount)
			this.ctx.IncrementDemonCount(); 
		
		switch (this.summonType) 
		{
			case SummonType.Demon:
				break;
			case SummonType.Boss:
				break;
			case SummonType.Any:
				break;
		}
	}

	private SummonInfo GetNextDemonInfo(int demonCount)  
	{
		this.summonType = SummonType.Demon;
		return this.demonInfos[demonCount];
	}
		
	private SummonInfo GetBossInfo() 
	{
		this.summonType = SummonType.Boss;
		this.ctx.IncrementDemonCount();
		return this.bossInfo;
	}
	
	private SummonInfo GetRandFishInfo() 
	{
		this.summonType = SummonType.Any;
		return this.fishInfos[this.rng.RandiRange(0, this.fishInfos.Count - 1)];
	}
		
	private SummonInfo GetRandDemonInfo() 
	{
		this.summonType = SummonType.Any;
		return this.demonInfos[this.rng.RandiRange(0, this.demonInfos.Count - 1)];
	}
	
	private static SummonDisplay InstantiateSummonDisplay(SummonInfo summonInfo) 
	{
		SummonSprite sprite = summonInfo.InstantiateSprite();
		SummonDisplay summonDisplay = new();
		summonDisplay.Setup(sprite, summonInfo.GetName());
		return summonDisplay;
	}
		
	private static void SetupPedestal(Sprite2D pedestal, SummonDisplay sprite, bool runOnFadedInEvent) => 
		pedestal.Call("setup_demon", sprite, runOnFadedInEvent);
}
