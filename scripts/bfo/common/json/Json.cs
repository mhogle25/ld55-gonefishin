using Newtonsoft.Json;
using System;

namespace BFO;

public static class JSON
{
	public static Option<T> Deserialize<T>(string json, Action<object> logger = null) where T : class
	{
		T t;
		try
		{
			t = JsonConvert.DeserializeObject<T>(json, new Newtonsoft.Json.Converters.StringEnumConverter());
		}
		catch (Exception x)
		{
			logger?.Invoke($"[JSON:DeserializeObject] Tried to deserialize JSON but it was not valid.");
			logger?.Invoke(x.Message);
			return Option<T>.None();
		}
		return Option<T>.Some(t);
	}
	
	public static ValueOption<T> DeserializeValue<T>(string json, Action<object> logger = null) where T : struct 
	{
		T t;
		try 
		{
			t = JsonConvert.DeserializeObject<T>(json, new Newtonsoft.Json.Converters.StringEnumConverter());
		}
		catch (Exception x) 
		{
			logger?.Invoke($"[JSON:DeserializeValue] Tried to deserialize JSON but it was not valid.");
			logger?.Invoke(x.Message);
			return ValueOption<T>.None();
		}
		return ValueOption<T>.Some(t);
	}

	public static string Serialize<T>(T obj, Action<object> logger = null)
	{
		string t;
		try
		{
			t = JsonConvert.SerializeObject(obj, Formatting.Indented, new Newtonsoft.Json.Converters.StringEnumConverter());
		}
		catch (Exception x)
		{
			logger?.Invoke($"[JSON:SerializeObject] Tried to serialize JSON but it was not valid.");
			logger?.Invoke(x.Message);
			return string.Empty;
		}
		return t;
	}
}