using System;
using System.Runtime.Serialization;

namespace BFO;

[Serializable]
public enum VDirection 
{
	[EnumMember]
	Up,
	[EnumMember]
	Down
}

public static class VDirectionExtensions 
{	
	public static ValueOption<VDirection> AxisToVDirection(this float axis) => Math.Sign(axis).SignToVDirection();
	
	private static ValueOption<VDirection> SignToVDirection(this int sign) 
	{
		if (sign < 0) return ValueOption<VDirection>.Some(VDirection.Up);
		if (sign > 0) return ValueOption<VDirection>.Some(VDirection.Down);
		
		return ValueOption<VDirection>.None();
	}
	
	public record CharacterDirectionInfo(int Sign, string MoveId, string TurnId);
	
	private static readonly CharacterDirectionInfo upInfo = new(-1, "MOVE_U", "TURN_D_U");
	private static readonly CharacterDirectionInfo downInfo = new(1, "MOVE_D", "TURN_U_D");
	
	public static CharacterDirectionInfo Info(this VDirection direction) => direction switch
	{
		VDirection.Up => VDirectionExtensions.upInfo,
		VDirection.Down => VDirectionExtensions.downInfo,
		_ => default
	};
	
	public static string MoveId(this VDirection direction) => direction.Info().MoveId;
	public static string TurnId(this VDirection direction) => direction.Info().TurnId;
	public static int Sign(this VDirection direction) => direction.Info().Sign;
	public static DirectionalAxis ToDirectionalAxis(this DirectionalAxis _) => DirectionalAxis.Vertical;
}