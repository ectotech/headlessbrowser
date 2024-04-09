using System.Net.WebSockets;

namespace HeadlessBrowser.Utilities;

public static class WebSocketUtility
{
	public static async Task Forward(WebSocket sender, WebSocket receiver)
	{
		var buffer = new byte[1024];
		while (sender.State == WebSocketState.Open && receiver.State == WebSocketState.Open)
		{
			var result = await sender.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
			if (result.MessageType == WebSocketMessageType.Close)
			{
				await receiver.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
				break;
			}

			await receiver.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType,
				result.EndOfMessage, CancellationToken.None);
		}
	}
}