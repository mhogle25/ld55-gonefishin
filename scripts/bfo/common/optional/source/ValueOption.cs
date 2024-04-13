using System;

#nullable enable
namespace BFO;

public struct ValueOption<T> : IEquatable<ValueOption<T>> where T : struct
{
	private T? _content;

	public static ValueOption<T> Some(T content) => new() { _content = content };
	public static ValueOption<T> None() => new();

	public readonly ValueOption<T> IfSome(Action<T> action) { if (_content.HasValue) action(_content.Value); return this; } 
	public readonly ValueOption<T> IfNone(Action action) { if (!_content.HasValue) action(); return this; }
	
	public readonly bool IsSome => _content.HasValue;
	public readonly bool IsNone => !_content.HasValue;
	
	public readonly Option<TResult> Map<TResult>(Func<T, TResult> map) where TResult : class =>
		_content.HasValue ? Option<TResult>.Some(map(_content.Value)) : Option<TResult>.None();
	public ValueOption<TResult> MapValue<TResult>(Func<T, TResult> map) where TResult : struct =>
		new() { _content = _content.HasValue ? map(_content.Value) : null };
		
	public readonly Option<TResult> Replace<TResult>(TResult map) where TResult : class =>
		_content.HasValue ? Option<TResult>.Some(map) : Option<TResult>.None();
	public ValueOption<TResult> ReplaceValue<TResult>(TResult map) where TResult : struct =>
		new() { _content = _content.HasValue ? map : null };

	public readonly Option<TResult> MapOptional<TResult>(Func<T, Option<TResult>> map) where TResult : class =>
		_content.HasValue ? map(_content.Value) : Option<TResult>.None();
	public readonly ValueOption<TResult> MapOptionalValue<TResult>(Func<T, ValueOption<TResult>> map) where TResult : struct =>
		_content.HasValue ? map(_content.Value) : ValueOption<TResult>.None();

	public readonly T? Content => _content;
	
#pragma warning disable CS8629 // Nullable value type may be null.
	public readonly T AssertedContent => (T) _content;
#pragma warning restore CS8629 // Nullable value type may be null.

	public readonly T Reduce(T orElse) => _content ?? orElse;
	public readonly T ReduceTo(Func<T> orElse) => _content ?? orElse();

	public readonly ValueOption<T> Where(Func<T, bool> predicate) =>
		_content.HasValue && predicate(_content.Value) ? this : ValueOption<T>.None();

	public readonly ValueOption<T> WhereNot(Func<T, bool> predicate) =>
		_content.HasValue && !predicate(_content.Value) ? this : ValueOption<T>.None();

	public override readonly int GetHashCode() => _content?.GetHashCode() ?? 0;
	public override readonly bool Equals(object? other) => other is ValueOption<T> option && Equals(option);

	public readonly bool Equals(ValueOption<T> other) =>
		_content.HasValue ? other._content.HasValue && _content.Value.Equals(other._content.Value)
		: !other._content.HasValue;

	public static bool operator ==(ValueOption<T> a, ValueOption<T> b) => a.Equals(b);
	public static bool operator !=(ValueOption<T> a, ValueOption<T> b) => !a.Equals(b);
}