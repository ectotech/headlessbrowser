using HeadlessBrowser.Models;
using System.Net.WebSockets;

namespace HeadlessBrowser;

public interface IBrowser
{
	public Task LaunchAsync(BrowserType browserType, bool headless);

	public Task<WebSocket> ConnectAsync();

	public Task CloseAsync();
}