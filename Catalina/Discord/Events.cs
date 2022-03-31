using Catalina.Database;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Catalina.Discord
{
    class Events
    {

        internal static async Task Discord_ReactionAdded(DiscordClient sender, MessageReactionAddEventArgs e)
        {
            using var database = new DatabaseContextFactory().CreateDbContext();
            if (e.Guild != null) await GuildPingAsync(e.Guild.Id);

            if (database.Reactions.AsNoTracking().Any(i => i.GuildID == e.Guild.Id))
            {
                var reactions = database.Reactions.AsNoTracking().Where(i => i.GuildID == e.Guild.Id).ToList();
                if (reactions.Select(reaction => reaction.MessageID).Contains(e.Message.Id) && reactions.Select(reaction => reaction.EmojiName).Contains(e.Emoji.Name) && !e.User.IsBot)
                {
                    var reaction = reactions.Find(r => r.MessageID == e.Message.Id && r.EmojiName == e.Emoji.Name);
                    var member = await e.Guild.GetMemberAsync(e.User.Id);
                    try
                    {
                        await member.GrantRoleAsync(e.Guild.GetRole(reaction.RoleID), "Assigned role upon reaction");
                    }
                    catch
                    {
                        //try
                        //{
                        //    await e.Message.DeleteReactionAsync(e.Emoji, member);
                        //}
                        //catch
                        //{

                        //}
                    }
                }
            }
        }

        internal static async Task Discord_ReactionRemoved(DiscordClient sender, MessageReactionRemoveEventArgs e)
        {
            using var database = new DatabaseContextFactory().CreateDbContext();
            if (e.Guild != null) await GuildPingAsync(e.Guild.Id);

            if (database.Reactions.AsNoTracking().Any(i => i.GuildID == e.Guild.Id))
            {
                var reactions = database.Reactions.AsNoTracking().Where(i => i.GuildID == e.Guild.Id).ToList();
                if (reactions.Select(reaction => reaction.MessageID).Contains(e.Message.Id) && reactions.Select(reaction => reaction.EmojiName).Contains(e.Emoji.Name) && !e.User.IsBot)
                {
                    var reaction = reactions.Find(r => r.MessageID == e.Message.Id && r.EmojiName == e.Emoji.Name);
                    var member = await e.Guild.GetMemberAsync(e.User.Id);
                    try
                    {
                        await member.RevokeRoleAsync(e.Guild.GetRole(reaction.RoleID), "Revoked role upon reaction");
                    }
                    catch { }
                }
            }
        }

        internal static async Task Discord_MessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {

            if (e.Author.IsBot || e.Author.Discriminator == "0000") return;
            using var database = new DatabaseContextFactory().CreateDbContext();
            var responses = database.Responses.AsNoTracking().Where(r => r.GuildID == e.Guild.Id).ToList();

            if (responses.Count > 0 && responses.Select(r => r.Trigger.ToLower()).Any(t => t == e.Message.Content.ToLower()))
            {
                var response = responses.Where(r => r.Trigger.ToLower() == e.Message.Content.ToLower()).First();
                var message = await e.Message.RespondAsync(response.Content);

                int chance = (int) MathF.Abs(response.Bonus);
                int randomResult = Program.Random.Next(0, 1000);

                Log.Debug(chance + " / 1000, rolled " + randomResult);
                if (response.Bonus < 0 && (chance > 1000 || randomResult < chance))
                {  
                    if (database.GuildUsers.Any(u => u.DiscordID == e.Author.Id))
                    {
                        database.GuildUsers.Where(u => u.DiscordID == e.Author.Id).First().Score += response.Bonus;
                    }
                    else
                    {
                        database.GuildUsers.Add(new Database.Models.GuildUser
                        {
                            DiscordID = e.Author.Id,
                            Score = response.Bonus
                        });
                        await database.SaveChangesAsync();
                    }
                    await message.ModifyAsync(content: string.Format(message.Content + "\nfyi: you lost {0} favour with me for that. you have {1} favour with me.", response.Bonus, database.GuildUsers.Where(u => u.DiscordID == e.Author.Id).First().Score));
                   
                }
                else if (response.Bonus > 0 && (chance > 1000 || randomResult < chance))
                {
                    
                    if (database.GuildUsers.Any(u => u.DiscordID == e.Author.Id))
                    {
                        database.GuildUsers.Where(u => u.DiscordID == e.Author.Id).First().Score += response.Bonus;
                    }
                    else
                    {
                    database.GuildUsers.Add(new Database.Models.GuildUser
                        {
                            DiscordID = e.Author.Id,
                            Score = response.Bonus
                        });
                        await database.SaveChangesAsync();
                    }
                    await message.ModifyAsync(content: string.Format(message.Content + "\nfyi: you gained {0} favour with me for that. you have {1} favour with me.", response.Bonus, database.GuildUsers.Where(u => u.DiscordID == e.Author.Id).First().Score));
                }
                await database.SaveChangesAsync();
               
            }
        }

        internal static async Task Discord_ReactionsCleared(DiscordClient sender, MessageReactionsClearEventArgs e)
        {
            using var database = new DatabaseContextFactory().CreateDbContext();
            if (e.Guild != null) await GuildPingAsync(e.Guild.Id);

            //database.Reactions.AsQueryable().Where(i => i.GuildID == e.Guild.Id);
            if (database.Reactions.AsNoTracking().Any(i => i.GuildID == e.Guild.Id))
            {
                var reactions = database.Reactions.AsNoTracking().Where(i => i.GuildID == e.Guild.Id);
                if (reactions.Any(reaction => reaction.MessageID == e.Message.Id))
                {
                    var matches = reactions.Where(r => r.MessageID == e.Message.Id);
                    foreach (var match in matches)
                    {
                        var emoji = Discord.GetEmojiFromString(match.EmojiName);

                        if (emoji != null)
                        {
                            try
                            {
                                await e.Message.CreateReactionAsync(emoji);
                            }
                            catch
                            {
                                Console.WriteLine("Could not repopulate reactions after a message was cleared of reactions.");
                            }
                        }
                    }
                }
            }
        }

        internal static async Task Discord_GuildMemberAdded(DiscordClient sender, GuildMemberAddEventArgs e)
        {
            using var database = new DatabaseContextFactory().CreateDbContext();
            if (e.Guild != null) await GuildPingAsync(e.Guild.Id);

            if (database.GuildProperties.AsNoTracking().Any(g => g.ID == e.Guild.Id && g.DefaultRole.HasValue))
            {
                try
                {
                    await e.Member.GrantRoleAsync(e.Guild.GetRole((ulong)database.GuildProperties.Where(p => p.ID == e.Guild.Id).Select(p => p.DefaultRole.Value).FirstOrDefault()));
                }
                catch
                {
                    var guildID = e.Guild.Id;
                    Log.Information($"Could not grant new user default role on guild #{guildID}");
                }
            }
        }

        internal static async Task Discord_MessageDeleted(DiscordClient sender, MessageDeleteEventArgs e)
        {
            using var database = new DatabaseContextFactory().CreateDbContext();
            if (e.Guild != null) await GuildPingAsync(e.Guild.Id);

            if (database.Reactions.AsQueryable().Any(i => i.GuildID == e.Guild.Id))
            {
                var reactions = database.Reactions.AsNoTracking().Where(i => i.GuildID == e.Guild.Id);
                if (reactions.Select(reaction => reaction.MessageID).Contains(e.Message.Id))
                {
                    var matches = reactions.Where(r => r.MessageID == e.Message.Id).ToList();
                    matches.ForEach(match =>
                    {
                        database.Reactions.Remove(match);
                    });

                    await database.SaveChangesAsync();
                }
            }
            Log.Information("Removed reactions from deleted message!");
        }

        internal static async Task GuildPingAsync(ulong guildID)
        {
            using var database = new DatabaseContextFactory().CreateDbContext();
            if (!database.GuildProperties.AsQueryable().Any(g => g.ID == guildID))
            {
                database.GuildProperties.Add(new Database.Models.GuildProperty
                {
                    ID = guildID
                    
                });
                await database.SaveChangesAsync();
            }
        }
    }
}
