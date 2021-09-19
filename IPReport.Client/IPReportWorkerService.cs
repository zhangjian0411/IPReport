using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class IPReportWorkerService : BackgroundService
{
    private readonly ILogger<IPReportWorkerService> _logger;
    private readonly IPReportOptions _options;
    public IPReportWorkerService(ILogger<IPReportWorkerService> logger, IOptions<IPReportOptions> options)
    {
        _logger = logger;
        _options = options.Value;

        if (string.IsNullOrEmpty(_options.Server))
        {
            throw new ApplicationException("Configuration value at 'IPReport:Server' can not be empty.");
        }
        if (_options.IntervalMinutes <= 0)
        {
            throw new ApplicationException("Configuration value at 'IPReport:IntervalMinutes' must greater than 0.");
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            var ip = await GetPublicIP();
            if (string.IsNullOrEmpty(ip))
            {
                _logger.LogWarning("GetPulicIP return empty.");
            }
            else
            {
                _logger.LogInformation("GetPulicIP return {ip}.", ip);

                await ReportIPToServer(ip);
                _logger.LogInformation("Report public ip [{ip}] to server.", ip);
            }

            await Task.Delay(_options.IntervalMinutes * 60 * 1000);
        }

        _logger.LogInformation("IPReportWorkerService stopped.");
    }

    public async Task<string> GetPublicIP()
    {
        var url = "http://api.k780.com/?app=ip.local";
        var client = new HttpClient();

        var json = await client.GetFromJsonAsync<JsonElement>(url);
        var result = json.GetProperty("result");
        var ip = result.GetProperty("ip").GetString();

        return ip;
    }

    private async Task ReportIPToServer(string ip)
    {
        var url = _options.Server + "/wulei?ip=" + ip;

        var client = new HttpClient();
        var response = await client.PostAsync(url, new StringContent(""));
        response.EnsureSuccessStatusCode();
    }
}