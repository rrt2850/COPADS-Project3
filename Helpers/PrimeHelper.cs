// Robert Tetreault (rrt2850)

/*******************************************************************************************
* File: PrimeHelper.cs
* -----------------------------------------------------------------------------------------
* This file contains the PrimeHelper class, which contains functions from the previous
* project that generate prime numbers and help with prime number related tasks.
*
* Note: in the original project there were multiple files but I've condensed them all into
*       this one file to save space since it's not the main point of this project.
********************************************************************************************/

using System.Numerics;
using System.Security.Cryptography;

namespace Messenger.Helpers{
    /// <summary>
    /// The Extensions class contains extension methods for the BigInteger class
    /// </summary>
    public static class Extensions{
        /// <summary>
        /// Checks if a BigInteger is probably prime
        /// </summary>
        /// <param name="value">The BigInteger to check</param>
        /// <param name="k">The number of times to check if the BigInteger is prime</param>
        /// <returns>True if the BigInteger is probably prime, false if it is not</returns>
        public static bool IsProbablyPrime(this BigInteger value, int k = 10){
            RandomNumberGenerator rng = RandomNumberGenerator.Create();

            if (value == 2 || value == 3) return true;          // 2 and 3 are prime
            if (value <= 1 || value % 2 == 0) return false;     // negatives, 0 and 1 are not prime, even numbers are not prime

            // write (value - 1) as 2^r * d
            // keep halving value - 1 while it is even (and use counter r to count how many times it's halved)
            BigInteger d = value - 1;   
            int r = 0;
            while (d % 2 == 0){
                d /= 2;
                r++;
            }

            BigInteger a;

            // witness loop
            for (int i = 0; i < k; i++){
                a = Extensions.GenerateRandom(2, value - 2, rng); // ensure the range [2, value - 2]

                BigInteger x = BigInteger.ModPow(a, d, value); // compute a^d % value

                if (x == 1 || x == value - 1) continue;

                // repeat r - 1 times
                for (int j = 1; j < r; j++){
                    x = BigInteger.ModPow(x, 2, value); // compute x^2 % value
                    if (x == 1) return false;       // if x is 1, then value is not prime
                    if (x == value - 1) break;      // if x is value - 1, then continue witness loop
                }
                if (x != value - 1) return false;   // if x is not value - 1, then value is not prime
            }

            //  value is probably prime if it passed all the tests
            return true;
        }


        /// <summary>
        /// Generates a random BigInteger in the range [minValue, maxValue)
        /// </summary>
        /// <param name="minValue">The lower bound of the random number returned</param>
        /// <param name="maxValue">The upper bound of the random number returned</param>
        /// <param name="rng">The RandomNumberGenerator to use</param>
        /// <returns>A random BigInteger in the range [minValue, maxValue)</returns>
        public static BigInteger GenerateRandom(BigInteger minValue, BigInteger maxValue, RandomNumberGenerator rng){
            if (maxValue - minValue < 2){
                throw new ArgumentException("Invalid range for random BigInteger generation");
            }

            BigInteger difference = maxValue - minValue;                //  Calculate the difference between the bounds
            byte[] differenceBytes = difference.ToByteArray();          //  Get the byte representation of the difference

            //  Generate a random BigInteger that is within the range of [0, difference)
            rng.GetBytes(differenceBytes);                     
            BigInteger randomNumber = new BigInteger(differenceBytes);

            // Make sure randomNumber is not negative and less than difference
            randomNumber = BigInteger.Abs(randomNumber) % difference; 

            return minValue + randomNumber;
        }

    }

    /// <summary>
    /// The PrimeGenerator class is used to generate prime numbers
    /// </summary>
    public class PrimeGenerator{
        //  Initialize a new RandomNumberGenerator
        private RandomNumberGenerator rng = RandomNumberGenerator.Create();
        
        /// <summary>
        /// Generates a prime number with the specified number of bits
        /// </summary>
        /// <param name="bits">The number of bits the prime should be</param>
        /// <returns>A BigInteger prime number with the specified number of bits</returns>
        public BigInteger GeneratePrime(int bits){
            BigInteger value;

            value = GenerateRandom(bits);

            // Ensure value is odd
            if (value % 2 == 0) value++;

            // Keep incrementing value by 2 until it is probably prime
            while (!value.IsProbablyPrime(10))  value += 2;

            return value;
        }

        /// <summary>
        /// Generates a random BigInteger with the specified number of bits
        /// </summary>
        /// <param name="bits">The number of bits the BigInteger should be</param>
        /// <returns>A random odd BigInteger with the specified number of bits</returns>
        public BigInteger GenerateRandom(int bits){
            int bytes = bits / 8;                   //  Calculate the number of bytes needed to hold the bits
            byte[] buffer = new byte[bytes + 1];    //  Initialize a new byte array to hold the random bytes, plus one to ensure positive BigInteger
            rng.GetBytes(buffer);                   //  Fill the byte array with random bytes

            buffer[bytes] = 0; // This ensures that the BigInteger will always be positive

            //  Make sure the byte array is large enough by ORing the last byte with 0x80
            //  to make sure the most significant bit is 1
            buffer[bytes - 1] |= 0x80;   //   makes sure the new BigInteger has at least the specified number of bits

            BigInteger newPrime = new BigInteger(buffer);   //  Make a new BigInteger from the byte array

            //  make the BigInteger odd if it isn't already (all prime numbers are odd except 2)
            if (newPrime.IsEven) newPrime++;
        
            return newPrime;
        }
    }
}

    