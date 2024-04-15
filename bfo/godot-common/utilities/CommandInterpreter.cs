using Godot;

namespace BFO.G.Utilities;

public abstract partial class CommandInterpreter : Node
{
	public abstract void ExecuteCommand(string command);
	public abstract Option<string> ProcessCommand(string command);
}