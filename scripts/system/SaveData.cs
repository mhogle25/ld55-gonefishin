using System.Collections.Generic;
using Newtonsoft.Json;

namespace BFO.G.GoneFishin;

public class SaveData 
{
	[JsonProperty] private readonly List<SummonData> summons = new(); 
	[JsonProperty] private int demonCount = 0;
	[JsonProperty] private bool boss = false;
	[JsonProperty] float musicVol = 1.0f;
	[JsonProperty] float masterVol = 1.0f;
	[JsonProperty] bool limitMouseMovement = false;
	[JsonProperty] int difficultySelected = 1;
	[JsonProperty] bool fullscreen = false;
	
	[JsonIgnore] public float MusicVol { get => this.musicVol; set => this.musicVol = value; }
	[JsonIgnore] public float MasterVol { get => this.masterVol; set => this.masterVol = value; } 
	[JsonIgnore] public bool LimitMouseMovement { get => this.limitMouseMovement; set => this.limitMouseMovement = value; }
	[JsonIgnore] public int DifficultySelected { get => this.difficultySelected; set => this.difficultySelected = value;}
	[JsonIgnore] public bool Fullscreen { get => this.fullscreen; set => this.fullscreen = value; }
 	
	public void AddSummon(SummonData summon) => this.summons.Add(summon);
	public void IncrementDemonCount() => (++this.demonCount).Run(x => BFCtx.Print($"Demon Count: {x}"));
	public void DecrementDemonCount() => (--this.demonCount).Run(x => BFCtx.Print($"Demon Count: {x}"));
	public int GetDemonCount() => this.demonCount;
	public bool GetEncounteredBoss() => this.boss;
	public void EncounterBoss() => this.boss = true;
	public IEnumerable<SummonData> GetSummons() => this.summons;
}