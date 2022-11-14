using System.Text.Json;
using Overlay.API;
using Overlay.API.Hubs;
using Overlay.Data;
using Overlay.Data.Interfaces;

// HTTP(s) calls are incredibly slow unless we disable ipv6 for our current application
// You can now set the environment variable DOTNET_SYSTEM_NET_DISABLEIPV6 to 1 or the System.Net.DisableIPv6 runtime configuration setting to true if you experience similar problems and decide to address them by disabling IPv6.
// https://learn.microsoft.com/en-us/dotnet/core/runtime-config/
Environment.SetEnvironmentVariable("DOTNET_SYSTEM_NET_DISABLEIPV6", "1");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });

builder.Services.AddSignalR();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddSingleton<CaptureClient>();
builder.Services.AddSingleton<IOCRClient, OCRClient>();
builder.Services.AddSingleton<IMMRClient, MMRClient>();
builder.Services.AddSingleton<SC2Client>();
builder.Services.AddSingleton<Broker>();


var app = builder.Build();

// Start our event loop for monitoring the UI
var sc2Client = app.Services.GetService<SC2Client>();
var uiMonitor = sc2Client.MonitorUI();

// Get an instance of the broker so that we dispense events to signalR hub
var broker = app.Services.GetService<Broker>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.MapHub<OverlayHub>("/overlay");
app.Run();