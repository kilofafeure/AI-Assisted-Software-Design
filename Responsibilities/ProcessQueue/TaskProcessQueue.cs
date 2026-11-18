using ApplicationLayer;
using Domain;

namespace ProcessQueue;

public class TaskProcessQueue : IQueue
{
    private static readonly HttpClient _httpClient = new();

    public async Task EnqueueAsync(TaskItem taskItem)
    {
        ArgumentNullException.ThrowIfNull(taskItem);

        switch (taskItem.Type)
        {
            case TaskType.HTTP:
                await SendHttpGetAsync(taskItem.Data);
                break;
            default:
                throw new NotSupportedException($"Task type '{taskItem.Type}' is not supported.");
        }
    }

    private static async Task SendHttpGetAsync(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var requestUri) ||
            (requestUri.Scheme != Uri.UriSchemeHttp && requestUri.Scheme != Uri.UriSchemeHttps))
        {
            throw new ArgumentException("Data debe contener una URL HTTP o HTTPS absoluta.", nameof(url));
        }

        using var response = await _httpClient.GetAsync(
            requestUri,
            HttpCompletionOption.ResponseHeadersRead);
        await Console.Out.WriteLineAsync(
            $"Código de estado HTTP: {(int)response.StatusCode} ({response.StatusCode})");
        response.EnsureSuccessStatusCode();
    }
}
