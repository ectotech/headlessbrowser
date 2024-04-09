namespace HeadlessBrowser.Utilities;

public static class ProgramArgumentUtility
{
	private static string GetValue(IEnumerable<string>? args, string name)
	{
		if (args == null) throw new Exception("No args provided.");

		var arg = $"--{name}=";
		var value = args.FirstOrDefault(i => i.StartsWith(arg))
			?.Replace(arg, string.Empty);

		if (value == null) throw new Exception($"No argument provided for {name}.");
		return value;
	}

	public static string? Parse(IEnumerable<string>? args, string name)
	{
		try
		{
			return GetValue(args, name);
		}
		catch
		{
			return null;
		}
	}

	public static TEnum? Parse<TEnum>(IEnumerable<string>? args, string name) where TEnum : struct
	{
		try
		{
			return Enum.Parse<TEnum>(GetValue(args, name));
		}
		catch
		{
			return null;
		}
	}
}