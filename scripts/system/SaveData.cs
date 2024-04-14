using System.Collections.Generic;
using Newtonsoft.Json;

namespace BFO.G.GoneFishin;

public class SaveData 
{
	[JsonProperty] private readonly List<Catch> catches = new(); 
	[JsonProperty] private readonly List<Demon> caughtDemons = new();
}