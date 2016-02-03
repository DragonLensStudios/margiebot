using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexDiceDotNet
{
    public static class Dice
    {
        public static Random Random = new Random();

        /// <summary>
        /// This is the base method that all other dice methods should use.  It rolls one die of NumSides.
        /// </summary>
        /// <param name="NumSides"></param>
        /// <returns></returns>
        public static int Roll(int NumSides)
        {
            if (NumSides == 0)
            {
                return 0;
            }
            else
            {
                return (Dice.Random.Next(NumSides) + 1);
            }
        }

        public static int Roll(int NumDice, int NumSides, int Modifier = 0)
        {
            int result = 0;
            for (int i = 1; (i <= NumDice); i++)
            {
                result = result + (Dice.Roll(NumSides));
            }
            return (result + Modifier);
        }

        public static int Roll(string DiceExpression)
        { return RollExpanded(DiceExpression).FullResult; }

        private static int? ParseNullableInteger(string StringValue)
        {
            int i;
            int? result = null;
            if (Int32.TryParse(StringValue, out i))
            {
                result = i;
            }
            return result;
        }

        public static ExpandedDiceRoll RollExpanded(string DiceExpression)
        {
            ExpandedDiceRoll result = new ExpandedDiceRoll();

            int literalInteger;
            //Allow a literal number to override a dice roll.
            if (Int32.TryParse(DiceExpression, out literalInteger))
            {
                Roll literalRoll = new Roll();
                literalRoll.RollValue = literalInteger;
                literalRoll.Result = literalInteger;
                
                //result.Accounting = "Literal Integer: " + literalInteger;
                return result;
            }

            DiceParameters diceParams = new DiceParameters(DiceExpression);

            if (!diceParams.IsValid)
            {
                result.IsValid = false;
                result.DiceParameters = diceParams;
                return result;
            }

            result.DiceParameters = diceParams;

            result.Roll();

            ////Normal dice
            //if (diceType == "d"
            //    && !threshold.HasValue)
            //{
            //    List<int> rolls = new List<int>();



            //    if (keepnumber.HasValue)
            //    {
            //        if (keepType == "k")
            //        {
            //            //Keep the highest
            //            rolls = rolls.OrderByDescending(p => p).ToList();
            //        }
            //        else if (keepType == "l")
            //        {
            //            //keep the lowest
            //            rolls = rolls.OrderBy(p => p).ToList();
            //        }
            //    }
            //    StringBuilder sb = new StringBuilder();
            //    int total = 0;
            //    for (int i = 0; i < rolls.Count(); i++)
            //    {
            //        if (keepnumber.HasValue && i < keepnumber.Value)
            //        {
            //            sb.Append(rolls[i] + "*");
            //            total += rolls[i];
            //        }
            //        else if (keepnumber.HasValue)
            //        {
            //            //Don't add
            //            sb.Append(rolls[i]);
            //        }
            //        else
            //        {
            //            //normal roll, always add
            //            sb.Append(rolls[i]);
            //            total += rolls[i];
            //        }
            //        if (i < rolls.Count - 1)
            //        {
            //            sb.Append(" + ");
            //        }
            //    }
            //    result.Accounting = sb.ToString();
            //    result.Result = total;
            //}

            ////Non-smooth exploding dice
            //if ((diceType == "e"
            //    || diceType == "eu"
            //    || diceType == "ed")
            //    && !threshold.HasValue)
            //{
            //    for (int i = 1; (i <= numDice); i++)
            //    {
            //        int roll = Dice.Roll(numSides);
            //        string explodedResultText = "";

            //        if (roll == numSides && (diceType == "e" || diceType == "eu"))
            //        {
            //            //Non-smooth explode up
            //            explodedResultText += "(UP: " + roll + "+";
            //            int explodedRoll = 0;

            //            while (explodedRoll == 0 || explodedRoll == numSides)
            //            {
            //                explodedRoll = Dice.Roll(numSides);
            //                if (explodedRoll == numSides)
            //                {
            //                    explodedResultText += explodedRoll + "+";
            //                    roll += explodedRoll;
            //                }
            //                else
            //                {
            //                    explodedResultText += explodedRoll + ")";
            //                    roll += explodedRoll;
            //                }
            //            }
            //        }
            //        else if (roll == 1 && (diceType == "e" || diceType == "ed"))
            //        {
            //            //Non-smooth explode down
            //            explodedResultText += "(DN: " + roll + "-";
            //            int explodedRoll = 0;

            //            while (explodedRoll == 0 || explodedRoll == numSides)
            //            {
            //                explodedRoll = Dice.Roll(numSides);
            //                if (explodedRoll == numSides)
            //                {
            //                    explodedResultText += explodedRoll + "-";
            //                    roll -= explodedRoll;
            //                }
            //                else
            //                {
            //                    explodedResultText += explodedRoll + ")";
            //                    roll -= explodedRoll;
            //                }
            //            }
            //        }
            //        explodedResultText = roll + " " + explodedResultText;

            //        result.Result += roll;

            //        if (!string.IsNullOrWhiteSpace(explodedResultText))
            //        {
            //            result.Accounting += explodedResultText;
            //        }
            //        if ((i < numDice))
            //        {
            //            result.Accounting += " + ";
            //        }
            //        //else
            //        //{
            //        //    result.FullResults += " = ";
            //        //}
            //    }
            //}


            ////Smooth exploding dice
            //if ((diceType == "s"
            //    || diceType == "su"
            //    || diceType == "sd")
            //    && !threshold.HasValue)
            //{
            //    for (int i = 1; (i <= numDice); i++)
            //    {
            //        int smoothSides = 1;
            //        if (diceType == "s") { smoothSides = 2; }

            //        int roll = Dice.Roll(numSides + smoothSides);

            //        string explodedResultText = "";

            //        if (roll == numSides + smoothSides && (diceType == "s" || diceType == "su"))
            //        {
            //            //Smooth explode up
            //            roll -= smoothSides;

            //            explodedResultText += "(UP: " + roll + "+";
            //            int explodedRoll = 0;

            //            while (explodedRoll == 0 || explodedRoll == numSides + 1)
            //            {
            //                explodedRoll = Dice.Roll(numSides + 1);
            //                if (explodedRoll == numSides + 1)
            //                {
            //                    explodedResultText += numSides + "+";
            //                    roll += numSides;
            //                }
            //                else
            //                {
            //                    explodedResultText += explodedRoll + ")";
            //                    roll += explodedRoll;
            //                }
            //            }
            //        }
            //        else if (roll == numSides + 1 && (diceType == "s" || diceType == "sd"))
            //        {
            //            //Smooth explode down
            //            roll = 1;

            //            explodedResultText += "(DN: " + roll + "-";
            //            int explodedRoll = 0;

            //            while (explodedRoll == 0 || explodedRoll == numSides + 1)
            //            {
            //                explodedRoll = Dice.Roll(numSides + 1);
            //                if (explodedRoll == numSides + 1)
            //                {
            //                    explodedResultText += numSides + "-";
            //                    roll -= numSides;
            //                }
            //                else
            //                {
            //                    explodedResultText += explodedRoll + ")";
            //                    roll -= explodedRoll;
            //                }
            //            }
            //        }

            //        explodedResultText = roll + " " + explodedResultText;

            //        result.Result += roll;

            //        if (!string.IsNullOrWhiteSpace(explodedResultText))
            //        {
            //            result.Accounting += explodedResultText;
            //        }
            //        if ((i < numDice))
            //        {
            //            result.Accounting += " + ";
            //        }
            //        //else
            //        //{
            //        //    result.FullResults += " = ";
            //        //}
            //    }

            //}

            ////Threshold dice
            //if (threshold.HasValue)
            //{
            //    int successes = 0;
            //    string tresultText = "";

            //    //roll just one die of the type 
            //    string newDiceExpression = match.Groups["dsides"].Value;
            //    for (int i = 1; (i <= numDice); i++)
            //    {

            //        int tresult = Dice.Roll(newDiceExpression);
            //        tresultText += tresult;
            //        if (tresult >= threshold.Value)
            //        {
            //            successes += 1;
            //            tresultText += "*";
            //            if (thresholdType == "b" && tresult >= numSides)
            //            {
            //                //max rolls "burst" to new chances

            //                int burstRoll = 0;
            //                tresultText += "(BURST: ";

            //                while (burstRoll == 0 || burstRoll >= numSides)
            //                {
            //                    burstRoll = Dice.Roll(newDiceExpression);
            //                    tresultText += burstRoll;
            //                    if (burstRoll >= threshold)
            //                    {
            //                        successes += 1;
            //                        tresultText += "*";
            //                    }
            //                    if (burstRoll == numSides)
            //                    {
            //                        tresultText += ", ";
            //                    }
            //                }
            //                tresultText += ")";

            //            }
            //        }
            //        if (i != numDice)
            //        {
            //            tresultText += ", ";
            //        }
            //    }

            //    result.Result = successes;

            //    if (!string.IsNullOrWhiteSpace(tresultText))
            //    {
            //        result.Accounting += tresultText;
            //    }
            //    //result.FullResults += " = ";

            //}


            //result.Result += modifier;



            //if ((modifier < 0))
            //{
            //    result.Accounting += " (" + modifier + ")";
            //}
            //if ((modifier > 0))
            //{
            //    result.Accounting += " (+" + modifier + ")";
            //}

            //// Limits occur after Modifiers are applied        
            //if ((lowerlimit.HasValue && (result.Result < lowerlimit)))
            //{
            //    result.Accounting = result.Result + " (FLOOR " + lowerlimit.Value + ") = " + result.Accounting;
            //    result.Result = lowerlimit.Value;

            //}
            //// Limits occur after Modifiers are applied        
            //if ((upperlimit.HasValue && (result.Result > upperlimit)))
            //{
            //    result.Accounting = result.Result + " (CEILING " + upperlimit.Value + ") = " + result.Accounting;
            //    result.Result = upperlimit.Value;

            //}

            //result.Accounting = result.Result + " = " + result.Accounting;
            return result;
        }


        public static bool ValidateDiceExpression(string DiceExpression)
        {
            if (String.IsNullOrWhiteSpace(DiceExpression))
            {
                return false;
            }
            else
            {
                return DiceParameters.diceregex.IsMatch(DiceExpression);
            }
        }


        public static string NumberToText(int n)
        {
            if (n == 0)
            {
                return "Zero";
            }
            else
            {
                return myNumberToText(n).Trim();
            }
        }

        private static string myNumberToText(int n)
        {
            if (n < 0)
                return "Minus " + myNumberToText(-n);
            else if (n == 0)
                return "";
            else if (n <= 19)
                return new string[] {"One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", 
         "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", 
         "Seventeen", "Eighteen", "Nineteen"}[n - 1] + " ";
            else if (n <= 99)
                return new string[] {"Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", 
         "Eighty", "Ninety"}[n / 10 - 2] + " " + myNumberToText(n % 10);
            else if (n <= 199)
                return "One Hundred " + myNumberToText(n % 100);
            else if (n <= 999)
                return myNumberToText(n / 100) + "Hundred " + myNumberToText(n % 100);
            else if (n <= 1999)
                return "One Thousand " + myNumberToText(n % 1000);
            else if (n <= 999999)
                return myNumberToText(n / 1000) + "Thousand " + myNumberToText(n % 1000);
            else if (n <= 1999999)
                return "One Million " + myNumberToText(n % 1000000);
            else if (n <= 999999999)
                return myNumberToText(n / 1000000) + "Million " + myNumberToText(n % 1000000);
            else if (n <= 1999999999)
                return "One Billion " + myNumberToText(n % 1000000000);
            else
                return myNumberToText(n / 1000000000) + "Billion " + myNumberToText(n % 1000000000);
        }

    }



}
