using System;
using System.Runtime.Serialization;

namespace BFO.G.Utilities 
{
	[Serializable]
	public enum FilePath 
	{
		[EnumMember]
		Both,
		[EnumMember]
		User,
		[EnumMember]
		Resources
	}
}