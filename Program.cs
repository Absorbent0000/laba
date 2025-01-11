using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80);
});

var app = builder.Build();

string DATA_DIR = "app/data";
string DATA_FILE = Path.Combine(DATA_DIR, "message.json");
app.MapGet("/", async () =>
{
    var message = "Message for  docker volume";
    Console.WriteLine(message);

    if (!Directory.Exists(DATA_DIR))
    {
        Directory.CreateDirectory(DATA_DIR);
    }
    var jsonMessage = JsonSerializer.Serialize(new { message });
    await File.WriteAllTextAsync(DATA_FILE, jsonMessage);
    return "Сообщение сохранено в файл!";
});

app.MapPost("/save", async (HttpContext context) =>
{
    var message = "Message for docker volume";

    Console.WriteLine(message);

    if (!Directory.Exists(DATA_DIR))
    {
        Directory.CreateDirectory(DATA_DIR);
    }
    var jsonMessage = JsonSerializer.Serialize(new { message });
    await File.WriteAllTextAsync(DATA_FILE, jsonMessage);

    await context.Response.WriteAsJsonAsync(new { message = "Сообщение сохранено в файл" });
});

app.Run();
