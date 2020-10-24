﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strategy
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: Input  two args as radomeseed1 ane randomeseed2");
                Console.WriteLine("Example: 314 15");
                Environment.Exit(0);
            }

            int seed1 = int.Parse(args[0]);
            int seed2 = int.Parse(args[1]);

            Player player1 = new Player("Taro", new WinningStrategy(seed1));
            Player player2 = new Player("Hanako", new ProbStrategy(seed2));

            for (int i = 0; i < 10000; i++)
            {
                Hand nextHand1 = player1.NextHand();
                Hand nextHand2 = player2.NextHand();

                if (nextHand1.IsStrongerThan(nextHand2))
                {
                    Console.WriteLine($"Winner: {player1}");
                    player1.Win();
                    player2.Lose();
                }
                else if (nextHand2.IsStrongerThan(nextHand1))
                {
                    Console.WriteLine($"Winner: {player2}");
                    player1.Lose();
                    player2.Win();
                }
                else
                {
                    Console.WriteLine("Even...");
                    player1.Even();
                    player2.Even();
                }

                Console.WriteLine("Total result:");
                Console.WriteLine(player1.ToString());
                Console.WriteLine(player2.ToString());
            }
        }
    }

    public class Hand
    {
        public static readonly int HANDVALUE_GUU = 0;
        public static readonly int HANDVALUE_CHO = 1;
        public static readonly int HANDVALUE_PAA = 2;

        public static Hand[] hand = {
            new Hand(HANDVALUE_GUU),
            new Hand(HANDVALUE_CHO),
            new Hand(HANDVALUE_PAA),
        };

        private static readonly string[] name = {
            "グー", "チョキ", "パー"
        };

        private int handvalue;

        public static Hand GetHand(int handvalue)
        {
            return hand[handvalue];
        }

        private Hand(int handvalue)
        {
            this.handvalue = handvalue;
        }

        public bool IsStrongerThan(Hand h)
        {
            return Fight(h) == 1;
        }

        public bool IsWeakerThan(Hand h)
        {
            return Fight(h) == -1;
        }

        private int Fight(Hand h)
        {
            if (this == h)
            {
                return 0;
            }
            else if ((this.handvalue + 1) % 3 == h.handvalue)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        public override string ToString()
        {
            return name[handvalue];
        }
    }

    public interface Strategy
    {
        Hand NextHand();
        void Study(bool win);
    }

    public class WinningStrategy : Strategy
    {
        private Random random;
        private bool won = false;
        private Hand prevHand;

        public WinningStrategy(int seed)
        {
            random = new Random(seed);
        }

        public Hand NextHand()
        {
            if (!won)
            {
                prevHand = Hand.GetHand(random.Next(3));
            }
            return prevHand;
        }

        public void Study(bool win)
        {
            won = win;
        }
    }

    public class ProbStrategy : Strategy
    {
        private Random random;
        private int prevHandValue = 0;
        private int currentHandValue = 0;

        private int[][] history =
        {
            new int[] { 1, 1, 1, },
            new int[] { 1, 1, 1, },
            new int[] { 1, 1, 1, },
        };

        public ProbStrategy(int seed)
        {
            random = new Random(seed);
        }

        public Hand NextHand()
        {
            int bet = random.Next(GetSum(currentHandValue));
            int handvalue = 0;

            if (bet < history[currentHandValue][0])
            {
                handvalue = 0;
            }
            else if (bet < history[currentHandValue][0] + history[currentHandValue][1])
            {
                handvalue = 1;
            }
            else
            {
                handvalue = 2;
            }

            prevHandValue = currentHandValue;
            currentHandValue = handvalue;

            return Hand.GetHand(handvalue);
        }

        private int GetSum(int hv)
        {
            int sum = 0;
            for (int i = 0; i < 3; i++)
            {
                sum += history[hv][i];
            }
            return sum;
        }

        public void Study(bool win)
        {
            if (win)
            {
                history[prevHandValue][currentHandValue]++;
            }
            else
            {
                history[prevHandValue][(currentHandValue + 1) % 3]++;
                history[prevHandValue][(currentHandValue + 2) % 3]++;
            }
        }
    }

    public class Player
    {
        private string name;
        private Strategy strategy;
        private int wincount;
        private int losecount;
        private int gamecount;

        public Player(string name, Strategy strategy)
        {
            this.name = name;
            this.strategy = strategy;
        }

        public Hand NextHand()
        {
            return strategy.NextHand();
        }

        public void Win()
        {
            strategy.Study(true);
            wincount++;
            gamecount++;
        }

        public void Lose()
        {
            strategy.Study(false);
            losecount++;
            gamecount++;
        }

        public void Even()
        {
            gamecount++;
        }

        public override string ToString()
        {
            return $"[{name}:{gamecount} games, {wincount} win, {losecount} lose]";
        }
    }
}