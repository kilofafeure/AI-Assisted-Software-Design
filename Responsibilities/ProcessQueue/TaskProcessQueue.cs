using System.Threading.Channels;
using ApplicationLayer;
using Domain;

namespace ProcessQueue;

public sealed class TaskProcessQueue : IQueue, IAsyncDisposable
{
    private static readonly HttpClient _httpClient = new();

    private readonly Channel<TaskItem> _channel =
        Channel.CreateBounded<TaskItem>(new BoundedChannelOptions(100)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = false
        });

    private readonly ISave _save;
    private readonly Task _consumerTask;

    public TaskProcessQueue(ISave save)
    {
        _save = save ?? throw new ArgumentNullException(nameof(save));
        _consumerTask = ProcessQueueAsync();
    }

    public async Task<bool> EnqueueAsync(TaskItem taskItem)
    {
        ArgumentNullException.ThrowIfNull(taskItem);

        if (taskItem.Type != TaskType.HTTP)
        {
            throw new NotSupportedException($"Task type '{taskItem.Type}' is not supported.");
        }

        try
        {
            await _channel.Writer.WriteAsync(taskItem);
            return true;
        }
        catch (ChannelClosedException)
        {
            return false;
        }
    }

    public async ValueTask DisposeAsync()
    {
        _channel.Writer.TryComplete();
        await _consumerTask;
    }

    private async Task ProcessQueueAsync()
    {
        await foreach (var taskItem in _channel.Reader.ReadAllAsync())
        {
            taskItem.Status = await SendHttpGetAsync(taskItem.Data);
            await _save.Save(taskItem);
        }
    }

    private static async Task<bool> SendHttpGetAsync(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var requestUri) ||
            (requestUri.Scheme != Uri.UriSchemeHttp && requestUri.Scheme != Uri.UriSchemeHttps))
        {
            await Console.Error.WriteLineAsync("Data debe contener una URL HTTP o HTTPS absoluta.");
            return false;
        }

        try
        {
            using var response = await _httpClient.GetAsync(
                requestUri,
                HttpCompletionOption.ResponseHeadersRead);
            await Console.Out.WriteLineAsync(
                $"Código de estado HTTP: {(int)response.StatusCode} ({response.StatusCode})");
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException exception)
        {
            await Console.Error.WriteLineAsync($"No se pudo completar la solicitud HTTP: {exception.Message}");
            return false;
        }
        catch (TaskCanceledException exception)
        {
            await Console.Error.WriteLineAsync($"La solicitud HTTP excedió el tiempo de espera: {exception.Message}");
            return false;
        }
    }
}
