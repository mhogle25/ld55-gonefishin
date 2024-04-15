using Godot;
using System;

namespace BFO.G.GoneFishin;

public partial class Wizard : AnimatedSprite2D
{	
	public void SetSpriteDefault() => this.Play("default");
	public void SetSpriteSummon() => this.Play("summon");
	public void SetSpriteFail() => this.Play("fail");

	public override void _Ready()
	{
		this.ZIndex = (int) this.Position.Y;
	}

	public static void HandleBodyEntered(Node2D body) 
	{
		body.QueueFree();
	}
}
