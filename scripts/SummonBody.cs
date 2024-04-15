using System;
using Godot;

namespace BFO.G.GoneFishin;


public partial class SummonBody : CharacterBody2D
{
	enum State 
	{ 
		Idle,
		Tugging,
		Caught,
		Fleeing
	}
	
	[Export] int tugSpeed = 80;
	[Export] int tugJumpDistance = 20;
	[Export] int chanceToTugBack = 15;
	
	SummonSprite sprite = null;
	State state = State.Idle;
	RandomNumberGenerator rng = new();
	
	public void Setup(SummonSprite sprite) 
	{
		AddChild(sprite);
		sprite.Position = new Vector2(0, 0);
		this.sprite = sprite;
		this.state = State.Tugging;
	}

	public override void _Process(double delta) => GetProcessAction()?.Invoke((float) delta);
	
	public void Catch() => this.state = State.Caught;
	public void Flee() => this.state = State.Fleeing;
	
	private Action<float> GetProcessAction() => this.state switch 
	{
		State.Tugging => ProcessTugging,
		State.Caught => ProcessCaught,
		State.Fleeing => ProcessFleeing,
		_ => null
	};
	
	private void ProcessTugging(float delta) 
	{
		MoveAndCollide(new(0, -tugSpeed * delta));
		if (this.rng.RandiRange(0, this.chanceToTugBack) == 0)
			this.Position += new Vector2(0, this.tugJumpDistance);
	}
	
	private void ProcessCaught(float delta) 
	{
		
	}
	
	private void ProcessFleeing(float delta) 
	{
		
	}
}
