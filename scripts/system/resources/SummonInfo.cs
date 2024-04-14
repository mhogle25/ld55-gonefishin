using System.Collections.Generic;
using Godot;

namespace BFO.G.GoneFishin;

public partial class SummonInfo : Resource 
{
	[Export] private string id = string.Empty;
	[Export] private string name = string.Empty;
	[Export] private string songId = string.Empty;
	[Export] private PackedScene spritePrefab = null;
	
	public string Id => this.id;
	
	public SummonSprite InstantiateSprite() 
	{
		return InstantiateSprite(new Color("#ffffff"));
	}
	
	public SummonSprite InstantiateSprite(Color color) 
	{
		SummonSprite sprite = this.spritePrefab.Instantiate<SummonSprite>();
		sprite.Modulate = color;
		return sprite;
	}
	
	public string GetName() => this.name;
	public string SongId => this.songId;
}

public static class SummonInfoExtensions 
{
	public static void SetupDictionary<T>(this IEnumerable<T> infos, Dictionary<string, T> dictionary) where T : SummonInfo
	{
		foreach (T info in infos) 
			dictionary[info.Id] = info;
	}
}