using System.Collections.Generic;
using Godot;
using System.Linq;

namespace BFO.G.GoneFishin;

public partial class GameManager : Node 
{
	public enum SummonType 
	{
		Demon,
		Boss,
		Any
	}
	
	public record SummonEvent(SummonBody Body, SummonInfo Info, SummonData Data, SummonType Type, ColorInfo Color);
	
	[Signal] public delegate void OnAwaitEventHandler();
	[Signal] public delegate void OnSummonEventHandler();
	[Signal] public delegate void OnFailEventHandler();
	
	[Export] private int fishToDemonRatio = 10;
	[Export] private int maxWaitTime = 20;
	[Export] private Godot.Collections.Array<SummonInfo> demonInfos = new();
	[Export] private Godot.Collections.Array<SummonInfo> fishInfos = new();
	[Export] private SummonInfo bossInfo = new();
	[Export] private Godot.Collections.Array<Sprite2D> pedestals = new();
	[Export] private Area2D inventoryDisplay = null;
	[Export] private Node2D fishingMinigame = null;
	[Export] private PackedScene summonBodyPrefab = null;
	[Export] private PackedScene summonDisplayPrefab = null;
	[Export] private Vector2 bodyOffset = new(960, 540);
	[Export] private Wizard wizard = null;
	
	private readonly Dictionary<string, SummonInfo> summonInfoBank = new();
	private readonly RandomNumberGenerator rng = new();
	private readonly Timer failureTimer;
	private readonly Timer summoningTimer;
	private SummonEvent summonEvent = null;
	
	private GameCtx ctx = null;
	
	GameManager() 
	{
		this.summoningTimer = new() { OneShot = true };
		AddChild(this.summoningTimer);
		this.summoningTimer.Timeout += BeginNextSummoning;
		
		this.failureTimer = new() { OneShot = true };
		AddChild(this.failureTimer);
		this.failureTimer.Timeout += BeginAwaiting;
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
		this.summonInfoBank[this.bossInfo.Id] = this.bossInfo;
		
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
			SetupPedestal(pedestal, InstantiateSummonDisplay(demonInfo, demonInfo.GetName()));
		}
		
		IEnumerable<SummonDisplay> displays = this.ctx
			.GetSummons()
			.Select(data => 
			{
				SummonInfo info = GetSummonInfo(data.Id);
				SummonDisplay display = InstantiateSummonDisplay(info, data.FullName, data.ColorCode);
				return display;
			});
			
		SetupInventory(this.inventoryDisplay, displays);
		
