using Godot;

namespace BFO.G.GoneFishin;

public partial class SummonDisplay : Node2D
{	
	[Export] Container tooltipContainer = null;
	[Export] Label tooltipLabel = null;
	[Export] Timer timer = null;
	
	public SummonSprite Sprite { get; private set; }= null;

	public override void _Ready()
	{
		foreach (Node node in GetChildren())
			if (node is SummonSprite sprite) 
			{
				SetupInternal(sprite, sprite.Name);
				return;
			}
	}

	public void Setup(SummonSprite sprite, string text) 
	{
		SetupInternal(sprite, text);	
		AddChild(sprite);
	}
	
	private void SetupInternal(SummonSprite sprite, string text) 
	{
		sprite.Position = new Vector2(0, 0);
		sprite.Hitbox.MouseEntered += OnMouseEnter;
		sprite.Hitbox.MouseExited += OnMouseExit;
		
		this.Sprite = sprite;
		this.tooltipLabel.Text = text;
	}
	
	public void ShowTooltip() 
	{
		this.tooltipContainer.GlobalPosition = GetViewport().GetMousePosition();
		this.tooltipContainer.Visible = true;
	}
	
	private void OnMouseEnter() 
	{
		this.timer.Start();
	}
	
	private void OnMouseExit() 
	{
		this.timer.Stop();
		this.tooltipContainer.Visible = false;
	}
	
}