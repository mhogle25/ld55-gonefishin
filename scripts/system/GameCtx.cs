using System;
using BFO.G.Utilities;
using Godot;

namespace BFO.G.GoneFishin;

public partial class GameCtx : Node
{
	public const string PATH = "/root/GameCtx";
	
	private const string SAVE_ID = "save_01";
	private readonly FileManager saveFileManager = new(FilePath.User, "save", "json");
	
	private SaveData saveData = null;

	public override void _Ready()
	{
		GetSaveData();
	}

	public SaveData GetSaveData() 
	{
		this.saveData ??= JSON.Deserialize<SaveData>(this.saveFileManager.Read(SAVE_ID)).Reduce(new SaveData());
		return this.saveData;
	}
	
	public void AddSummon(Summon summon) 
	{
		this.saveData.AddSummon(summon);
		SaveDataToDisk();
	}
	
	public void AddDemon(Demon demon) 
	{
		this.saveData.AddDemon(demon);
		AddSummon(demon);
		SaveDataToDisk();
	}
	
	private void SaveDataToDisk() 
	{
		string data = JSON.Serialize(this.saveData);
		
		if (string.IsNullOrWhiteSpace(data))
			return;
		
		this.saveFileManager.Write(SAVE_ID, data);
	}
}