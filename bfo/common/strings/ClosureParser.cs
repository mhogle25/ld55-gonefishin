using System;

namespace BFO;

public class ClosureParser
{	
	public char OpeningBracket { get; init; }
	public char ClosingBracket { get; init; }
	public bool Trim { get; init; }
	public ValueOption<char> Prefix { get; init; }
	
	public ClosureParser(char openingBracket, char closingBracket, bool trim = false, char? prefixSymbol = null) 
	{
		this.OpeningBracket = openingBracket;
		this.ClosingBracket = closingBracket;
		this.Trim = trim;
		this.Prefix = prefixSymbol.ToValueOption();
	}
	
	bool closureFound = false;
	int deckCount = 0;
	bool EmptyDeck => this.deckCount < 1;
	bool Uno => this.deckCount == 1;
	bool MultiCardDeck => this.deckCount > 1;
	
	/// <param name="text">The text to parse</param>
	/// <param name="startingIndex">The index to start parsing from</param>
	/// <param name="onAccumulate">Called once for every character in the closure. If trimming is enabled, this won't be called if the character is one of the closure's brackets or the closure's bracket prefix</param>
	/// <param name="onEndOfClosure">Called when the end of the first closure is reached</param>
	/// <param name="onNoClosuresFound">Called if the end of the text is reached and no closures were found</param>
	/// <returns>The index of the text the closure ends on, or the starting index if no closures are found</returns>
	/// <exception cref="ArgumentException"></exception>
	/// <exception cref="InvalidOperationException"></exception>
	public int ParseGetFirst(string text, int startingIndex, Action<char> onAccumulate, Action onEndOfClosure = null, Action onNoClosuresFound = null) 
	{
		if (onAccumulate is null)
			throw new ArgumentException($"[ClosureParser:ParseFirst] onAccumulate callback cannot be null");
		
		if (startingIndex < 0 || startingIndex > text.Length)
			throw new ArgumentException($"[ClosureParser:ParseFirst] Index was outside the bounds of the text -> index: {startingIndex}, text.Length: {text.Length}");
		
		Clear();
		
		int i = startingIndex;
		
		for (; i < text.Length; i++) 
		{
			char currentCharacter = text[i];
			
			bool TryParsePrefixOpening(char prefix) 
			{
				if (currentCharacter != prefix || (i + 1 < text.Length && text[i + 1] != this.OpeningBracket)) 
					return false;
				
				char next = text[++i];
				
				if (!this.EmptyDeck || !this.Trim)
				{
					onAccumulate(currentCharacter);
					onAccumulate(next);
				}
				
				PushOne();
				
				return true;
			}
			
			if (this.Prefix.MapValue(TryParsePrefixOpening).Reduce(false))
				continue;
			
			if (currentCharacter == this.ClosingBracket && (this.OpeningBracket != this.ClosingBracket || this.Uno))
			{
				if (this.EmptyDeck)
					throw new ArgumentException($"[ClosureParser:ParseOne] Expected string character, whitespace character or end of text but found '{this.ClosingBracket}'");
				
				if (this.MultiCardDeck || (this.Uno && !this.Trim))
					onAccumulate(currentCharacter);
				
				PopOne();
				
				if (this.EmptyDeck)
					break;
					
				continue;
			} 
			
			if (this.Prefix.IsNone && currentCharacter == this.OpeningBracket) 
			{
				if (!this.Trim)
					onAccumulate(currentCharacter);
					
				PushOne();
				continue;
			}
			
			if (!this.EmptyDeck)
				onAccumulate(currentCharacter);
		}
		
		if (!this.EmptyDeck)
			throw new InvalidOperationException($"[ClosureParser:ParseFirst] Syntax error: expected '{this.ClosingBracket}'. Deck Count: {this.deckCount}");
		
		if (this.closureFound) 
		{
			onEndOfClosure?.Invoke();
			return i;	
		}
			
		onNoClosuresFound?.Invoke();
		return startingIndex;
	}
	
	void PushOne() 
	{
		this.closureFound = true;
		this.deckCount++;
	}
	
	void PopOne() => this.deckCount--;
	
	void Clear() 
	{
		this.closureFound = false;
		this.deckCount = 0;
	}
}