using System.Collections.Generic;
using Newtonsoft.Json;

namespace BFO.G.GoneFishin;

public class SaveData 
{
	[JsonProperty] private readonly List<string> summonIds = new(); 
	[JsonProperty] private int demonCount = 0;
	
	public void AddSummon(string summonId) => this.summonIds.Add(summonId);
	public void IncrementDemonCount() => this.demonCount++;
	public int DemonCount => this.demonCount;
}