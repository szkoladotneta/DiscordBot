using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordBot.Common;
using DiscordBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

IConfiguration _configuration;
IServiceProvider _services;

DiscordSocketConfig _socketConfig = new()
{
    GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers,
    AlwaysDownloadUsers = true,
};

_configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

_services = new ServiceCollection()
    .AddSingleton(_configuration)
    .AddSingleton(_socketConfig)
    .AddSingleton<DiscordSocketClient>()
    .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
    .AddSingleton<CommandHandler>()
    .BuildServiceProvider();

await MainAsync();

async Task MainAsync()
{
    var client = _services.GetRequiredService<DiscordSocketClient>();

    client.Log += LogAsync;

    await _services.GetRequiredService<CommandHandler>()
        .InitializeAsync();

    await client.LoginAsync(TokenType.Bot, _configuration.GetRequiredSection("Settings")["DiscordBotToken"]);
    await client.StartAsync();

    await Task.Delay(Timeout.Infinite);
}

async Task LogAsync(LogMessage message)
            => Logger.Log(message);