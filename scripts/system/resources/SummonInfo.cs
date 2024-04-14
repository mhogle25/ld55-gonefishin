using System.Collections.Generic;
using Godot;

namespace BFO.G.GoneFishin;

public partial class SummonInfo : Resource 
{
	[Export] private string id = string.Empty;
	[Export] private string songId = string.Empty;
	[Export] private PackedScene spritePrefab = null;
	
	public string Id => this.id;
	public PackedScene SpritePrefab => this.spritePrefab;
	public string SongId => this.songId;
}

public static class SummonInfoExtensions 
{
	public static void SetupDictionary(this IEnumerable<SummonInfo> infos, Dictionary<string, SummonInfo> dictionary) 
	{
		dictionary.Clear();
		foreach (SummonInfo info in infos) 
			dictionary[info.Id] = info;
	}
}