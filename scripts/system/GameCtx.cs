using System.Collections.Generic;
using System.IO;
using BFO.G.Utilities;
using Godot;

namespace BFO.G.GoneFishin;

public partial class ColorInfo : Resource 
{
	public ColorInfo(string code, string name) 
	{
		this.Code = code;
		this.Name = name;
	}
	
	public string Code { get; init; } 
	public string Name { get; init;}
	
	public string GetCode() => this.Code;
	public string GetName() => this.Name;
}

public partial class GameCtx : Node
{
	public const string PATH = "/root/GameCtx";
	
	private const string SAVE_ID = "save_01";
	private const string SAVE_DIR = "save";
	private const string COLOR_DIR = "assets/colors";
	private const string COLOR_CODES_ID = "codes";
	private const string COLOR_NAMES_ID = "names";
	private readonly FileManager saveFileManager = new(FilePath.User, SAVE_DIR, "json");
	private readonly FileManager colorDataFileManager = new(FilePath.Resources, COLOR_DIR, "txt");
	
	private readonly List<ColorInfo> colorInfos = new();
	private readonly RandomNumberGenerator rng = new();
	
	private SaveData saveData = null;

	public override void _Ready()
	{	
		string savePath = $"{FileManager.USER_PATH}{SAVE_DIR}";
		
		if (!DirAccess.DirExistsAbsolute(savePath))
			DirAccess.MakeDirRecursiveAbsolute(savePath);
			
		string codesFile = this.colorDataFileManager.Read(COLOR_CODES_ID);
		string namesFile = this.colorDataFileManager.Read(COLOR_NAMES_ID);
		
		StringReader codesReader = new(codesFile);
		StringReader namesReader = new(namesFile);
		
		string code = codesReader.ReadLine();
		string name = namesReader.ReadLine();
		while (code != null && name != null) 
		{
			this.colorInfos.Add(new ColorInfo(code, name));
			code = codesReader.ReadLine();
			name = namesReader.ReadLine();
		}
			
		if ((code != null && name == null) || (code == null && name != null))
			BFCtx.PrintErr("Color files must have the same number of lines");
			
		GetSaveData();
	}

	public int GetDemonCount() => 
		GetSaveData().GetDemonCount();
	
	public bool GetEncounteredBoss() =>
		GetSaveData().GetEncounteredBoss();
		
	public IEnumerable<SummonData> GetSummons() =>
		GetSaveData().GetSummons();
	
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
	
	public void EncounterBoss() => 
		GetSaveData().EncounterBoss();
		
	public float GetMusicVol() => this.saveData.MusicVol; 
	public float GetMasterVol() => this.saveData.MasterVol;
	public bool GetLimitMouseMovement() => this.saveData.LimitMouseMovement;
	public int GetDifficultySelected() => this.saveData.DifficultySelected; 
	public bool GetFullscreen() => this.saveData.Fullscreen; 
	public void SetMusicVol(float value) 
	{
		this.saveData.MusicVol = value; 
		SaveDataToDisk();
	}
	public void SetMasterVol(float value) 
	{
		this.saveData.MasterVol = value; 
		SaveDataToDisk();
	}
	public void SetLimitMouseMovement(bool value)
	{
		this.saveData.LimitMouseMovement = value; 
		SaveDataToDisk();
	}
	public void SetDifficultySelected(int value) 
	{
		this.saveData.DifficultySelected = value;
		SaveDataToDisk();
	}
	public void SetFullscreen(bool value) 
	{
		this.saveData.Fullscreen = value;
		SaveDataToDisk();
	}

	public ColorInfo GetRandColorInfo() 
	{
		return this.colorInfos[this.rng.RandiRange(0, this.colorInfos.Count - 1)];
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
