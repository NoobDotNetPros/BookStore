using System.Net.Sockets;
using Bookstore.Business.Models;
using Bookstore.DataAccess.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Bookstore.Web.Controllers;

[ApiController]
[Route("api/health")]
[Tags("System")]
public class HealthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly SmtpSettings _smtpSettings;

    public HealthController(
        ApplicationDbContext context,
        IConfiguration configuration,
        IOptions<SmtpSettings> smtpSettings
    )
    {
        _context = context;
        _configuration = configuration;
        _smtpSettings = smtpSettings.Value;
    }

    [HttpGet]
    public async Task<IActionResult> CheckHealth()
    {
        var status = new Dictionary<string, object>();
        bool isHealthy = true;

        // 1. Database Check
        try
        {
            await _context.Database.CanConnectAsync();
            status.Add("Database", "Online");
        }
        catch (Exception ex)
        {
            isHealthy = false;
            status.Add("Database", $"Offline: {ex.Message}");
        }

        // 2. JWT Configuration Check
        var jwtKey = _configuration["Jwt:Key"];
        if (!string.IsNullOrEmpty(jwtKey) && jwtKey.Length >= 32)
        {
            status.Add("JWT", "Configured");
        }
        else
        {
            isHealthy = false;
            status.Add("JWT", "Misconfigured (Missing or too short key)");
        }

        // 3. SMTP Check (Configuration & Connection)
        var smtpStatus = new Dictionary<string, string>();
        if (
            !string.IsNullOrEmpty(_smtpSettings.Host)
            && !string.IsNullOrEmpty(_smtpSettings.UserName)
        )
        {
            smtpStatus.Add("Config", "Present");

            // Try pinging the SMTP server port
            try
            {
                using var client = new TcpClient();
                var connectTask = client.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port);
                if (await Task.WhenAny(connectTask, Task.Delay(2000)) == connectTask)
                {
                    smtpStatus.Add("Connection", "Reachable");
                }
                else
                {
                    smtpStatus.Add("Connection", "Timeout");
                    // We don't mark overall health as false for SMTP timeout as it might be firewall,
                    // but good to know.
                }
            }
            catch (Exception ex)
            {
                smtpStatus.Add("Connection", $"Failed: {ex.Message}");
            }
        }
        else
        {
            smtpStatus.Add("Config", "Missing");
            // Again, maybe not critical for app start, but critical for features.
        }
        status.Add("SMTP", smtpStatus);

        status.Add("OverallStatus", isHealthy ? "Healthy" : "Unhealthy");
        status.Add("Timestamp", DateTime.UtcNow);

        return Ok(status);
    }
}
