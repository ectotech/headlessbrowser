using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace HeadlessBrowser.Models;

[JsonConverter(typeof(StringEnumConverter))]
public enum BrowserApiType
{
	[EnumMember(Value = "puppeteer")] Puppeteer = 0,

	[EnumMember(Value = "playwright")] Playwright = 1
}