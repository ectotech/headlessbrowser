using System.Net;
using System.Net.WebSockets;
using HeadlessBrowser.Utilities;
using HeadlessBrowser.Models;

namespace HeadlessBrowser;

public static class BrowserHandler
{
	public static async Task HandleConnection(HttpListenerContext? context, string token, bool headless)
	{
		try
		{
			if (context == null) throw new Exception("Http listener context is null.");

			var requestToken = UrlQueryUtility.Parse(context.Request.Url?.Query ?? "", "token");
			if (requestToken != token)
			{
				context.Response.StatusCode = 401;
				context.Response.Close();
				return;
			}

			var browserApiType =
				UrlQueryUtility.Parse<BrowserApiType>(context.Request.Url?.Query ?? "", "browserApi") ??
				BrowserApiType.Puppeteer;
			var browserType = UrlQueryUtility.Parse<BrowserType>(context.Request.Url?.Query ?? "", "browser") ??
			                  BrowserType.Chrome;

			if (!context.Request.IsWebSocketRequest)
			{
				context.Response.StatusCode = 101;
				context.Response.Close();
				return;
			}

			IBrowser? browser = null;
			switch (browserApiType)
			{
				case BrowserApiType.Playwright:
					throw new Exception("Playwright is not supported as of yet.");
				case BrowserApiType.Puppeteer:
					browser = new PuppeteerBrowser();
					break;
				default:
					throw new Exception("Selected browser API is not supported.");
			}

			if (browser == null) throw new Exception("No browser type was specified.");

			await browser.LaunchAsync(browserType, headless);
			var browserWebSocket = await browser.ConnectAsync();

			var webSocketContext = await context.AcceptWebSocketAsync(null);
			var webSocket = webSocketContext.WebSocket;

			while (webSocket.State == WebSocketState.Open && browserWebSocket.State == WebSocketState.Open)
			{
				await Task.WhenAny(WebSocketUtility.Forward(webSocket, browserWebSocket),
					WebSocketUtility.Forward(browserWebSocket, webSocket));
			}

			await browser.CloseAsync();
		}
		catch (Exception exception)
		{
			Console.WriteLine(exception.Message);
		}
	}
}