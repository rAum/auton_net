using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisionUtils
{
    /// <summary>
    /// This class can in O(1) produce sequence of random shuffled numbers.
    /// Based on code from "Game Coding Complete" by Mr Mike.
    /// </summary>
    public class TakeRandom
    {
        static readonly UInt32[] primes = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43,
                                          59, 67, 79, 89, 97, 103, 127, 151, 167, 457, 587, 619, 1019, 
                                          1993, 1471, 2437, 3011, 3571 }; // this should be enough... add more if needed
        UInt32 skip, prime, visited, max, currPos;

        public TakeRandom(UInt32 max_) : this(max_, FindPrime(max_))
        {
        }

        public UInt32 Prime { get { return prime; } }
        public UInt32 Visited { get { return visited; } }
        public UInt32 Count { get { return max; } }
        public UInt32 Current { get { return currPos; } }

        /// <summary>
        /// Construct TakeRandom object
        /// </summary>
        /// <param name="max_">integers to shuffle</param>
        /// <param name="prime_">the first biggest prime than count_</param>
        public TakeRandom(UInt32 max_, UInt32 prime_)
        {
            max = max_;
            prime = prime_;

            Shuffle();
        }

        public IEnumerable<UInt32> Get()
        {
            if (visited >= max) Shuffle();

            do
            {
                currPos = (currPos + skip) % prime;
            } while (currPos > max && currPos < prime);

            ++visited;
            yield return currPos;
        }

        public void Shuffle()
        {
            currPos = 0;
            visited = 0;

            Random rnd = new Random();
            UInt32 a = (UInt32)rnd.Next(1, 23);
            UInt32 b = (UInt32)rnd.Next(1, 17);
            UInt32 c = (UInt32)rnd.Next(1, 5);

            skip = a * (max * max) + b * max + c;
            skip &= ~0xc0000000; // to ensure that skip is not too big

            UInt32 test = skip % prime;
            if (test == 0)
                ++skip;
        }


        private static UInt32 FindPrime(UInt32 num)
        {
            #if DEBUG
            if (num > primes.Last())
                throw new Exception("TakeRandom does not support this count, it's bigger than the biggest prime in list.");
            if (num <= 2)
                throw new Exception("TakeRandom does not support count less than 2.");
            #endif
            int i = primes.Length -1;
            UInt32 p = primes[i];
            while (primes[--i] > num) p = primes[i]; // stop condition assert: count > 2 [constructor providing]  
            return p;
        }
    }
}
