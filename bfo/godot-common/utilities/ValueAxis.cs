using Godot;

namespace BFO.G;

public abstract partial class ValueAxis : Resource
{
	public ValueAxis() { }
	
	public abstract float Value();
}
