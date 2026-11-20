using ApplicationLayer;
using Domain;

namespace ProcessQueue.Strategies;

public sealed class HttpTaskStrategy : ITaskStrategy
{
    private static readonly HttpClient _httpClient = new();

    public TaskType Type => TaskType.HTTP;

    public async Task<bool> ExecuteAsync(TaskItem taskItem)
    {
        ArgumentNullException.ThrowIfNull(taskItem);

        if (!Uri.TryCreate(taskItem.Data, UriKind.Absolute, out var requestUri) ||
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
