using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

public class HeartbeatService : IHostedService, IDisposable
{
    private readonly HttpClient _httpClient;
    private Timer _timer;

    public HeartbeatService()
    {
        _httpClient = new HttpClient();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Inicia o timer para chamar a API a cada 30 segundos
        _timer = new Timer(async _ =>
        {
            await SendHeartbeatRequest();
        }, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));

        return Task.CompletedTask;
    }

    private async Task SendHeartbeatRequest()
    {
        try
        {
            var response = await _httpClient.GetAsync("https://esferas-bot-e3n3.onrender.com/heartbeat");

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("API externa está online.");
            }
            else
            {
                Console.WriteLine($"Erro na requisição: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao realizar requisição para API externa: {ex.Message}");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
        _httpClient?.Dispose();
    }
}
