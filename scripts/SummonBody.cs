using System;
using System.Diagnostics.Metrics;
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
	[Export] Timer tugTimer = null;
	[Export] int captureSpeed = 200;
	[Export] int fleeTimeout = 2;
	
	SummonSprite sprite = null;
	Node2D target = null;
	State state = State.Idle;
	RandomNumberGenerator rng = new();
	Timer deathTimer = null;
	int tugCoefficient = 1;
	
	public void Setup(SummonSprite sprite, Node2D target) 
	{
		AddChild(sprite);
		sprite.Position = new Vector2(0, 0);
		this.sprite = sprite;
		this.state = State.Tugging;
		this.target = target;
		
		this.deathTimer = new() { OneShot = true, WaitTime = this.fleeTimeout };
		AddChild(this.deathTimer);
		this.deathTimer.Timeout += QueueFree;
	}

	public override void _Process(double delta) 
	{
		GetProcessAction()?.Invoke((float) delta);	
		this.ZIndex = (int) this.Position.Y;
	}
	
	public void Catch() => this.state = State.Caught;
	public void Flee() 
	{
		this.state = State.Fleeing;
		this.deathTimer.Start();
	}
	
	public void TugTimeout() 
	{
		this.tugCoefficient *= -1;
	}
	
	private Action<float> GetProcessAction() => this.state switch 
	{
		State.Tugging => ProcessTugging,
		State.Caught => ProcessCaught,
		State.Fleeing => ProcessFleeing,
		_ => null
	};
	
	private void ProcessTugging(float delta) 
	{
		MoveAndCollide(new(0, this.tugCoefficient * tugSpeed * delta));
	}
	
	private void ProcessCaught(float delta) 
	{
		MoveAndCollide(this.GlobalPosition.DirectionTo(this.target.GlobalPosition) * this.captureSpeed * delta);
	}
	
	private void ProcessFleeing(float delta) 
	{
		MoveAndCollide(new(0, tugSpeed * delta));
	}
}
