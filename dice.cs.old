using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;

class RNGCSP {
    private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
    // Main method.
    static int[] cardNumber = new int[13];
    static int[] cardColor = new int[4];
    static KeyValuePair<byte, byte>[] board = new KeyValuePair<byte, byte>[5];
    public static void Main() {
        List<Card> distCard = new List<Card>();
        //Dictionary<byte, byte> distCard = new Dictionary<byte, byte>();
        //var decks = Enumerable.Range(0, 52);
        //for (int i = 0; i < 10; i++) {
        //    var d = decks.OrderBy(x => Guid.NewGuid().ToString("N")).Take(2).ToArray();
        //    Console.WriteLine(d[0] + " " + d[1]);
        //}
        //return;


        int numberOfPlayer = 2;
        List<Player> playerList = new List<Player>();

        for (int i = 1; i <= numberOfPlayer; i++) 
        {
            playerList.Add(new Player() 
            { 
                Name = "Player" + i,
                Hand = new Hand ()
                { 
                    Card1 = GenerateCard(cardNumber, cardColor, distCard),
                    Card2 = GenerateCard(cardNumber, cardColor, distCard)
                }
            });
        }

        foreach (Player player in playerList) {
            Console.WriteLine("-------------------");
            Console.WriteLine("NOM :" + player.Name);
            Console.WriteLine("CARTE 1 :" + getCard(player.Hand.Card1));
            Console.WriteLine("CARTE 2 :" + getCard(player.Hand.Card2));
            Console.WriteLine("-------------------");
        }
        Console.WriteLine("--------------");
        Console.WriteLine("FLOP :");
        Console.WriteLine("--------------");
        for (int i = 0; i < 3; i++) {
            Console.WriteLine(getCard(GenerateCard(cardNumber, cardColor, distCard)));
        }
        Console.WriteLine("Continue?");
        Console.ReadKey();
        Console.WriteLine("--------------");
        Console.WriteLine("Turn :");
        Console.WriteLine(getCard(GenerateCard(cardNumber, cardColor, distCard)));
        Console.WriteLine("--------------");
        Console.WriteLine("Continue?");
        Console.ReadKey();
        Console.WriteLine("--------------");
        Console.WriteLine("River :");
        Console.WriteLine(getCard(GenerateCard(cardNumber, cardColor, distCard)));
        Console.WriteLine("--------------");
        Console.ReadKey();
        rngCsp.Dispose();
    }

    private static Card GenerateCard(int[] cardNumber, int[] cardColor, List<Card> distCard) {
        Card card = new Card();
        do {
            card.KvpCard = new KeyValuePair<byte, byte>(RollDice((byte)cardNumber.Length), RollDice((byte)cardColor.Length));
        }
        while (distCard.Contains(card));
        distCard.Add(card);
        return card;
    }

    static protected string getCard(Card card) {
        string number = dicoNumber.FirstOrDefault(x => x.Key == card.KvpCard.Key).Value;
        string color = dicoColor.FirstOrDefault(x => x.Key == card.KvpCard.Value).Value;
        return number + " " + color;
    }

    public class Player {
        public string Name { get; set; }
        private Hand _hand;
        public Hand Hand {
            get {
                if (_hand == null)
                    return new Hand();
                else
                    return _hand;
            }
            set {
                _hand = value;
            }
        }
    }
    public class Card {
        public KeyValuePair<byte, byte> KvpCard { get; set; }
    }
    public class Hand {
        public Card Card1 { get; set; }
        public Card Card2 { get; set; }
        //private Card _card1;
        //public Card Card1 {
        //    get 
        //        {
        //            if (_card1 == null)
        //                return _card1;
        //            else
        //                return _card1;
        //        }
        //    set { _card1 = value; }
        //}
        //private Card _card2;
        //public Card Card2 {
        //    get {
        //        if (_card2 == null)
        //            return _card2;
        //        else
        //            return _card2;
        //    }
        //    set { _card2 = value; }
        //}
    }


    static public IDictionary<int, string> dicoNumber {
        get {
            return new Dictionary<int, string>() {
                {1, "As" },
                {2, "Deux" },
                {3, "Trois" },
                {4, "Quatre" },
                {5, "Cinq" },
                {6, "Six" },
                {7, "Sept" },
                {8, "Huit" },
                {9, "Neuf" },
                {10, "Dix" },
                {11, "Valet" },
                {12, "Dame" },
                {13, "Roi" },
            };
        }
    }
    //static public IDictionary<int, int> dicoNumber {
    //    get {
    //        return new Dictionary<int, int>() {
    //            {1, 10 },
    //            {2, 20 },
    //            {3, 30 },
    //            {4, 40 },
    //            {5, 50 },
    //            {6, 60 },
    //            {7, 70 },
    //            {8, 80 },
    //            {9, 90 },
    //            {10, 100 },
    //            {11, 110 },
    //            {12, 120 },
    //            {13, 130 },
    //        };
    //    }
    //}
    //static public IDictionary<int, int> dicoColor {
    //    get {
    //        return new Dictionary<int, int>() {
    //            {1, 1 },
    //            {2, 2 },
    //            {3, 3 },
    //            {4, 4 },
    //        };
    //    }
    //}
    static public IDictionary<int, string> dicoColor {
        get {
            return new Dictionary<int, string>() {
                {1, "Carreau" },
                {2, "Coeur" },
                {3, "Trèfle" },
                {4, "Pique" },
            };
        }
    }

    #region Generate number
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
    #endregion
}