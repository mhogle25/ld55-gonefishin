using System;

namespace BFO;
public static partial class StringExtensions 
{
	public static string ToStringModifier(this int value)
	{
		if (value > 0)
			return $"+{value}";


		if (value < 0)
			return $"-{Math.Abs(value)}";

		return null;
	}
}