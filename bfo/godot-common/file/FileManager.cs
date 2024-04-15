using System.IO;
using Godot;

namespace BFO.G.Utilities 
{
	public class FileManager 
	{
		const bool DEBUG = false;

		public const string USER_PATH = "user://";
		public const string RESOURCE_PATH = "res://";
		
		public FileManager(FilePath mode, string path, string extension) 
		{
			this.mode = mode;
			this.path = path;
			this.extension = extension;
		}
		
		private readonly FilePath mode = FilePath.User;
		private readonly string path = string.Empty;
		private readonly string extension = string.Empty;
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="content"></param>
		/// <returns>True if successful, otherwise false</returns>
		public bool Write(string id, string content) 
		{
			BFCtx.PrintIf(DEBUG, $"{USER_PATH} -> {ProjectSettings.GlobalizePath(CreatePath(USER_PATH, id))}");
			BFCtx.PrintIf(DEBUG, $"{RESOURCE_PATH} -> {ProjectSettings.GlobalizePath(CreatePath(RESOURCE_PATH, id))}");
			
			Godot.FileAccess resourceFile = OpenFileRead(RESOURCE_PATH, id);
			
			if (resourceFile is not null) 
			{
				BFCtx.Print($"[FileManager:WriteToFile] A resource file already exists with that ID -> {id}");
				return false;
			}
			
			if (this.mode == FilePath.Resources) 
			{
				BFCtx.Print($"[FileManager:WriteToFile] Cannot write to a resource file (they are read-only) -> {id}");
				return false;
			}
			
			using Godot.FileAccess file = OpenFileWrite(USER_PATH, id);
			
			if (file is null) 
			{
				BFCtx.PrintErr($"[FileManager:WriteToFile] File opening failed for id '{id}' -> ${Godot.FileAccess.GetOpenError()}");
				return false;
			}
			
			file.StoreString(content);
			return true;
		}
		
		public string Read(string id) 
		{
			BFCtx.PrintIf(DEBUG, $"{USER_PATH} -> {ProjectSettings.GlobalizePath(CreatePath(USER_PATH, id))}");
			BFCtx.PrintIf(DEBUG, $"{RESOURCE_PATH} -> {ProjectSettings.GlobalizePath(CreatePath(RESOURCE_PATH, id))}");

			if (string.IsNullOrWhiteSpace(id))
				return string.Empty;
			
			using Godot.FileAccess userFile = OpenFileRead(USER_PATH, id);
			using Godot.FileAccess resourceFile = OpenFileRead(RESOURCE_PATH, id);
			
			string GetText(Godot.FileAccess file, string type) 
			{
				if (file is null) 
				{
					BFCtx.PrintErr($"[FileManager:Read] {type} file at id '{id}' does not exist");
					return string.Empty;
				}
				return file.GetAsText();
			}
			
			switch (this.mode) 
			{
				case FilePath.User: return GetText(userFile, "User");
				
				case FilePath.Resources: return GetText(resourceFile, "Resource");
				
				case FilePath.Both:
					if (userFile is not null && resourceFile is not null) 
					{
						BFCtx.PrintErr($"[FileManager:LoadFile] Two files with the same id exist -> {id}");
						return string.Empty;
					}
						
					if (userFile is not null)
						return userFile.GetAsText();
						
					return GetText(resourceFile, "Resource/User");
					
				default: 
					BFCtx.PrintErr($"[FileManager:Read] Mode '{this.mode}' was invalid");
					return string.Empty;
			}
		}
		
		private Godot.FileAccess OpenFileRead(string pathPrefix, string id) => 
			Godot.FileAccess.Open(CreatePath(pathPrefix, id), Godot.FileAccess.ModeFlags.Read);
			
		private Godot.FileAccess OpenFileWrite(string pathPrefix, string id) => 
			Godot.FileAccess.Open(CreatePath(pathPrefix, id), Godot.FileAccess.ModeFlags.Write);
		
		private string CreatePath(string pathPrefix, string id) =>
			Path.Combine(pathPrefix, this.path, $"{id}.{extension}");
	}
}
