using System;

namespace BFO;

public static class FuncTools 
{
	public static void Run<T>(this T it, Action<T> block) 
	{
		if (block is null) 
			return;
		
		block(it);
	}
	
	public static TResult Let<T, TResult>(this T it, Func<T, TResult> block) 
	{
		if (block is null) 
			return default;
		
		return block(it);
	}
	
	public static T Also<T>(this T it, Action<T> block) 
	{
		if (block is null) 
			return it;
		
		block(it);
		return it;
	}
}