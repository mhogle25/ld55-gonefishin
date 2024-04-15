using System;
using BFO.G.Utilities;
using Godot;

namespace BFO.G.GoneFishin;

public partial class GameCtx : Node
{
	public const string PATH = "/root/GameCtx";
	
	private const string SAVE_ID = "save_01";
	private const string SAVE_DIR = "save";
	private readonly FileManager saveFileManager = new(FilePath.User, SAVE_DIR, "json");
	
	private SaveData saveData = null;

	public override void _Ready()
	{
		string path = $"{FileManager.USER_PATH}{SAVE_DIR}";
		
		if (!DirAccess.DirExistsAbsolute(path))
			DirAccess.MakeDirRecursiveAbsolute(path);
	}

	public int GetDemonCount() => 
		GetSaveData().GetDemonCount();
	
	public void AddSummon(SummonData summon) 
	{
		GetSaveData().AddSummon(summon);
		SaveDataToDisk();
	}
	
	public void IncrementDemonCount() 
	{
		GetSaveData().IncrementDemonCount();
		SaveDataToDisk();
	}
	
	public void DecrementDemonCount() 
	{
		GetSaveData().DecrementDemonCount();
		SaveDataToDisk();
	}
	
	private SaveData GetSaveData() 
	{
		this.saveData ??= JSON
			.Deserialize<SaveData>(this.saveFileManager.Read(SAVE_ID))
			.ReduceTo(() => 
			{
				SaveData newSave = new();
				SaveDataToDisk(newSave);
				BFCtx.Print("Created new save file");
				return newSave;
			});
		return this.saveData;
	}
	
	private void SaveDataToDisk() => SaveDataToDisk(GetSaveData());
	
	private void SaveDataToDisk(SaveData save) 
	{
		string data = JSON.Serialize(save);
		
		if (string.IsNullOrWhiteSpace(data))
			return;
		
		this.saveFileManager.Write(SAVE_ID, data);
	}
}