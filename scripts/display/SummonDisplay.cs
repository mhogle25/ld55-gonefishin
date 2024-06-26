using Godot;

namespace BFO.G.GoneFishin;

public partial class SummonDisplay : Node2D
{	
	[Export] Container tooltipContainer = null;
	[Export] Label tooltipLabel = null;
	[Export] Timer timer = null;
	
	private SummonSprite sprite = null;
	
	public SummonSprite GetSprite() => this.sprite;

	public void Setup(SummonSprite sprite, string text) 
	{
		SetupInternal(sprite, text);	
		AddChild(sprite);
	}
	
	private void SetupInternal(SummonSprite sprite, string text) 
	{
		sprite.Position = new Vector2(0, 0);
		BFCtx.Print($"Signal connection length: {sprite.Hitbox.GetSignalConnectionList("MouseEntered").Count}");
		
		sprite.Hitbox.MouseEntered += OnMouseEnter;
		sprite.Hitbox.MouseExited += OnMouseExit;
		
		this.sprite = sprite;
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
