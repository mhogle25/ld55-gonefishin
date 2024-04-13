using System;
using BFO.G.Utilities;
using Godot;

namespace BFO.G.GoneFishin;

public partial class GameCtx : Node
{
	public const string PATH = "/root/GameCtx";
	
	
	private const string SAVE_ID = "save_01";
	private readonly FileManager saveFileManager = new(FilePath.User, "save", "json");
	
	public SaveData GetSaveData() 
	{
		throw new NotImplementedException();
	}
}