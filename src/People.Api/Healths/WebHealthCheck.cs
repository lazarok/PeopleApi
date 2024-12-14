using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace People.Api.Healths;

public class WebHealthCheck : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public WebHealthCheck(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_configuration["AppUrl"]}/api/persons?pageNumber=2000&pageSize=1", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Healthy("The server is responding.");
            }
            else
            {
                return HealthCheckResult.Unhealthy("The server is not responding properly.");
            }
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"The server check failed: {ex.Message}");
        }
    }
}