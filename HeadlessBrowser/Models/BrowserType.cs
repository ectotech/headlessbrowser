using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace HeadlessBrowser.Models;

[JsonConverter(typeof(StringEnumConverter))]
public enum BrowserType
{
	[EnumMember(Value = "chrome")] Chrome = 0,

	[EnumMember(Value = "firefox")] Firefox = 1,

	[EnumMember(Value = "chromium")] Chromium = 2,

	[EnumMember(Value = "chromeHeadlessShell")]
	ChromeHeadlessShell = 3,

	[EnumMember(Value = "webkit")] Webkit = 4
}