﻿using Discord;
using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Catalina.Database;
using Discord.Commands;

namespace Catalina.Discord
{
    class Events
    {

        internal static async Task Discord_ReactionAdded(Cacheable<IUserMessage, ulong> arg1, Cacheable<IMessageChannel, ulong> arg2, SocketReaction arg3)
        {
            using var database = new DatabaseContextFactory().CreateDbContext();
        }

        internal static async Task Discord_ReactionRemoved(Cacheable<IUserMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
        {
            using var database = new DatabaseContextFactory().CreateDbContext();
        }

        internal static async Task Discord_MessageCreated(SocketMessage arg)
        {

            if (arg.Author.IsBot ||
                arg.Author.IsWebhook ||
                arg.Author.DiscriminatorValue == 0000 ||
                arg is not SocketUserMessage message) return;

            CommandContext context = new CommandContext(Discord.discord, message);
            string prefix = Environment.GetEnvironmentVariable(AppProperties.BotPrefix);
            if (message.Content.StartsWith(prefix)) {
                await Discord.commandService.ExecuteAsync(context, prefix.Length, null);
            }

        }

        internal static async Task Discord_ReactionsCleared(Cacheable<IUserMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel)
        {
            using var database = new DatabaseContextFactory().CreateDbContext();
        }

        internal static async Task Discord_GuildMemberAdded(SocketGuildUser user)
        {
            using var database = new DatabaseContextFactory().CreateDbContext();
        }

        internal static async Task Discord_MessageDeleted(Cacheable<IMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel)
        {
            using var database = new DatabaseContextFactory().CreateDbContext();
        }

        internal static async Task GuildPingAsync(ulong guildID)
        {
            using var database = new DatabaseContextFactory().CreateDbContext();
        }

        internal static Task Discord_Log(LogMessage msg)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();

            switch (msg.Severity)
            {
                case LogSeverity.Critical:
                    if (msg.Exception is not null)
                        logger.Fatal(msg.Exception, $"{msg.Message}");
                    else
                        logger.Fatal($"{msg.Message}");
                    break;
                case LogSeverity.Debug:
                    if (msg.Exception is not null)
                        logger.Debug(msg.Exception, $"{msg.Source}");
                    else
                        logger.Debug($"{msg.Message}");
                    break;
                case LogSeverity.Error:
                    if (msg.Exception is not null)
                        logger.Error(msg.Exception, $"{msg.Message}");
                    else
                        logger.Error($"{msg.Message}");
                    break;
                case LogSeverity.Info:
                    if (msg.Exception is not null)
                        logger.Info(msg.Exception, $"{msg.Message}");
                    else
                        logger.Info($"{msg.Message}");
                    break;
                case LogSeverity.Verbose:
                    if (msg.Exception is not null)
                        logger.Debug(msg.Exception, $"{msg.Message}");
                    else
                        logger.Debug($"{msg.Message}");
                    break;
                case LogSeverity.Warning:
                    if (msg.Exception is not null)
                        logger.Warn(msg.Exception, $"{msg.Message}");
                    else
                        logger.Warn($"{msg.Message}");
                    break;
            };

            return Task.CompletedTask;
        }

        internal static async Task Discord_Ready()
        {
            await Discord.discord.SetGameAsync(type: ActivityType.Watching, name: "you sleep.");
            Discord.UpdateChannels();
            NLog.LogManager.GetCurrentClassLogger().Info("Hallo!");
        }
    }
}