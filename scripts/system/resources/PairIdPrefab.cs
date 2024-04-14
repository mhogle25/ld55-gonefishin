using System.Collections.Generic;
using Godot;

namespace BFO.G.GoneFishin;

public partial class PairIdPrefab : Resource 
{
	[Export] private string id = string.Empty;
	[Export] private PackedScene prefab = null;
	
	public string Id => this.id;
	public PackedScene Prefab => this.prefab;
}

public static class PairIdPrefabExtensions 
{
	public static void SetupDictionary(this IEnumerable<PairIdPrefab> pairs, Dictionary<string, PackedScene> dictionary) 
	{
		dictionary.Clear();
		foreach (PairIdPrefab pair in pairs) 
			dictionary[pair.Id] = pair.Prefab;
	}
}