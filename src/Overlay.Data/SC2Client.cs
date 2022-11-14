using System.Drawing;
using System.Text.Json;
using Overlay.Data.Enums;
using Overlay.Data.Interfaces;
using Overlay.Data.Models;

namespace Overlay.Data;

public class SC2Client
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly CaptureClient _capture;
    private readonly IMMRClient _mmr;
    public SC2Scene PreviousScreen;
    public SC2Scene CurrentScreen;
    public Dictionary<int, int> MMRs { get; set; } = new Dictionary<int, int>();
    public event SC2Event OnSceneChangeAsync;
    public event SC2Event OnEnterGameAsync;
    public event SC2Event OnLeaveGameAsync;
    public delegate Task SC2Event();

    public SC2Client(IHttpClientFactory httpClientFactory, CaptureClient capture, IMMRClient mmr)
    {
        _httpClientFactory = httpClientFactory;
        _serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        _capture = capture;
        _mmr = mmr;
    }

    public async Task<object> GetAllAsync()
    {
        var game = GetGameAsync();
        var ui = GetUIAsync();

        await Task.WhenAll(game, ui);

        return new
        {
            Game = game.Result,
            UI = ui.Result
        };
    }

    private async Task<T> GetAsync<T>(string url) where T : new()
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            var result = await httpClient.GetAsync(url);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content, _serializerOptions);
            }
        }
        catch (HttpRequestException)
        {
            Console.WriteLine("Game is probably just closed.");
        }

        return new T();
    }

    public async Task<SC2Game> GetGameAsync()
    {
        var game = await GetAsync<SC2Game>("http://localhost:6119/game");
        game.Players.ForEach(x => x.MMR = MMRs.SingleOrDefault(y => y.Key == x.Id).Value);

        return game;
    }

    public async Task<SC2UI> GetUIAsync()
    {
        var ui = await GetAsync<SC2UI>("http://localhost:6119/ui");

        return ui;
    }

    public async Task MonitorUI()
    {
        var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(500));

        while (await timer.WaitForNextTickAsync())
        {
            var ui = await GetUIAsync();
            CurrentScreen = ui.Scene;

            if (CurrentScreen != PreviousScreen)
            {
                // Matchup screen before a game
                if (PreviousScreen != SC2Scene.None && CurrentScreen == SC2Scene.Loading)
                {
                    await GetMMRviaOCR();
                }
                // Entering a Game
                else if (PreviousScreen == SC2Scene.Loading && CurrentScreen == SC2Scene.None)
                {
                    await OnEnterGameAsync();
                }
                // Leaving a Game
                else if (PreviousScreen == SC2Scene.None && CurrentScreen != SC2Scene.None)
                {
                    await OnLeaveGameAsync();
                }

                await OnSceneChangeAsync();
            }

            PreviousScreen = CurrentScreen;
        }
    }

    private async Task GetMMRviaOCR()
    {
        var attempts = 0;

        do
        {
            await Task.Delay(2000);

            Console.WriteLine($"OCR attempt {attempts + 1}");

            MMRs = _mmr.GetFromBitmap((Bitmap)_capture.CaptureWindow("StarCraft II", "StarCraft II"));
            attempts++;
        }
        while (attempts < 3 && MMRs.All(x => x.Value == 0));
    }
}