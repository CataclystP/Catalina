using Catalina.Database;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Catalina.Database.Models;

namespace Catalina.Discord
{
    class FoolsModule : BaseCommandModule
    {
        [Command("say")]
        [Description("make catalina say something")]
        public async Task SayMessage(CommandContext ctx, [RemainingText] string contents)
        {
            using var database = new DatabaseContextFactory().CreateDbContext();

            var verification = await CoreModule.IsVerifiedAsync(ctx, true);
            if (verification != PermissionCode.Qualify) return;

            try
            {
                await ctx.Message.DeleteAsync("tidywork.");
            }
            catch (Exception ex)
            {
                //couldn't delete message
            }

            var message = new DiscordMessageBuilder()
            {
                Content = contents,

            };
            await ctx.Channel.SendMessageAsync(message);

        }

        [Command("response")]
        [Description("add or remove responses")]
        public async Task HandleResponses(CommandContext ctx, string mode = null, string name = null, string trigger = null, string response = null, int bonus = 0)
        {
            using var database = new DatabaseContextFactory().CreateDbContext();

            var verification = await CoreModule.IsVerifiedAsync(ctx, true);
            if (verification != PermissionCode.Qualify) return;

            mode = string.IsNullOrEmpty(mode) ? null : mode.ToLower();

            switch (mode)
            {
                case "add":
                    await AddResponseAsync(ctx, name, trigger, response, bonus);
                    break;

                case "remove":
                    await RemoveResponseAsync(ctx, name);
                    break;

                case "list":
                default:
                    await ListResponses(ctx);
                    break;
            }
        }

