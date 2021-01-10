//  Dices class
using System;

namespace legend
{
    /// <summary>
    /// Different dices for use.
    /// </summary>
    public enum Dice { dice1k3, dice1k6, percent, dice1k10 };
    
    /// <summary>
    /// This class covers all dice values, needed for AGE RPG system dice roll.
    /// </summary>
    public class DicesRoll
    {
        public int[] standardDices = new int[2];  // roll of 2 standard dices
        public int dramaDice;      // roll on drama dice
        public int total;          // total number on dices

        public DicesRoll()
        {
            MakeRoll();
        }

        public int WasDrama()
        {
            int res = 0;    // No, it wasn't

            if (standardDices[0]==standardDices[1]) res = dramaDice;
            if (standardDices[0]==dramaDice) res = dramaDice;
            if (standardDices[1]==dramaDice) res = dramaDice;

            return res;
        }

        public void MakeRoll()
        {
            Dices dices = new Dices();
            standardDices[0] = dices.ThrowDice();
            standardDices[1] = dices.ThrowDice();
            dramaDice = dices.ThrowDice();
            total = standardDices[0] + standardDices[1] + dramaDice; 
        }
    }

    /// <summary>
    /// This class covers random generator for throwing dices.
    /// </summary>
    public class Dices
    {
        // Random generator
        Random rnd = new Random(DateTime.Now.Millisecond);   

        /// <summary>
        /// Make 1k6 roll.
        /// </summary>
        /// <returns>Summary from dice throw</returns>
        public int ThrowDice()
        {
            return rnd.Next(1,7);
        }

        /// <summary>
        /// Make roll with particular dice.
        /// </summary>
        /// <param name="diceToUse">Which dice to use</param>
        /// <returns>Summary from dice throw</returns>
        public int ThrowDice(Dice diceToUse)
        {
            int value = 7;  // Defaut, 6-sided dice
            if (diceToUse == Dice.dice1k3) value = 4;
            if (diceToUse == Dice.dice1k10) value = 11;
            if (diceToUse == Dice.percent) value = 101;
            return rnd.Next(1,value);
        }

        /// <summary>
        /// Make 1k3 roll
        /// </summary>
        /// <returns>Summary from dice throw</returns>
        public int ThrowDice1k3()
        {
            return rnd.Next(1,4);
        }

        /// <summary>
        /// When we need to throw X-dices, with special side number.
        /// </summary>
        /// <param name="count">Dices count.</param>
        /// <param name="sides">Count of dice sides.</param>
        /// <returns>Summary from dices throw.</returns>
        public int ThrowDiceX(int count, int sides)
        {
            int res = 0;
            for (int a=0;a<count;a++)
            {
                res = res + rnd.Next(1,sides+1);
            }
            return res;
        }

        /// <summary>
        /// Special way, how to get dice throw. When we have a string
        /// like this "1k6+3", we can use this method to get result
        /// of such dice throw.
        /// </summary>
        /// <param name="diceThrow">Input string in "1k6+3" format.</param>
        /// <returns>Summary from dices throw.</returns>
        public int ThrowDiceString(string diceThrow)
        {
            // example input: 1k6+3
            int total = 0;  // error throw

            string tmp = "" + diceThrow[0];
            int diceCount=Int32.Parse(tmp);

            tmp = "" + diceThrow[2];
            int diceSide=Int32.Parse(tmp);

            int additionalDmg = 0;
            if (diceThrow.Length>3)
            {
                tmp = "" + diceThrow[4];
                additionalDmg=Int32.Parse(tmp);
            
                if (diceThrow[3]=='-') additionalDmg = additionalDmg * -1;
            }

            total = ThrowDiceX(diceCount,diceSide);
            total += additionalDmg;

            return total;
        }
    }
}