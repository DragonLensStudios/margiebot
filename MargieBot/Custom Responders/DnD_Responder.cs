
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MargieBot.Models;
using MargieBot.Responders;
using RegexDiceDotNet;

namespace MargieBot.Custom_Responders
{


    /// <summary>
    /// This responder makes MargieBot into a game! When a user says "@user+1" or similar in chat, Margie awards the mentioned user a point. The 
    /// accompanying ScoreboardRequestResponder displays the scoreboard to chat.
    /// </summary>
    public class DnD_Responder : IResponder
    {
        public class DiceRoll
        {
            public override string ToString()
            {
                return $"{Name} Rolled: {Number}D{Sides} for a Total: {Total}";
            }
            public string Name { get; set; }
            public int Number { get; set; }
            public int Sides { get; set; }
            public int Total { get; set; }

            public DiceRoll(string name, int numberofdice, int dicesides)
            {
                
                Name = name;
                Number = numberofdice;
                Sides = dicesides;

                if (numberofdice > 0)
                {
                    for (int i = 0; i < numberofdice; i++)
                    {
                        Total += Dice.Roll(1, dicesides);
                    }
                }
            }
        }


        public static Regex DICE_REGEX = new Regex("roll\\s*(\\d*)d(\\d*)",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace);
        private static string DICE_REGEX_STRING = DICE_REGEX.ToString();
        public bool CanRespond(ResponseContext context)
        {
            return !context.Message.User.IsSlackbot && DICE_REGEX.IsMatch(context.Message.Text);
        }

        public BotMessage GetResponse(ResponseContext context)
        {
            var match = DICE_REGEX.Match(context.Message.Text);
            var group1 = int.Parse(match.Groups[1].Value);
            var group2 = int.Parse(match.Groups[2].Value);
            var roll = new DiceRoll(context.Message.User.FormattedUserID, group1, group2);
            return new BotMessage() { Text = roll.ToString() };
        }

        
    }
}
