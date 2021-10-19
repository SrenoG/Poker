using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;

class RNGCSP {
    private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
    // Main method.
    private Dictionary<byte, byte> distCard = new Dictionary<byte, byte>();
    static int[] cardNumber = new int[13];
    static int[] cardColor = new int[4];
    static byte[] board = new byte[5];
    public static void Main() {
        //var decks = Enumerable.Range(0, 52);
        //for (int i = 0; i < 10; i++) {
        //    var d = decks.OrderBy(x => Guid.NewGuid().ToString("N")).Take(2).ToArray();
        //    Console.WriteLine(d[0] + " " + d[1]);
        //}
        //return;


        int numberOfPlayer = 2;
        List<player> playerList = new List<player>();
        //List<byte> distributedCard = new List<byte>();

        for (int i = 1; i <= numberOfPlayer; i++) {
            KeyValuePair<byte, byte> first;
            KeyValuePair<byte, byte> second;
            do {
                first = GenerateCard(cardNumber, cardColor);
                second = GenerateCard(cardNumber, cardColor);
            } while (distributedCard.Contains(first) || distributedCard.Contains(second));
            distributedCard.Add(first);
            distributedCard.Add(second);
            playerList.Add(new player() {
                Name = "Player" + i,
                Card1 = Enum.GetName(typeof(Card), first),
                Card2 = Enum.GetName(typeof(Card), second),
            });
        }
        for (int i = 0; i <= board.Length - 1; i++) {
            do {
                board[i] = RollDice((byte)results.Length);
            }
            while (distributedCard.Contains(board[i]));
            distributedCard.Add(board[i]);
        }
        foreach (player item in playerList) {
            Console.WriteLine("-------------------");
            Console.WriteLine("NOM :" + item.Name);
            Console.WriteLine("CARTE 1 :" + item.Card1);
            Console.WriteLine("CARTE 2 :" + item.Card2);
            Console.WriteLine("-------------------");
        }
        Console.WriteLine("--------------");
        Console.WriteLine("FLOP :");
        Console.WriteLine("--------------");
        for (int i = 0; i < 3; i++) {
            Console.WriteLine(Enum.GetName(typeof(Card), board[i]));
        }
        Console.WriteLine("Continue?");
        Console.ReadKey();
        Console.WriteLine("--------------");
        Console.WriteLine("Turn :");
        Console.WriteLine(Enum.GetName(typeof(Card), 3));
        Console.WriteLine("--------------");
        Console.WriteLine("Continue?");
        Console.ReadKey();
        Console.WriteLine("--------------");
        Console.WriteLine("River :");
        Console.WriteLine(Enum.GetName(typeof(Card), 4));
        Console.WriteLine("--------------");
        Console.ReadKey();

        rngCsp.Dispose();
    }

    private static KeyValuePair<byte, byte> GenerateHand(int[] cardNumber, int[] cardColor) {
        return new KeyValuePair<byte, byte>(RollDice((byte)cardNumber.Length), RollDice((byte)cardColor.Length));
    }

    public class player {
        public string Name { get; set; }
        KeyValuePair<byte, byte> Card1 { get; set; }
        KeyValuePair<byte, byte> Card2 { get; set; }

    }


    //public enum CardNumber {
    //    Deux = 2,
    //    Trois = 3,
    //    Quatre = 4,
    //    Cinq = 5,
    //    Six = 6,
    //    Sept = 7,
    //    Huit = 8,
    //    Neuf = 9,
    //    Dix = 10,
    //    Valet = 11,
    //    Dame = 12,
    //    Roi = 13,
    //    As = 14
    //}

    public List<byte> CardNumber { 
        get {
            return new List<byte>() {
                1,
                2,
                3,
                4,
                5,
                6,
                7,
                8,
                9,
                10,
                11,
                12,
                13
            };
        }
    }
    public List<byte> CardColor {
        get {
            return new List<byte>() {
                1,
                2,
                3,
                4,
            };
        }
    }



    public enum Card {
        DeuxCarreau = 1,
        TroisCarreau = 2,
        QuatreCarreau = 3,
        CinqCarreau = 4,
        SixCarreau = 5,
        SeptCarreau = 6,
        HuitCarreau = 7,
        NeufCarreau = 8,
        DixCarreau = 9,
        ValetCarreau = 10,
        DameCarreau = 11,
        RoiCarreau = 12,
        AsCarreau = 13,

        DeuxCoeur = 14,
        TroisCoeur = 15,
        QuatreCoeur = 16,
        CinqCoeur = 17,
        SixCoeur = 18,
        SeptCoeur = 19,
        HuitCoeur = 20,
        NeufCoeur = 21,
        DixCoeur = 22,
        ValetCoeur = 23,
        DameCoeur = 24,
        RoiCoeur = 25,
        AsCoeur = 26,

        DeuxTrefle = 27,
        TroisTrefle = 28,
        QuatreTrefle = 29,
        CinqTrefle = 30,
        SixTrefle = 31,
        SeptTrefle = 32,
        HuitTrefle = 33,
        NeufTrefle = 34,
        DixTrefle = 35,
        ValetTrefle = 36,
        DameTrefle = 37,
        RoiTrefle = 38,
        AsTrefle = 39,

        DeuxPique = 40,
        TroisPique = 41,
        QuatrePique = 42,
        CinqPique = 43,
        SixPique = 44,
        SeptPique = 45,
        HuitPique = 46,
        NeufPique = 47,
        DixPique = 48,
        ValetPique = 49,
        DamePique = 50,
        RoiPique = 51,
        AsPique = 52,
    }

    // This method simulates a roll of the dice. The input parameter is the
    // number of sides of the dice.

    public static byte RollDice(byte numberSides) {
        if (numberSides <= 0)
            throw new ArgumentOutOfRangeException("numberSides");

        // Create a byte array to hold the random value.
        byte[] randomNumber = new byte[1];
        do {
            // Fill the array with a random value.
            rngCsp.GetBytes(randomNumber);
        }
        while (!IsFairRoll(randomNumber[0], numberSides));
        // Return the random number mod the number
        // of sides.  The possible values are zero-
        // based, so we add one.
        return (byte)((randomNumber[0] % numberSides) + 1);
    }

    private static bool IsFairRoll(byte roll, byte numSides) {
        // There are MaxValue / numSides full sets of numbers that can come up
        // in a single byte.  For instance, if we have a 6 sided die, there are
        // 42 full sets of 1-6 that come up.  The 43rd set is incomplete.
        int fullSetsOfValues = Byte.MaxValue / numSides;

        // If the roll is within this range of fair values, then we let it continue.
        // In the 6 sided die case, a roll between 0 and 251 is allowed.  (We use
        // < rather than <= since the = portion allows through an extra 0 value).
        // 252 through 255 would provide an extra 0, 1, 2, 3 so they are not fair
        // to use.
        return roll < numSides * fullSetsOfValues;
    }
}