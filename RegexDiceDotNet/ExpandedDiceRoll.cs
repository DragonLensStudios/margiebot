using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexDiceDotNet
{
    public class ExpandedDiceRoll
    {
        public string Accounting { get; set; }
        public int FullResult
        {
            get
            {
                int result = 0;
                foreach (Roll r in this.Rolls)
                {
                    result += r.Result;
                    foreach (Roll extraRoll in r.ExtraRolls)
                    {
                        if (extraRoll.RollType == RollType.ExplodedDown)
                        {
                            result -= extraRoll.Result;
                        }
                        else
                        {
                            result += extraRoll.Result;
                        }

                    }
                }
                return result;
            }
        }
        public List<Roll> Rolls { get; set; }

        public DiceParameters DiceParameters { get; set; }

        public Boolean IsValid { get; set; }
        public ExpandedDiceRoll()
        {
            Rolls = new List<Roll>();
        }
        public void Roll()
        {

            for (int rollNum = 1; rollNum <= DiceParameters.NumberOfDice; rollNum++)
            {
                Roll currentRoll = new Roll();
                this.Rolls.Add(currentRoll);
                if (this.DiceParameters.DiceType == DiceType.Normal)
                {
                    currentRoll.RollValue = Dice.Roll(DiceParameters.NumberOfSides);
                    currentRoll.Result = currentRoll.RollValue;
                    currentRoll.RollType = RollType.Normal;
                    this.Accounting += currentRoll.Result.ToString();
                }
                else if (this.DiceParameters.DiceType == DiceType.SmoothlyExploding
                    || this.DiceParameters.DiceType == DiceType.SmoothlyExplodingDownward
                    || this.DiceParameters.DiceType == DiceType.SmoothlyExplodingUpward)
                {
                    int extraSides = 1;
                    if (this.DiceParameters.DiceType == DiceType.SmoothlyExploding)
                    {
                        extraSides = 2;
                    }

                    int r = Dice.Roll(this.DiceParameters.NumberOfSides + extraSides) - extraSides;

                    if (r > 0)
                    {
                        //Did not explode
                        currentRoll.RollValue = r;
                        currentRoll.Result = r;
                        currentRoll.RollType = RollType.NotExploded;
                        this.Accounting += currentRoll.Result.ToString();
                    }
                    else if (r == -1
                        || (r == 0 && this.DiceParameters.DiceType == DiceType.SmoothlyExplodingUpward))
                    {
                        //Explode Up
                        currentRoll.RollValue = this.DiceParameters.NumberOfSides;
                        currentRoll.Result = this.DiceParameters.NumberOfSides;
                        currentRoll.RollType = RollType.ExplodedUp;
                        this.Accounting += this.ExplodeSmoothly(currentRoll);
                    }
                    else if (r == 0)
                    {
                        //Explode Down
                        currentRoll.RollValue = 1;
                        currentRoll.Result = 1;
                        currentRoll.RollType = RollType.ExplodedDown;
                        this.Accounting += this.ExplodeSmoothly(currentRoll);
                    }


                }

                if (rollNum < this.DiceParameters.NumberOfDice)
                {
                    this.Accounting += " + ";
                }
            }

            this.Accounting = this.FullResult + " = " + this.Accounting;

 
        }

        private string ExplodeSmoothly(Roll originalRoll)
        {
            int r = 0;

            string accountingResult = "";

            if (originalRoll.RollType == RollType.ExplodedDown)
            {
                accountingResult = "(DN: " + originalRoll.RollValue;
            }
            else if (originalRoll.RollType == RollType.ExplodedUp)
            {
                accountingResult = "(UP: " + originalRoll.RollValue;
            }

            do
            {
                r = Dice.Roll(this.DiceParameters.NumberOfSides + 1) - 1;
                Roll extra = new Roll();
                originalRoll.ExtraRolls.Add(extra);
                if (r < 1)
                {
                    extra.RollValue = this.DiceParameters.NumberOfSides;
                    extra.Result = extra.RollValue;
                }
                if (r < 1 && originalRoll.RollType == RollType.ExplodedDown)
                {
                    originalRoll.Result -= extra.RollValue;
                    extra.RollType = RollType.ExplodedDown;
                    accountingResult += " - " + extra.RollValue.ToString();
                }
                if (r < 1 && originalRoll.RollType == RollType.ExplodedUp)
                {
                    originalRoll.Result += extra.RollValue;
                    extra.RollType = RollType.ExplodedUp;
                    accountingResult += " + " + extra.RollValue.ToString();
                }
                if (r > 0 && originalRoll.RollType == RollType.ExplodedDown)
                {
                    originalRoll.Result -= r;
                    extra.RollType = RollType.NotExploded;
                    accountingResult += " - " + r.ToString();
                }
                if (r > 0 && originalRoll.RollType == RollType.ExplodedUp)
                {
                    originalRoll.Result += r;
                    extra.RollType = RollType.NotExploded;
                    accountingResult += " + " + r.ToString();
                }
            } while (r < 1);

            accountingResult = originalRoll.Result.ToString() + accountingResult + ")";

            return accountingResult;
        }

    } //ExpandedDiceRoll




    /// <summary>
    /// Represents the roll of one die.
    /// </summary>
    public class Roll
    {
        /// <summary>
        /// The value actually rolled by the die.
        /// </summary>
        public int RollValue { get; set; }

        /// <summary>
        /// The value as it will count in the result.
        /// </summary>
        public int Result { get; set; }

        public Boolean IncludeInFullResult { get; set; }

        /// <summary>
        /// Do I need this anymore?
        /// </summary>
        public RollType RollType { get; set; }

        /// <summary>
        /// Rolls generated from Threshold Bursting or Exploding.
        /// </summary>
        public List<Roll> ExtraRolls { get; set; }
        public Roll()
        {
            ExtraRolls = new List<Roll>();
            IncludeInFullResult = true;
            RollType = RollType.Normal;
        }
    }

    public enum RollType
    {
        Normal,
        NotExploded,
        ExplodedUp,
        ExplodedDown,
        Threshold,
        ThresholdBurst
    }

}
