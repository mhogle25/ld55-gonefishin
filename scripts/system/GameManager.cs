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
	
	public record SummonEvent(SummonBody Body, SummonInfo Info, SummonData Data, SummonType Type);
	
	[Export] private int fishToSummonRatio = 10;
	[Export] private int maxWaitTime = 20;
	[Export] private Godot.Collections.Array<SummonInfo> demonInfos = new();
	[Export] private Godot.Collections.Array<SummonInfo> fishInfos = new();
	[Export] private SummonInfo bossInfo = new();
	[Export] private Godot.Collections.Array<Sprite2D> pedestals = new();
	[Export] private Node2D fishMinigame = null;
	[Export] private PackedScene summonBodyPrefab = null;
	[Export] private Vector2 bodyOffset = new(960, 540);
	
	private readonly Dictionary<string, SummonInfo> summonInfoBank = new();
	private readonly RandomNumberGenerator rng = new();
	private readonly Timer timer;
	private SummonEvent summonEvent = null;
	
	private GameCtx ctx = null;
	
	GameManager() 
	{
		this.timer = new() { OneShot = true };
		AddChild(this.timer);
		this.timer.Timeout += BeginNextSummoning;
	}
	
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
		BFCtx.Print($"Initializing pedestals and inventory... Demon Count: {this.ctx.GetDemonCount()}");
		// Setup the pedestals
		for (int i = 0; i < this.ctx.GetDemonCount(); i++) 
		{
			BFCtx.Print("initializing pedestal");
			SummonInfo demonInfo = this.demonInfos[i];
			Sprite2D pedestal = this.pedestals[i];
			SetupPedestal(pedestal, InstantiateSummonDisplay(demonInfo), false);
		}
		
		BeginAwaiting();
	}
	
	public SummonInfo GetSummonInfo(string id) => this.summonInfoBank[id];
	
	public void BeginAwaiting() 
	{
		BFCtx.Print("Beginning awaiting...");
		this.timer.WaitTime = this.rng.RandfRange(0, this.maxWaitTime);
		this.timer.Start();
	}
	
	public void BeginNextSummoning() 
	{
		BFCtx.Print("Summon caught! Beginning summon minigame...");
		int demonCount = this.ctx.GetDemonCount();
		
		SummonInfo info;
		SummonData data;
		SummonType type;
		
		if (demonCount <= this.summonInfoBank.Count) 
		{
			bool boss = demonCount == this.demonInfos.Count;
			info = boss ? GetNextDemonInfo(demonCount) : GetBossInfo();
			data = new() 
			{
				Id = info.Id,
				FullName = info.GetName(),
			};
			type = boss ? SummonType.Boss : SummonType.Demon;
		} 
		else if (this.rng.RandiRange(1, this.fishToSummonRatio) == 1)
		{
			info = GetRandDemonInfo();
			data = new() 
			{
				Id = info.Id,
				FullName = info.GetName(),
			};
			type = SummonType.Any;
		} 
		else 
		{
			info = GetRandFishInfo();
			data = new() 
			{
				Id = info.Id,
				FullName = info.GetName(),
			};
			type = SummonType.Any;
		}
		
		SummonBody body = InstantiateSummonBody(info);
		this.summonEvent = new SummonEvent(body, info, data, type);
		BeginMinigame(this.fishMinigame, info.GetSongId());
	}
	
	public void HandleSummoningCompleted(int totalScore) 
	{
		BFCtx.Print("Summoning minigame complete.");
		SummonEvent sevent = this.summonEvent;
		
		if (sevent.Info.GetSuccessThreshold() > totalScore) 
		{
			BFCtx.Print($"Failed to catch, need to get above {sevent.Info.GetSuccessThreshold()}");
			sevent.Body.Flee();
			return;
		}
		
		switch (sevent.Type) 
		{
			case SummonType.Demon:
				this.ctx.IncrementDemonCount();
				break;
			case SummonType.Boss:
				this.ctx.IncrementDemonCount();
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
	
	private SummonBody InstantiateSummonBody(SummonInfo info) 
	{
		SummonBody body = this.summonBodyPrefab.Instantiate<SummonBody>();
		body.Setup(info.InstantiateSprite());
		AddChild(body);
		body.Position = this.bodyOffset;
		return body;
	}
		
	private static void BeginMinigame(Node2D fishingMinigame, string songId) => 
		fishingMinigame.Call("begin", songId);
	
	private static void SetupPedestal(Sprite2D pedestal, SummonDisplay display, bool runOnFadedInEvent) => 
		pedestal.Call("setup_demon", display, runOnFadedInEvent);
}
