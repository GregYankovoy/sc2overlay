using Microsoft.AspNetCore.SignalR;
using Overlay.API.Hubs;
using Overlay.Data;

namespace Overlay.API;

public class Broker
{
    private readonly IHubContext<OverlayHub> _hub;
    private readonly SC2Client _sc2;

    public Broker(IHubContext<OverlayHub> hub, SC2Client sc2)
    {
        _hub = hub;
        _sc2 = sc2;

        _sc2.OnSceneChangeAsync += OnSceneChangeAsync;
        _sc2.OnEnterGameAsync += OnEnterGameAsync;
        _sc2.OnLeaveGameAsync += OnLeaveGameAsync;
    }

    private async Task OnSceneChangeAsync()
    {
        await _hub.Clients.All.SendAsync("ChangeScene", _sc2.CurrentScreen.ToString());
    }

    private async Task OnEnterGameAsync()
    {
        var game = await _sc2.GetGameAsync();

        await _hub.Clients.All.SendAsync("EnterGame", game);
    }

    private async Task OnLeaveGameAsync()
    {
        var game = await _sc2.GetGameAsync();

        await _hub.Clients.All.SendAsync("LeaveGame", game);
    }
}