using System;
using System.Runtime.Serialization;

namespace BFO;

[Serializable]
public enum DirectionalAxis 
{
	[EnumMember]
	Horizontal,
	[EnumMember]
	Vertical
};