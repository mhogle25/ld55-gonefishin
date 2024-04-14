using System;
using System.Runtime.Serialization;

namespace BFO;

[Serializable]
public enum HDirection 
{
	[EnumMember]
	Left,
	[EnumMember]
	Right
}

public static class HDirectionExtensions 
{	
	public static ValueOption<HDirection> AxisToHDirection(this float axis) => Math.Sign(axis).SignToHDirection();
	
	private static ValueOption<HDirection> SignToHDirection(this int sign) 
	{
		if (sign < 0) return ValueOption<HDirection>.Some(HDirection.Left);
		if (sign > 0) return ValueOption<HDirection>.Some(HDirection.Right);
		
		return ValueOption<HDirection>.None();
	}
	
	public record struct CharacterDirectionInfo(int Sign, string MoveId, string TurnId);
	
	private static readonly CharacterDirectionInfo leftInfo = new(-1, "MOVE_L", "TURN_R_L");
	private static readonly CharacterDirectionInfo rightInfo = new(1, "MOVE_R", "TURN_L_R");
	
	public static CharacterDirectionInfo Info(this HDirection direction) => direction switch
	{
		HDirection.Left => HDirectionExtensions.leftInfo,
		HDirection.Right => HDirectionExtensions.rightInfo,
		_ => default
	};
	
	public static string MoveId(this HDirection direction) => direction.Info().MoveId;
	public static string TurnId(this HDirection direction) => direction.Info().TurnId;
	public static int Sign(this HDirection direction) => direction.Info().Sign;
	public static DirectionalAxis ToDirectionalAxis(this HDirection _) => DirectionalAxis.Horizontal;
}