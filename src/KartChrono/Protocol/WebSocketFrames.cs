using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;
using System.Text;
using Pure.Primitives.Abstractions.String;

namespace KartChrono.Protocol;

[ExcludeFromCodeCoverage]
public sealed record WebSocketFrames : IAsyncEnumerable<ReadOnlyMemory<byte>>
{
    private const string Endpoint = "wss://kartchrono.com:9180";

    private const int BufferSize = 16 * 1024;

    private readonly IString _trackId;

    private readonly TimeSpan _reconnectDelay;

    public WebSocketFrames(IString trackId)
        : this(trackId, TimeSpan.FromSeconds(5)) { }

    public WebSocketFrames(IString trackId, TimeSpan reconnectDelay)
    {
        _trackId = trackId;
        _reconnectDelay = reconnectDelay;
    }

    public async IAsyncEnumerator<ReadOnlyMemory<byte>> GetAsyncEnumerator(
        CancellationToken cancellationToken = default
    )
    {
        byte[] buffer = new byte[BufferSize];

        while (!cancellationToken.IsCancellationRequested)
        {
            using ClientWebSocket socket = new ClientWebSocket();

            await socket.ConnectAsync(new Uri(Endpoint), cancellationToken);

            await socket.SendAsync(
                Encoding.UTF8.GetBytes($"{{\"trackId\":\"{_trackId.TextValue}\"}}"),
                WebSocketMessageType.Text,
                true,
                cancellationToken
            );

            while (socket.State == WebSocketState.Open)
            {
                using MemoryStream frame = new MemoryStream();
                WebSocketReceiveResult received;

                do
                {
                    received = await socket.ReceiveAsync(buffer, cancellationToken);

                    if (received.MessageType == WebSocketMessageType.Close)
                    {
                        break;
                    }

                    frame.Write(buffer, 0, received.Count);
                } while (!received.EndOfMessage);

                if (received.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }

                yield return frame.ToArray();
            }

            await Task.Delay(_reconnectDelay, cancellationToken);
        }
    }

    public override int GetHashCode()
    {
        throw new NotSupportedException();
    }

    public override string ToString()
    {
        throw new NotSupportedException();
    }
}
