using Godot;
using Newtonsoft.Json;

namespace BFO.G.GoneFishin;

public abstract class Summon 
{
	[JsonProperty] protected string id = string.Empty;
}