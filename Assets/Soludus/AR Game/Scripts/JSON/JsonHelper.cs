using System;
using UnityEngine;
using System.Collections;

public static class JsonHelper
{
	public static T[] FromJson<T>(string json)
	{
		Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
		return wrapper.docs;
	}

	public static string ToJson<T>(T[] array)
	{
		Wrapper<T> wrapper = new Wrapper<T>();
		wrapper.docs = array;
		return UnityEngine.JsonUtility.ToJson(wrapper);
	}

	[Serializable]
	private class Wrapper<T>
	{
		public T[] docs;
	}
}