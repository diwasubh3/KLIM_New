using System.Collections.Generic;

namespace YCM.CLO.DataAccess.Extensions
{
	public static class DictionaryExtensions
	{
		public static void UpdateKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary
			, TKey oldKey, TKey newKey)
		{
			dictionary[newKey] = dictionary[oldKey];
			dictionary.Remove(oldKey);
		}
	}
}
