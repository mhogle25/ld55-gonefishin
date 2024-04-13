using System;

#nullable enable
namespace BFO;

public struct Option<T> : IEquatable<Option<T>> where T : class
{
	private T? _content;

	public static Option<T> Some(T content) => new() { _content = content };
	public static Option<T> None() => new();

	public readonly Option<T> IfSome(Action<T> action) { if (_content is not null) action(_content); return this; } 
	public readonly Option<T> IfNone(Action action) { if (_content is null) action(); return this; }
	
	public readonly bool IsSome => _content is not null;
	public readonly bool IsNone => _content is null;
	
	public Option<TResult> Map<TResult>(Func<T, TResult> map) where TResult : class =>
		new() { _content = _content is not null ? map(_content) : null };
	public readonly ValueOption<TResult> MapValue<TResult>(Func<T, TResult> map) where TResult : struct =>
		_content is not null ? ValueOption<TResult>.Some(map(_content)) : ValueOption<TResult>.None();
		
	public Option<TResult> Replace<TResult>(TResult map) where TResult : class =>
		new() { _content = _content is not null ? map : null };
	public readonly ValueOption<TResult> ReplaceValue<TResult>(TResult map) where TResult : struct =>
		_content is not null ? ValueOption<TResult>.Some(map) : ValueOption<TResult>.None();

	public readonly Option<TResult> MapOptional<TResult>(Func<T, Option<TResult>> map) where TResult : class =>
		_content is not null ? map(_content) : Option<TResult>.None();
	public readonly ValueOption<TResult> MapOptionalValue<TResult>(Func<T, ValueOption<TResult>> map) where TResult : struct =>
		_content is not null ? map(_content) : ValueOption<TResult>.None();

	public readonly T? Content => _content;
	
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Nullable value type may be null.
	public readonly T AssertedContent => (T) _content;
#pragma warning restore CS8600 // Nullable value type may be null.
#pragma warning restore CS8603 // Possible null reference return.
	
	public readonly T Reduce(T orElse) => _content ?? orElse;
	public readonly T ReduceTo(Func<T> orElse) => _content ?? orElse();

	public readonly Option<T> Where(Func<T, bool> predicate) =>
		_content is not null && predicate(_content) ? this : Option<T>.None();

	public readonly Option<T> WhereNot(Func<T, bool> predicate) =>
		_content is not null && !predicate(_content) ? this : Option<T>.None();

	public override readonly int GetHashCode() => _content?.GetHashCode() ?? 0;
	public override readonly bool Equals(object? other) => other is Option<T> option && Equals(option);

	public readonly bool Equals(Option<T> other) =>
		_content is null ? other._content is null
		: _content.Equals(other._content);

	public static bool operator ==(Option<T>? a, Option<T>? b) => a is null ? b is null : a.Equals(b);
	public static bool operator !=(Option<T>? a, Option<T>? b) => !(a == b);
}