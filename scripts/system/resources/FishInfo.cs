using System.Collections.Generic;
using Godot;

namespace BFO.G.GoneFishin;

public partial class FishInfo : SummonInfo 
{
	[Export] private Color hue = new();
	
	public Color Hue => this.hue;
}