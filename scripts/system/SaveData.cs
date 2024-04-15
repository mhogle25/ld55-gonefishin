using System.Collections.Generic;
using Newtonsoft.Json;

namespace BFO.G.GoneFishin;

public class SaveData 
{
	[JsonProperty] private readonly List<SummonData> summons = new(); 
	[JsonProperty] private int demonCount = 0;
	
	public void AddSummon(SummonData summon) => this.summons.Add(summon);
	public void IncrementDemonCount() => (++this.demonCount).Run(x => BFCtx.Print($"Demon Count: {x}"));
	public void DecrementDemonCount() => (--this.demonCount).Run(x => BFCtx.Print($"Demon Count: {x}"));
	public int GetDemonCount() => this.demonCount;
}