        public static async Task<bool> AddResponseAsync(CommandContext ctx, string name, string trigger, string response, int scoreBonus)
        {
            using var database = new DatabaseContextFactory().CreateDbContext();

            DiscordEmbed discordEmbed;
            var verification = await CoreModule.IsVerifiedAsync(ctx, true);
            if (verification == PermissionCode.Qualify)
            {
                if (string.IsNullOrEmpty(name))
                {
                    discordEmbed = new DiscordEmbedBuilder
                    {
                        Title = "Sorry!",
                        Description = "You didn't provide a message name, describe what this response is for.",
                        Color = DiscordColor.Red
                    }.Build();
                    await ctx.RespondAsync(discordEmbed);
                    return false;
                }
                else if (string.IsNullOrEmpty(trigger))
                {
                    discordEmbed = new DiscordEmbedBuilder
                    {
                        Title = "Sorry!",
                        Description = "You didn't provide a message trigger, what should I respond to?",
                        Color = DiscordColor.Red
                    }.Build();
                    await ctx.RespondAsync(discordEmbed);
                    return false;
                }
                else if (string.IsNullOrEmpty(response))
                {
                    discordEmbed = new DiscordEmbedBuilder
                    {
                        Title = "Sorry!",
                        Description = "You didn't provide a message response, how should I respond?",
                        Color = DiscordColor.Red
                    }.Build();
                    await ctx.RespondAsync(discordEmbed);
                    return false;
                }

                if (IfResponseExists(name, ctx.Guild.Id)) {
                    discordEmbed = new DiscordEmbedBuilder
                    {
                        Title = "Sorry!",
                        Description = "A response by that name already exists, please use a different name or remove the response first.",
                        Color = DiscordColor.Red
                    }.Build();
                    await ctx.RespondAsync(discordEmbed);
                    return false;
                }

                database.Responses.Add(new Response
                {
                    Name = name,
                    Trigger = trigger,
                    Content = response,
                    GuildID = ctx.Guild.Id,
                    Bonus = scoreBonus
                });
                _ = database.SaveChangesAsync();

                discordEmbed = new DiscordEmbedBuilder
                {
                    Title = "Success!",
                    Description = "Response added!",
                    Color = DiscordColor.Green
                }.Build();
                await ctx.RespondAsync(discordEmbed);

                return true;
            }
            return false;
        }
        public static async Task<bool> RemoveResponseAsync(CommandContext ctx, string name)
        {
            using var database = new DatabaseContextFactory().CreateDbContext();

            DiscordEmbed discordEmbed;
            var verification = await CoreModule.IsVerifiedAsync(ctx, true);
            if (verification == PermissionCode.Qualify)
            {
                if (string.IsNullOrEmpty(name))
                {
                    discordEmbed = new DiscordEmbedBuilder
                    {
                        Title = "Sorry!",
                        Description = "You didn't provide a response name.",
                        Color = DiscordColor.Red
                    }.Build();
                    await ctx.RespondAsync(discordEmbed);
                    return false;
                }

                if (!IfResponseExists(name, ctx.Guild.Id))
                {
                    discordEmbed = new DiscordEmbedBuilder
                    {
                        Title = "Sorry!",
                        Description = "A response by that name could not be found.",
                        Color = DiscordColor.Red
                    }.Build();
                    await ctx.RespondAsync(discordEmbed);
                    return false;
                }

                database.Responses.Remove(database.Responses.AsNoTracking().Where(r => r.GuildID == ctx.Guild.Id && r.Name == name).First());
                _ = database.SaveChangesAsync();

                discordEmbed = new DiscordEmbedBuilder
                {
                    Title = "Success!",
                    Description = "Response removed!",
                    Color = DiscordColor.Green
                }.Build();
                await ctx.RespondAsync(discordEmbed);

                return true;
            }
            return false;
        }
        public static async Task ListResponses(CommandContext ctx)
        {
            using var database = new DatabaseContextFactory().CreateDbContext();

            var responses = database.Responses.AsNoTracking().Where(r => r.GuildID == ctx.Guild.Id).ToList();
            DiscordEmbedBuilder discordEmbed = new DiscordEmbedBuilder
            {
                Title = "Uh oh!",
                Description = string.Format("No responses found for this server."),
                Color = DiscordColor.Red
            };

            if (responses.Count > 100)
            {
                for (int i = 0; i < MathF.Floor(responses.Count / 100) + 1; i++)
                {
                    discordEmbed = new DiscordEmbedBuilder
                    {
                        Title = "Success!",
                        Description = string.Format("Responses for this server: {0}/{1}", i + 1, MathF.Floor(responses.Count / 100) + 1),
                        Color = DiscordColor.Blue
                    };
                    int currentResponse = 0; int responsesLeft = responses.Count - (i * 100);

                    for (int j = i * 100; j < responses.Count - (i * 100); j+=2)
                    {
                        currentResponse++;
                        if (j < (responses.Count - 1)) discordEmbed.AddField(responses[j].Name, responses[j + 1].Name);
                        else discordEmbed.AddField(responses[j].Name, "**[end of list]**");
                    }
                    discordEmbed.Build();
                    await ctx.Message.RespondAsync(discordEmbed);
                }

            }

            else if (responses.Count > 0)
            {
                discordEmbed = new DiscordEmbedBuilder
                {
                    Title = "Success!",
                    Description = "Responses for this server:",
                    Color = DiscordColor.Blue
                };

                for (int i = 0; i < responses.Count; i+=2)
                {
                    
                    if (i < responses.Count - 1) discordEmbed.AddField(responses[i].Name, responses[i + 1].Name);
                    else discordEmbed.AddField(responses[i].Name, "**[end of list]**");

                    
                }
                discordEmbed.Build();
                await ctx.Message.RespondAsync(discordEmbed);
            }
            else
            {
                await ctx.Message.RespondAsync(discordEmbed.Build());
            }
        }
        public static bool IfResponseExists(Response query)
        {
            using var database = new DatabaseContextFactory().CreateDbContext();

            if (database.Responses.AsNoTracking().Any(r => r.Name == query.Name && r.GuildID == query.GuildID)) return true;
            else return false;
        }
        public static bool IfResponseExists(string name, ulong guildID)
        {
            using var database = new DatabaseContextFactory().CreateDbContext();

            if (database.Responses.AsNoTracking().Any(r => r.Name == name && r.GuildID == guildID)) return true;
            else return false;
        }

    }
}
