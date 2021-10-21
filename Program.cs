using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;

class RNGCSP {
    private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
    // Main method.
    static int[] DeckCount = new int[52];
    public static void Main() {
        bool again = true;
        while(again)
        {
            List<int> distCard = new List<int>();
            List<int> boardCard = new List<int>();
            int numberOfPlayer = 2;
            List<Player> playerList = CreatePlayer(numberOfPlayer, distCard).ToList();
            PrintPlayerCard(playerList);          
            WriteBoard("FLOP", 3, distCard, boardCard);
            WriteBoard("TURN", 1, distCard, boardCard);
            WriteBoard("RIVER", 1, distCard, boardCard);
            GetResult(playerList, boardCard);
            again = SetAgain();
        }
        rngCsp.Dispose();
    }

    private static void GetResult(List<Player> playerList, List<int> boardCard)
    {
        throw new NotImplementedException();
    }

    private static void PrintPlayerCard(List<Player> playerList)
    {
        foreach (Player player in playerList)
        {
            Console.Clear();
            Console.WriteLine("-------------------");
            Console.WriteLine("NOM     : " + player.Name);
            Console.WriteLine("CARTE 1 : " + getCard(player.Hand.Card1));
            Console.WriteLine("CARTE 2 : " + getCard(player.Hand.Card2));
            Console.WriteLine("-------------------");
        }
    }
    private static bool SetAgain()
    {
        string read = "";
        do
        {
            Console.Clear();
            Console.WriteLine("Main suivante ? Y ou N");
            read = Console.ReadLine().ToLower();
        }
        while (read != "y" && read != "n");
        if (read == "y")
            return true;
        else
            return false;
    }
    private static void WriteBoard(string v1, int v2, List<int> distCard, List<int> boardCard)
    {
        Console.WriteLine("--------------");
        Console.WriteLine(v1 + " :");
        Console.WriteLine("--------------");
        for (int i = 0; i < v2; i++)
        {
            Console.WriteLine(getCard(GenerateCard(distCard, boardCard)));
        }
        Console.WriteLine("Continue?");
        Console.ReadKey();
    }
    private static IEnumerable<Player> CreatePlayer(int numberOfPlayer, List<int> distCard)
    {
        for (int i = 1; i <= numberOfPlayer; i++)
        {
            yield return new Player()
            {
                Name = "Player" + i,
                Hand = new Hand()
                {
                    Card1 = GenerateCard(distCard),
                    Card2 = GenerateCard(distCard)
                }
            };
        }
    }
    private static Card GenerateCard(List<int> distCard) {
        Card card = new Card();
        int tmp;
        do {
            tmp = Deck.ToList()[RollDice((byte)DeckCount.Length)];
        }
        while (distCard.Contains(tmp));
        //TODO: CardReference had accessor now => to improve
        card.CardReference = tmp;
        distCard.Add(tmp);
        return card;
    }
    private static Card GenerateCard(List<int> distCard, List<int> boardCard)
    {
        Card card = new Card();
        int tmp;
        do
        {
            tmp = Deck.ToList()[RollDice((byte)DeckCount.Length)];
        }
        while (distCard.Contains(tmp));
        card.CardReference = tmp;
        distCard.Add(tmp);
        boardCard.Add(tmp);
        return card;
    }
    static protected string getCard(Card card) {
        return card.NumberString + card.Symbole;
    }
    public class Player {

        public int[] BestHand = new int[5];
        public int[] MaxCard = new int[7];
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
        private int _cardReference;
        public int CardReference 
        { 
            set
            {
                _cardReference = value;
            }
            get
            {
                return _cardReference;
            }
        }
        public string Symbole
        {
            get 
            {
                return symboleDico.FirstOrDefault(x => x.Key == _cardReference % 10).Value;
            }
        }

        public string NumberString
        {
            get 
            { 
                return dicoNumberString.FirstOrDefault(x => x.Key == _cardReference / 10).Value; 
            }
        }

    }
    public class Hand {
        public Card Card1 { get; set; }
        public Card Card2 { get; set; }
    }
    static public IDictionary<int, string> dicoNumberString
    {
        get
        {
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
    static public IDictionary<int, int> dicoGenerateReferenceNumber
    {
        get
        {
            return new Dictionary<int, int>() {
                {1, 10 },
                {2, 20 },
                {3, 30 },
                {4, 40 },
                {5, 50 },
                {6, 60 },
                {7, 70 },
                {8, 80 },
                {9, 90 },
                {10, 100 },
                {11, 110 },
                {12, 120 },
                {13, 130 },
            };
        }
    }
    static public IDictionary<int, string> symboleDico
    {
        get
        {
            return new Dictionary<int, string>() {
                {1, "♦" },
                {2, "♥" },
                {3, "♣ " },
                {4, "♠" },
            };
        }
    }
    static public IEnumerable<int> Deck
    {
        get 
        {
            List<int> temp = new List<int>();
            foreach (KeyValuePair<int, int> number in dicoGenerateReferenceNumber)
            {
                foreach (KeyValuePair<int, string> color in symboleDico)
                {
                    temp.Add(number.Value + color.Key);
                }
            }
            return temp.OrderBy(x => Guid.NewGuid().ToString("N"));
        }

    }

    #region Generate number
    public static byte RollDice(byte numberSides) {
        if (numberSides < 0)
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