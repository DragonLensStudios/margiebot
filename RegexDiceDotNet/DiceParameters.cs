using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

namespace RegexDiceDotNet
{
    public class DiceParameters
    {

        public const string DicePattern = @"^(?<numdice>\d*)(?<dsides>(?<type>[dDeEsS]|eu|EU|ed|ED|su|SU|sd|SD)(?<numsides>\d+))((?<keepseperator>[kKlL])(?<keepnumber>\d*))?(?<threshold>(?<thresholdtype>[b|B|t|T])(?<thresholdlimit>(?<thresholdsign>[\+\-])?(?<thresholdvalue>\d*)))?(?<modifier>(?<sign>[\+\-])(?<addend>\d*))?(?<floor>(?<floorseperator>[f|F])(?<floorlimit>(?<floorsign>[\+\-])?(?<floorlimiter>\d*)))?(?<ceiling>(?<ceilingseperator>[c|C])(?<ceilinglimit>(?<ceilingsign>[\+\-])?(?<ceilinglimiter>\d*)))?$";
        public static Regex diceregex = new Regex(DicePattern);

        //@"^(?<numdice>\d*)(?<dsides>(?<type>[dDeEsS]|eu|EU|ed|ED|su|SU|sd|SD)(?<numsides>\d+))
        //((?<keepseperator>[kKlL])(?<keepnumber>\d*))?
        //(?<threshold>(?<thresholdtype>[b|B|t|T])(?<thresholdlimit>(?<thresholdsign>[\+\-])?(?<thresholdvalue>\d*)))?
        //(?<modifier>(?<sign>[\+\-])(?<addend>\d*))?
        //(?<floor>(?<floorseperator>[f|F])(?<floorlimit>(?<floorsign>[\+\-])?(?<floorlimiter>\d*)))?
        //(?<ceiling>(?<ceilingseperator>[c|C])(?<ceilinglimit>(?<ceilingsign>[\+\-])?(?<ceilinglimiter>\d*)))?$"

        /// <summary>
        /// The number of dice to roll.  Corresponds to this portion of the regex: "(?<numdice>\d*)".
        /// </summary>
        public int NumberOfDice { get; set; }

        /// <summary>
        /// The type of dice to roll. Corresponds to this portion of the regex: "(?<type>[dDeEsS]|eu|EU|ed|ED|su|SU|sd|SD)".
        /// </summary>
        public DiceType DiceType { get; set; }

        /// <summary>
        /// The number of sides on the dice to roll. Corresponds to this portion of the regex: "(?<numsides>\d+)".
        /// </summary>
        public int NumberOfSides { get; set; }

        /// <summary>
        /// If present, indicates whether to keep high or low rolls.  Corresponse to this portion of the regex: "(?<keepseperator>[kKlL])".
        /// </summary>
        public KeepType? KeepType { get; set; }

        /// <summary>
        /// If present, indicates how many dice to keep.  Corresponds to this portion of the regex: "(?<keepnumber>\d*)".
        /// </summary>
        public int? KeepNumber { get; set; }

        /// <summary>
        /// If present, indicates whether or not threshold dice "burst" into extra dice when the result is greater than or equal to 
        /// NumberOfSides. Corresponds to this portion of the regex: "(?<thresholdtype>[b|B|t|T])".
        /// </summary>
        public ThresholdType? ThresholdType { get; set; }

        /// <summary>
        /// When rolling threshold dice, add one to the result for every die roll equal to or greater than the ThresholdValue.
        /// Corresponds to this portion of the regex: "(?<thresholdlimit>(?<thresholdsign>[\+\-])?(?<thresholdvalue>\d*))".
        /// </summary>
        public int? ThresholdValue { get; set; }

        /// <summary>
        /// The amount to add to the total before returning the final result.  Corresponds to this portion of the regex: "(?<modifier>(?<sign>[\+\-])(?<addend>\d*))?".
        /// </summary>
        public int? Modifier { get; set; }

        /// <summary>
        /// If the result is less than FloorValue, the FinalResult will be equal to FloorValue.  Corresponds to this portion of the regex: "(?<floorlimit>(?<floorsign>[\+\-])?(?<floorlimiter>\d*))".
        /// </summary>
        public int? Floor { get; set; }

