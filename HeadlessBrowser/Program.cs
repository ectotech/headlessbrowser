using System.Net;
using HeadlessBrowser.Utilities;
using static HeadlessBrowser.BrowserHandler;

var token = ProgramArgumentUtility.Parse(args, "token");
if (token == null) throw new Exception("No auth token provided.");

var headless = ProgramArgumentUtility.Parse<bool>(args, "headless") ?? true;

var httpListener = new HttpListener();
httpListener.Prefixes.Add("http://localhost:8080/");
httpListener.Start();

while (true)
{
	try
	{
		var context = await httpListener.GetContextAsync();
		Task.Run(() => HandleConnection(context, token, headless));
	}
	catch (Exception exception)
	{
		Console.WriteLine(exception.Message);
	}
}