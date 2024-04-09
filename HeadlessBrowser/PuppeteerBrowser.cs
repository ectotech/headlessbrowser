using System.Net.WebSockets;
using HeadlessBrowser.Models;
using PuppeteerSharp;

namespace HeadlessBrowser;

public class PuppeteerBrowser : IBrowser
{
	private PuppeteerSharp.IBrowser? Browser { get; set; }

	public async Task LaunchAsync(BrowserType browserType, bool headless)
	{
		var browserFetcherOptions = new BrowserFetcherOptions();

		var browserLaunchOptions = new LaunchOptions()
		{
			Headless = headless
		};

		switch (browserType)
		{
			case BrowserType.Chrome:
				browserFetcherOptions.Browser = SupportedBrowser.Chrome;
				browserLaunchOptions.Browser = SupportedBrowser.Chrome;
				break;
			case BrowserType.Firefox:
				browserFetcherOptions.Browser = SupportedBrowser.Firefox;
				browserLaunchOptions.Browser = SupportedBrowser.Firefox;
				break;
			case BrowserType.Chromium:
				browserFetcherOptions.Browser = SupportedBrowser.Chromium;
				browserLaunchOptions.Browser = SupportedBrowser.Chromium;
				break;
			case BrowserType.ChromeHeadlessShell:
				browserFetcherOptions.Browser = SupportedBrowser.ChromeHeadlessShell;
				browserLaunchOptions.Browser = SupportedBrowser.ChromeHeadlessShell;
				break;
			default:
				throw new Exception("Puppeteer does not support selected browser.");
		}

		var browserFetcher = new BrowserFetcher(browserFetcherOptions);
		await browserFetcher.DownloadAsync();

		Browser = await Puppeteer.LaunchAsync(browserLaunchOptions);
	}

	public async Task<WebSocket> ConnectAsync()
	{
		if (Browser == null) throw new Exception("Browser is not launched.");

		if (Browser.WebSocketEndpoint == null) throw new Exception("Failed to get browser web socket endpoint.");
		var browserWebSocketEndpoint = new Uri(Browser.WebSocketEndpoint);

		var browserWebSocket = new ClientWebSocket();
		await browserWebSocket.ConnectAsync(browserWebSocketEndpoint, CancellationToken.None);

		if (browserWebSocket.State != WebSocketState.Open)
			throw new Exception("Failed to connect to browser web socket.");

		return browserWebSocket;
	}

	public async Task CloseAsync()
	{
		if (Browser == null) throw new Exception("Browser is not launched.");

		await Browser.CloseAsync();
	}
}