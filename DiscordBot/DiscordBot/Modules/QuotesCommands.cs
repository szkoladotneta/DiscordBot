

using Discord;
using Discord.Interactions;
using DiscordBot.Services;

namespace DiscordBot.Modules;

public class QuotesCommands : InteractionModuleBase<SocketInteractionContext>
{
    public InteractionService Commands { get; set; }

    private CommandHandler _handler;

    public QuotesCommands(CommandHandler handler)
    {
        _handler = handler;
    }
    [SlashCommand("echo", "Repeat the input")]
    public async Task Echo(string echo, [Summary(description: "mention the user")] bool mention = false)
        => await RespondAsync(echo + (mention ? Context.User.Mention : string.Empty));

    [SlashCommand("ping", "Pings the bot and returns its latency.")]
    public async Task GreetUserAsync()
        => await RespondAsync(text: $":ping_pong: It took me {Context.Client.Latency}ms to respond to you!", ephemeral: true);

    [SlashCommand("quote", "Gives random quotes from movies")]
    public async Task Quote()
    => await RespondAsync(text: $"I'm too old for this shit!");

    [UserCommand("greet")]
    public async Task GreetUserAsync(IUser user)
        => await RespondAsync(text: $":wave: {Context.User} said hi to you, <@{user.Id}>!");
}