        /// <summary>
        /// If the result is greater than CeilingValue, the FinalResult will be equal to CeilingValue. Corresponds to this portion of the regex: "(?<ceilinglimit>(?<ceilingsign>[\+\-])?(?<ceilinglimiter>\d*)))".
        /// </summary>
        public int? Ceiling { get; set; }

        /// <summary>
        /// Indicates whether or not the dice expression string is valid.  See ErrorMessage for details if false.
        /// </summary>
        public Boolean IsValid { get; set; }

        public string ErrorMessage { get; set; }

        public Boolean IsSmooth
        {
            get
            {
                return (this.DiceType == DiceType.SmoothlyExploding
                    || this.DiceType == DiceType.SmoothlyExplodingDownward
                    || this.DiceType == DiceType.SmoothlyExplodingUpward);
            }
        }

        private string _diceExpression;



        public string DiceExpression
        {
            get
            {
                return this._diceExpression;
            }
            set 
            {
                this._diceExpression = value;
                Match match = diceregex.Match(value);
                if (!match.Success)
                {
                    this.ErrorMessage = String.Format(@"""{0}"" is not a valid dice expression", this._diceExpression);
                    this.IsValid = false;
                }
                else
                {
			        if (!string.IsNullOrWhiteSpace(match.Groups["numdice"].Value))
			        {
			            this.NumberOfDice = Int32.Parse(match.Groups["numdice"].Value);
			        }
                    else
                    {
                        this.NumberOfDice = 1;
                    }

                    string diceType = match.Groups["type"].Value.ToLower();
                    switch (diceType)
                    {
                        case "d":
                            this.DiceType = RegexDiceDotNet.DiceType.Normal;
                            break;
                        case "e":
                            this.DiceType = RegexDiceDotNet.DiceType.Exploding;
                            break;
                        case "eu":
                            this.DiceType = RegexDiceDotNet.DiceType.ExplodingUpward;
                            break;
                        case "ed":
                            this.DiceType = RegexDiceDotNet.DiceType.ExplodingDownward;
                            break;
                        case "s":
                            this.DiceType = RegexDiceDotNet.DiceType.SmoothlyExploding;
                            break;
                        case "su":
                            this.DiceType = RegexDiceDotNet.DiceType.SmoothlyExplodingUpward;
                            break;
                        case "sd":
                            this.DiceType = RegexDiceDotNet.DiceType.SmoothlyExplodingDownward;
                            break;
                    }

                    if (!string.IsNullOrWhiteSpace(match.Groups["numsides"].Value))
                    {
                        this.NumberOfSides = Int32.Parse(match.Groups["numsides"].Value);
                    }

                    this.KeepNumber = ParseNullableInteger(match.Groups["keepnumber"].Value);
                    if (this.KeepNumber.HasValue)
                    {
                        string keepType = match.Groups["keepseperator"].Value.ToLower();
                        switch (keepType)
                        {
                            case "k":
                                this.KeepType = RegexDiceDotNet.KeepType.High;
                                break;
                            case "l":
                                this.KeepType = RegexDiceDotNet.KeepType.Low;
                                break;
                        }
                    }

                    this.ThresholdValue = ParseNullableInteger(match.Groups["thresholdlimit"].Value);
                    if (this.ThresholdValue.HasValue)
                    {
                        string thresholdType = match.Groups["thresholdtype"].Value.ToLower();
                        switch (thresholdType)
                        {
                            case "b":
                                this.ThresholdType = RegexDiceDotNet.ThresholdType.Bursting;
                                break;
                            case "t":
                                this.ThresholdType = RegexDiceDotNet.ThresholdType.Simple;
                                break;
                        }

                    }


                    this.Floor = ParseNullableInteger(match.Groups["floorlimit"].Value);
                    this.Ceiling = ParseNullableInteger(match.Groups["ceilinglimit"].Value);
                    this.IsValid = true;
                }
            }
        }



        public DiceParameters()
        {
            this.IsValid = false;
            this.ErrorMessage = "No Dice Expression";
        }

        public DiceParameters(string DiceExpression)
        {
            this.DiceExpression = DiceExpression;
        }

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
    }

    public enum DiceType
    {
        Normal,
        Exploding,
        ExplodingUpward,
        ExplodingDownward,
        SmoothlyExploding,
        SmoothlyExplodingUpward,
        SmoothlyExplodingDownward
    }

    public enum KeepType
    {
        Low,
        High
    }

    public enum ThresholdType
    {
        Simple,
        Bursting
    }
}