		BeginAwaiting();
	}
	
	public SummonInfo GetSummonInfo(string id) => this.summonInfoBank[id];
	
	public void BeginAwaiting() 
	{
		BFCtx.Print("Beginning awaiting...");
		EmitSignal(SignalName.OnAwait);
		this.summoningTimer.WaitTime = this.rng.RandfRange(0, this.maxWaitTime);
		this.summoningTimer.Start();
	}
	
	public void BeginNextSummoning() 
	{
		BFCtx.Print("Summon caught! Beginning summon minigame...");
		EmitSignal(SignalName.OnSummon);
		int demonCount = this.ctx.GetDemonCount();
		bool boss = demonCount == this.demonInfos.Count;
		bool encounteredBoss = this.ctx.GetEncounteredBoss();
		
		SummonBody body;
		SummonInfo info;
		SummonData data;
		SummonType type;
		ColorInfo color = this.ctx.GetRandColorInfo();
		
		if (demonCount <= this.demonInfos.Count && !encounteredBoss) 
		{
			if (boss) 
				info = GetBossInfo().Also(_ => BFCtx.Print("Caught a boss!"));
			else
				info = GetNextDemonInfo(demonCount);
				
			data = new() 
			{
				Id = info.Id,
				FullName = info.GetName(),
			};
			type = boss ? SummonType.Boss : SummonType.Demon;
			body = InstantiateBody(info);
		} 
		else if (this.rng.RandiRange(1, this.fishToDemonRatio) == 1)
		{
			info = GetRandDemonInfo();
			data = new() 
			{
				Id = info.Id,
				FullName = info.GetName(),
			};
			type = SummonType.Any;
			body = InstantiateBody(info);
		} 
		else 
		{
			info = GetRandFishInfo();
			data = new() 
			{
				Id = info.Id,
				FullName = $"{color.Name} {info.GetName()}",
				ColorCode = color.Code,
			};
			type = SummonType.Any;
			body = InstantiateBodyPostgame(info, color);
		}
		this.summonEvent = new SummonEvent(body, info, data, type, color);
		BeginMinigame(this.fishingMinigame, info.GetSongId().Also(id => BFCtx.Print(id)));
	}
	
	public void HandleSummoningCompleted(int totalScore) 
	{
		BFCtx.Print("Finalizing summoning minigame...");
		SummonEvent sevent = this.summonEvent;
		
		if (sevent.Info.GetSuccessThreshold() > totalScore) 
		{
			BFCtx.Print($"Failed to catch, need to get above {sevent.Info.GetSuccessThreshold()}");
			sevent.Body.Flee();
			EmitSignal(SignalName.OnFail);
			this.failureTimer.Start();
			return;
		}
		
		sevent.Body.Catch();
	}
	
	public void HandleFinalizeSummoning() 
	{
		BFCtx.Print("Summoning minigame complete.");
		SummonEvent sevent = this.summonEvent;
		
		SummonDisplay display;
		switch (sevent.Type) 
		{
			case SummonType.Demon:
				display = InstantiateSummonDisplay(sevent.Info, sevent.Data.FullName);
				Sprite2D pedestal = this.pedestals[this.ctx.GetDemonCount()];
				SetupPedestal(pedestal, display);
				this.ctx.IncrementDemonCount();
				break;
			case SummonType.Boss:
				display = InstantiateSummonDisplay(sevent.Info, sevent.Data.FullName);
				this.ctx.EncounterBoss();
				AddToInventory(this.inventoryDisplay, display);
				this.ctx.AddSummon(sevent.Data);
				break;
			case SummonType.Any:
				display = InstantiateSummonDisplay(sevent.Info, sevent.Data.FullName, sevent.Color.Code);
				AddToInventory(this.inventoryDisplay, display);
				this.ctx.AddSummon(sevent.Data);
				break;
		}
		
		BeginAwaiting();
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
	
	private SummonDisplay InstantiateSummonDisplay(SummonInfo info, string text, string colorCode = null) 
	{
		SummonSprite sprite = colorCode is null ? info.InstantiateSprite() : info.InstantiateSprite(new Color(colorCode));
		SummonDisplay summonDisplay = this.summonDisplayPrefab.Instantiate<SummonDisplay>();
		summonDisplay.Setup(sprite, text);
		return summonDisplay;
	}
	
	private SummonBody InstantiateBody(SummonInfo info) 
	{
		SummonBody body = this.summonBodyPrefab.Instantiate<SummonBody>();
		body.Setup(info.InstantiateSprite(), this.wizard);
		AddChild(body);
		body.Position = this.bodyOffset;
		return body;
	}
	
	private SummonBody InstantiateBodyPostgame(SummonInfo info, ColorInfo color) 
	{
		SummonBody body = this.summonBodyPrefab.Instantiate<SummonBody>();
		body.Setup(info.InstantiateSprite(new Color(color.Code)), this.wizard);
		AddChild(body);
		body.Position = this.bodyOffset;
		return body;
	}
	
	private static void AddToInventory(Area2D inventory, Node2D display) => 
		inventory.CallDeferred("add_single", display);
	
	private static void SetupInventory(Area2D inventory, IEnumerable<Node2D> display) => 
		inventory.Call("setup",  new Godot.Collections.Array<Node2D>(display));
		
	private static void BeginMinigame(Node2D fishingMinigame, string songId) => 
		fishingMinigame.Call("begin", songId);
	
	private static void SetupPedestal(Sprite2D pedestal, Node2D display) => 
		pedestal.CallDeferred("setup_demon", display);
}
