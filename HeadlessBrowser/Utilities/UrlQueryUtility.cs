namespace HeadlessBrowser.Utilities;

public static class UrlQueryUtility
{
	private static string GetValue(string query, string name)
	{
		var queryParams = query.Replace("?", ";")
			.Replace("&", ";")
			.Split(";");

		var queryParam = $"{name}=";
		var value = queryParams.FirstOrDefault(i => i.StartsWith(queryParam))
			?.Replace(queryParam, "");

		if (value == null) throw new Exception($"No query parameter provided for {name}.");
		return value;
	}

	public static string? ParseString(string query, string name)
	{
		try
		{
			return GetValue(query, name);
		}
		catch
		{
			return null;
		}
	}

	public static TEnum? Parse<TEnum>(string query, string name) where TEnum : struct
	{
		try
		{
			return Enum.Parse<TEnum>(GetValue(query, name));
		}
		catch
		{
			return null;
		}
	}
}