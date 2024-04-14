using Godot;

namespace BFO.G
{
	public static class BFCtx 
	{ 
		const string WARNING_COLOR = "#eba434";
		
		public static void Print(object message) => GD.Print(message);
		
		public static void PrintWarn(object warning) 
		{
			GD.PushWarning(warning);
			GD.PrintRich($"[color:{WARNING_COLOR}]{warning}[/color]");
		}
		
		public static void PrintErr(object error) 
		{
			GD.PushError(error);
			GD.PrintErr(error);
		}
		
		public static void PrintIf(bool condition, object message) { if (condition) Print(message); }
		public static void PrintWarnIf(bool condition, object message) { if (condition) PrintWarn(message); }
		public static void PrintErrIf(bool condition, object message) { if (condition) PrintErr(message); }
		
		public static void Print(params object[] message) => GD.Print(message);
		
		public static void PrintWarn(params object[] warning) 
		{
			GD.PushWarning(warning);
			GD.PrintRich($"[color:{WARNING_COLOR}]{warning}[/color]");
		}
		
		public static void PrintErr(params object[] error) 
		{
			GD.PushError(error);
			GD.PrintErr(error);
		}
		
		public static void PrintIf(bool condition, params object[] message) { if (condition) Print(message); }
		public static void PrintWarnIf(bool condition, params object[] message) { if (condition) PrintWarn(message); }
		public static void PrintErrIf(bool condition, params object[] message) { if (condition) PrintErr(message); }
	}
}