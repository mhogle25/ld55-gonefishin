using System;
using Newtonsoft.Json;

namespace BFO.G.GoneFishin;

[Serializable]
public class SummonData 
{	
	[JsonProperty] private readonly string id = string.Empty;
	[JsonProperty] private readonly string fullName = string.Empty;
	[JsonProperty] private readonly string colorCode = "#ffffff";
	
	[JsonIgnore] public string Id { get => this.id; init => this.id = value; }
	[JsonIgnore] public string FullName { get => this.fullName; init => this.fullName = value; }
	[JsonIgnore] public string ColorCode { get => this.colorCode; init => this.colorCode = value; }
}