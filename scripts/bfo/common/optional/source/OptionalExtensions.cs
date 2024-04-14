using System;

#nullable enable
namespace BFO
{
	public static class OptionalExtensions
	{
		public static Option<T> ToOption<T>(this T? obj) where T : class =>
			obj is null ? Option<T>.None() : Option<T>.Some(obj);
			
		public static ValueOption<T> ToValueOption<T>(this T obj) where T : struct =>
			ValueOption<T>.Some(obj);
			
		public static ValueOption<T> ToValueOption<T>(this T? obj) where T : struct =>
			obj is null ? ValueOption<T>.None() : ValueOption<T>.Some(obj.GetValueOrDefault());

		public static Option<T> Where<T>(this T? obj, Func<T, bool> predicate) where T : class =>
			obj is not null && predicate(obj) ? Option<T>.Some(obj) : Option<T>.None();

		public static Option<T> WhereNot<T>(this T? obj, Func<T, bool> predicate) where T : class =>
			obj is not null && !predicate(obj) ? Option<T>.Some(obj) : Option<T>.None();
			
		public static Option<string> ToOptionEmpty(this string s) =>
			string.IsNullOrEmpty(s) ? Option<string>.None() : Option<string>.Some(s);
		
		public static Option<string> ToOptionWhitespace(this string s) =>
			string.IsNullOrWhiteSpace(s) ? Option<string>.None() : Option<string>.Some(s);
			
		public static Option<string> WhereNotEmpty(this Option<string> s) =>
			!string.IsNullOrEmpty(s.Content) ? Option<string>.Some(s.Content) : Option<string>.None();
			
		public static Option<string> WhereNotWhiteSpace(this Option<string> s) =>
			!string.IsNullOrWhiteSpace(s.Content) ? Option<string>.Some(s.Content) : Option<string>.None();
	}
}