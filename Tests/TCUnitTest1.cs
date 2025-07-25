using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Xunit;

namespace PlaywrightTests;

public class TCUnitTest1 : IAsyncLifetime
{
   
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IBrowserContext? _context;
    private IPage? _page;

    public async Task InitializeAsync()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
        _context = await _browser.NewContextAsync();
        _page = await _context.NewPageAsync();
    }

    public async Task DisposeAsync()
    {
        await _page.CloseAsync();
        await _context.CloseAsync();
        await _browser.CloseAsync();
        _playwright.Dispose();
    }


    private UnitTest1 LoadTestData()
    {
        var json = File.ReadAllText("C:/Users/Acer/PlaywrightTests/TestData/UnitTest1/unitTest1.json");
        return JsonSerializer.Deserialize<UnitTest1>(json);
    }

    [Fact]
    public async Task HasTitle()
    {
        var data = LoadTestData();
        await _context.Tracing.StartAsync(new TracingStartOptions
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });

        await _page.GotoAsync(data.Url);
        var title = await _page.TitleAsync();
        Assert.Contains(data.ExpectedTitle, title);

        await _context.Tracing.StopAsync(new TracingStopOptions
        {
            Path = "trace_HasTitle.zip"
        });
    }

    [Fact]
    public async Task GetStartedLink()
    {
        var data = LoadTestData();
        await _context.Tracing.StartAsync(new TracingStartOptions
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });

        await _page.GotoAsync(data.Url);
        await _page.GetByRole(AriaRole.Link, new() { Name = data.Link }).ClickAsync();
        var heading = await _page.GetByRole(AriaRole.Heading, new() { Name = data.Name }).InnerTextAsync();
        Assert.Equal(data.Name, heading);

        await _context.Tracing.StopAsync(new TracingStopOptions
        {
            Path = "trace_GetStartedLink.zip"
        });
    }
    
    
}