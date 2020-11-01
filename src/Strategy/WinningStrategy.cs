﻿using System;

namespace Strategy
{
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
}
