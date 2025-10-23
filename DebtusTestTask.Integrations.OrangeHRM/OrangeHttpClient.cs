using DebtusTestTask.Integrations.OrangeHRM.Contracts.Input;
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

    private string _cookie { get { return GetAuthCookieAsync().GetAwaiter().GetResult(); } }

    private readonly string _eventGetUrl;
    private readonly string _employeeGetUrl;

    private readonly string _employeePostUrl;

    public OrangeHttpClient(string username = "admin", string password = "admin123")
    {
        _handler = new HttpClientHandler
        {
            UseCookies = true,
            CookieContainer = new CookieContainer()
        };
        _client = new HttpClient(_handler);

        _baseUrl = "https://opensource-demo.orangehrmlive.com";

        _eventGetUrl = _baseUrl + "/" + "web/index.php/api/v2/claim/events?limit=0&status=true";
        _employeeGetUrl = _baseUrl + "/" + "web/index.php/api/v2/pim/employees?limit=0&offset=0&model=detailed&includeEmployees=onlyCurrent&sortField=employee.firstName&sortOrder=ASC";
        _loginUrl = _baseUrl + "/" + "web/index.php/auth/login";
        _employeePostUrl = _baseUrl + "/" + "web/index.php/api/v2/pim/employees";

        _username = username;
        _password = password;
    }

    public async Task<(HttpStatusCode code, string message)> EmployeePostAsync(EmployeeCreateBody b)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, _employeePostUrl);
        request.Headers.Add("Cookie", $"orangehrm={_cookie}");
        request.Content = b;

        var response = await _client.SendAsync(request);

        //response.EnsureSuccessStatusCode();

        var resultCode = response.StatusCode;
        var result = await response.Content.ReadAsStringAsync();

        return (resultCode, result);
    }

    public async Task<(HttpStatusCode code, string message)> EmployeeJobPutAsync(string employeeNumber, JobCreateBody b)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, GetEmployeeJobPutUrl(employeeNumber));
        request.Headers.Add("Cookie", $"orangehrm={_cookie}");
        request.Content = b;

        var response = await _client.SendAsync(request);

        //response.EnsureSuccessStatusCode();

        var resultCode = response.StatusCode;
        var result = await response.Content.ReadAsStringAsync();

        return (resultCode, result);
    }

    public async Task<(HttpStatusCode code, string message)> EventGetListAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, _eventGetUrl);
        request.Headers.Add("Cookie", $"orangehrm={_cookie}");

        var response = await _client.SendAsync(request);

        //response.EnsureSuccessStatusCode();

        var resultCode = response.StatusCode;
        var result = await response.Content.ReadAsStringAsync();

        return (resultCode, result);
    }

    public async Task<(HttpStatusCode code, string message)> EmployeeGetListAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, _employeeGetUrl);
        request.Headers.Add("Cookie", $"orangehrm={_cookie}");

        var response = await _client.SendAsync(request);

        //response.EnsureSuccessStatusCode();

        var resultCode = response.StatusCode;
        var result = await response.Content.ReadAsStringAsync();

        return (resultCode, result);
    }

    public async Task<(HttpStatusCode code, string message)> OrderPostAsync(string employeeNumber, OrderCreateBody b)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, GetOrderPostUrl(employeeNumber));
        request.Headers.Add("Cookie", $"orangehrm={_cookie}");
        request.Content = b;

        var response = await _client.SendAsync(request);

        //response.EnsureSuccessStatusCode();

        var resultCode = response.StatusCode;
        var result = await response.Content.ReadAsStringAsync();

        return (resultCode, result);
    }


    //  TODO: move to singletone / scoped
    public async Task<string> GetAuthCookieAsync()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new()
        {
            Headless = true,
            Timeout = 15000,
            
        });

        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();

        // Навигация на страницу входа
        await page.GotoAsync(_loginUrl, new() { WaitUntil = WaitUntilState.NetworkIdle });

        // Используем FillAsync вместо TypeAsync
        await page.Locator("input[name='username']").FillAsync(_username);
        await page.Locator("input[name='password']").FillAsync(_password);

        // Нажимаем на кнопку и ждём переход
        await Task.WhenAll(
            page.WaitForURLAsync($"{_baseUrl}/**"), // ожидаем переход на базовый URL
            page.Locator("button[type='submit']").ClickAsync()
        );

        // Получаем cookies из контекста
        var cookies = await context.CookiesAsync();
        var authCookie = cookies.FirstOrDefault(c => c.Name == "orangehrm")?.Value;

        if (string.IsNullOrEmpty(authCookie))
        {
            throw new Exception("Failed to retrieve 'orangehrm' cookie.");
        }

        return authCookie;
    }

    private string GetEmployeeJobPutUrl(string empNum)
        => _baseUrl + "/" + $"web/index.php/api/v2/pim/employees/{empNum}/job-details";

    private string GetOrderPostUrl(string empNum)
        => _baseUrl + "/" + $"web/index.php/api/v2/claim/employees/{empNum}/requests";
}
