using System.Collections.Generic;
using Godot;

namespace BFO.G;

public abstract partial class Predicate : Resource
{
	public abstract bool Condition();
}

public static class PredicateExtensions 
{
	public static bool OrCondition(this IEnumerable<Predicate> predicates) 
	{
		bool result = false;
		foreach (Predicate predicate in predicates)
			result = result || predicate.Condition();
		return result;
	}
	
	public static bool AndCondition(this IEnumerable<Predicate> predicates) 
	{
		bool result = false;
		foreach (Predicate predicate in predicates)
			result = result && predicate.Condition();
		return result;
	}
}