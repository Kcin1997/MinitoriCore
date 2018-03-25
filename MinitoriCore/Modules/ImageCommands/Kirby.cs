﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using RestSharp;
using Newtonsoft.Json;

namespace MinitoriCore.Modules.ImageCommands
{
    class ArenaScore
    {
        public int Score;
        public string Level;
        public TimeSpan Time;
        public string P1;
        public string P2 = "";
        public string P3 = "";
        public string P4 = "";
        public bool Group = false;
        public bool Amiibo = false;
    }

    public class Kirby : ModuleBase
    {
        [Command("addentry")]
        public async Task AddLeaderboard([Remainder]string remainder)
        {
            if (remainder.Length == 0)
            {
                await ReplyAsync("You can't leave this blank!"); // Todo: put a help command here
                return;
            }

            var split = remainder.Split(':').Select(x => x.Trim()).ToArray();

            if (split.Length % 2 == 1)
            {
                await ReplyAsync("You gave me an uneven number of responses! Check your colons.");
                return;
            }

            var Ranking = new ArenaScore();

            for (int i = 0; i < split.Length; i += 2)
            {
                switch(split[i].ToLower())
                {
                    case "score":
                        int tempScore;
                        if (int.TryParse(split[i+1], out tempScore))
                            Ranking.Score = tempScore;
                        else
                        {
                            await ReplyAsync($"`{split[i + 1]}` doesn't look like a valid score to me.");
                            return;
                        }
                        break;
                    case "time":
                        // Timespan.TryParse and add a "00:" to the start and see if that works
                        TimeSpan tempTime;
                        if (TimeSpan.TryParse(split[i+1], out tempTime))
                            Ranking.Time = tempTime;
                        else if (TimeSpan.TryParse($"00:{split[i + 1]}", out tempTime))
                            Ranking.Time = tempTime;
                        else
                        {
                            await ReplyAsync($"`{split[i + 1]}` doesn't look like a valid time to me.");
                            return;
                        }
                        break;
                    case "level":
                        // string because S rank
                        break;
                    case "p1":
                        Ranking.P1 = split[i + 1];
                        break;
                    case "p2":
                        Ranking.P2 = split[i + 1];
                        break;
                    case "p3":
                        Ranking.P3 = split[i + 1];
                        break;
                    case "p4":
                        Ranking.P4 = split[i + 1];
                        break;
                    case "amiibo":
                        if (split[i + 1].ToLower() == "yes")
                            Ranking.Amiibo = true;
                        else if (split[i + 1].ToLower() == "no")
                            Ranking.Amiibo = false;
                        else
                        {
                            await ReplyAsync("I was looking for a yes or no answer for the Amiibo section.");
                            return;
                        }
                        break;
                    case "group":
                        if (split[i + 1].ToLower() == "yes")
                            Ranking.Group = true;
                        else if (split[i + 1].ToLower() == "no")
                            Ranking.Group = false;
                        else
                        {
                            await ReplyAsync("I was looking for a yes or no answer for the Group section.");
                            return;
                        }
                        break;
                }
            }

            await ReplyAsync(
                $"Time: {Ranking.Time.ToString()}\n" +
                $"Score: {Ranking.Score.ToString()}\n" +
                $"Level: {Ranking.Level}\n" +
                $"P1: {Ranking.P1}\n" +
                $"P2: {Ranking.P2}\n" +
                $"P3: {Ranking.P3}\n" +
                $"P4: {Ranking.P4}\n" +
                $"Amiibo: {Ranking.Amiibo.ToString()}\n" +
                $"Group: {Ranking.Group.ToString()}");
        }

        public Kirby(CommandService commands, IServiceProvider services)
        {
            commands.CreateModuleAsync("", x =>
            {
                x.Name = "Kirby";

                foreach (string[] source in new string[][] {
                    new string[] { "poyo", "kirby", "gorb" },
                    new string[] { "ddd", "dedede" },
                    new string[] { "metaborb", "metaknight", "borb" },
                    new string[] { "bandana", "waddee", "waddle" },
                    new string[] { "egg", "lor" },
                    new string[] { "spiderman", "taranza", "spid" },
                    new string[] { "squeak", "squek" },
                    new string[] { "familyproblems", "susie", "soos" },
                    new string[] { "artist", "adeleine" },
                    new string[] { "randomfairy", "ribbon" },
                    new string[] { "dreamland" },
                    new string[] { "birb", "gala" },
                    new string[] { "onion", "witch", "gryll" },
                    new string[] { "queen", "secc", "sectonia" },
                    new string[] { "helper", "helpers", "helpful", "friendship", "friendo" },
                    new string[] { "moretsu", "manga", "mungu", "kirbymanga" },
                    new string[] { "grenpa", "mommy" },
                    new string[] { "eye", "eyeborb", "badsphere" },
                    new string[] { "dad", "father", "baddad", "haltman", "daddy" },
                    new string[] { "clown", "marx", "grape" } })
                {
                    x.AddCommand(source[0], async (context, param, serv, command) =>
                    {
                        await context.Channel.SendMessageAsync("It didnt work son");
                    },
                    command => 
                    {
                        command.AddAliases(source.Skip(1).ToArray());
                        command.Summary = $"***{source[0]}***";
                    });
                }
                
                x.Build(commands, services);
            });
        }

        private async Task UploadImage(string source, CommandContext Context)
        {
            
        }
    }
}
