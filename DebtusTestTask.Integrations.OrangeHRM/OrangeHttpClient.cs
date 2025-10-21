using Microsoft.Playwright;
using System.Net;

namespace DebtusTestTask.Integrations.OrangeHRM;

public class OrangeHttpClient
{
    private readonly HttpClientHandler _handler;
    private readonly HttpClient _client;
    private readonly string _baseUrl;
    private readonly string _loginUrl;
    private readonly string _username;
    private readonly string _password;

    public OrangeHttpClient(string username = "admin", string password = "admin123")
    {
        _handler = new HttpClientHandler
        {
            UseCookies = true,
            CookieContainer = new CookieContainer()
        };
        _client = new HttpClient(_handler);
        _baseUrl = "https://opensource-demo.orangehrmlive.com";
        _loginUrl = _baseUrl + "/web/index.php/auth/login";
        _username = username;
        _password = password;
    }

    public async Task<string> GetAuthCookieAsync()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        var page = await browser.NewPageAsync();

        await page.GotoAsync(_loginUrl);

        await page.TypeAsync("input[name='username']", _username);
        await page.TypeAsync("input[name='password']", _password);

        await page.ClickAsync("button[type='submit']");

        await page.WaitForNavigationAsync();

        var cookies = await page.Context.CookiesAsync([ _baseUrl ]);
        var authCookie = cookies.FirstOrDefault(c => c.Name == "orangehrm")?.Value;

        if (string.IsNullOrEmpty(authCookie))
        {
            throw new Exception("Failed to retrieve orangehrm cookie");
        }

        return authCookie;
    }

    // Пример использования cookie в API-запросе
    public async Task<string> CallApiAsync(string apiEndpoint, string cookie)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, _baseUrl + apiEndpoint);
        request.Headers.Add("Cookie", $"orangehrm={cookie}");
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
