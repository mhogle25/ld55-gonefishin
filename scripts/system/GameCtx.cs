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
	
	public int GetDemonCount() => 
		this.saveData.DemonCount;
	
	public void AddSummon(SummonInfo summonInfo) 
	{
		GetSaveData().AddSummon(summonInfo.Id);
		SaveDataToDisk();
	}
	
	public void IncrementDemonCount() 
	{
		GetSaveData().IncrementDemonCount();
		SaveDataToDisk();
	}
	
	private SaveData GetSaveData() 
	{
		this.saveData ??= JSON.Deserialize<SaveData>(this.saveFileManager.Read(SAVE_ID)).Reduce(new SaveData());
		return this.saveData;
	}
	
	private void SaveDataToDisk() 
	{
		string data = JSON.Serialize(GetSaveData());
		
		if (string.IsNullOrWhiteSpace(data))
			return;
		
		this.saveFileManager.Write(SAVE_ID, data);
	}
}