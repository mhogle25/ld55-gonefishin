using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace BFO.G.GoneFishin;

public class SaveData 
{
	[JsonProperty] private readonly List<Summon> summons = new(); 
	[JsonProperty] private readonly List<Demon> demons = new();
	
	public void AddSummon(Summon summon) => this.summons.Add(summon);
	public void AddDemon(Demon demon) => this.demons.Add(demon);
}