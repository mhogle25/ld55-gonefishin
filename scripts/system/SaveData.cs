using System.Collections.Generic;
using Newtonsoft.Json;

namespace BFO.G.GoneFishin;

public class SaveData 
{
	[JsonProperty] private readonly List<SummonData> summons = new(); 
	[JsonProperty] private int demonCount = 0;
	
	public void AddSummon(SummonData summon) => this.summons.Add(summon);
	public void IncrementDemonCount() => this.demonCount++;
	public void DecrementDemonCount() => this.demonCount--;
	public int DemonCount => this.demonCount;
}