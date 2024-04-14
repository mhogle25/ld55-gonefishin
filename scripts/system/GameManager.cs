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
	
	public record SummonEvent(SummonInfo Info, SummonData Data, SummonType Type= SummonType.Demon);
	
	[Export] private Godot.Collections.Array<SummonInfo> demonInfos = new();
	[Export] private Godot.Collections.Array<SummonInfo> fishInfos = new();
	[Export] private SummonInfo bossInfo = new();
	[Export] private int fishToSummonRatio = 10;
	[Export] private Godot.Collections.Array<Sprite2D> pedestals = new();
	
	private readonly Dictionary<string, SummonInfo> summonInfoBank = new();
	public RandomNumberGenerator rng = new();
	private SummonEvent summonEvent = null;
	
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
			bool boss = demonCount == this.demonInfos.Count;
			
			SummonInfo info = boss ? GetNextDemonInfo(demonCount) : GetBossInfo();
			SummonData data = new() 
			{
				Id = info.Id,
				FullName = info.GetName(),
			};
			
			this.summonEvent = new(info, data, boss ? SummonType.Boss : SummonType.Demon);
			// Do summoning stuff
		} 
		else if (this.rng.RandiRange(1, this.fishToSummonRatio) == 1)
		{
			SummonInfo info = GetRandDemonInfo();
			SummonData data = new() 
			{
				Id = info.Id,
				FullName = info.GetName(),
			};
			
			this.summonEvent = new(info, data, SummonType.Any);
			// Do summoning stuff
		} 
		else 
		{
			SummonInfo info = GetRandFishInfo();
			SummonData data = new() 
			{
				Id = info.Id,
				FullName = info.GetName(),
			};
			
			this.summonEvent = new(info, data, SummonType.Any);
			// Do summoning stuff
		}
	}
	
	public void HandleSummoningCompleted(/* success parameters go here */) 
	{
		
		
		switch (this.summonEvent.Type) 
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
		return this.demonInfos[demonCount];
	}
		
	private SummonInfo GetBossInfo() 
	{
		this.ctx.IncrementDemonCount();
		return this.bossInfo;
	}
	
	private SummonInfo GetRandFishInfo() 
	{
		return this.fishInfos[this.rng.RandiRange(0, this.fishInfos.Count - 1)];
	}
		
	private SummonInfo GetRandDemonInfo() 
	{
		return this.demonInfos[this.rng.RandiRange(0, this.demonInfos.Count - 1)];
	}
	
	private static SummonDisplay InstantiateSummonDisplay(SummonInfo summonInfo) 
	{
		SummonSprite sprite = summonInfo.InstantiateSprite();
		SummonDisplay summonDisplay = new();
		summonDisplay.Setup(sprite, summonInfo.GetName());
		return summonDisplay;
	}
		
	private static void SetupPedestal(Sprite2D pedestal, SummonDisplay display, bool runOnFadedInEvent) => 
		pedestal.Call("setup_demon", display, runOnFadedInEvent);
}
