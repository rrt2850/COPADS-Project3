// Robert Tetreault (rrt2850@g.rit.edu)

/********************************************************************************************
* File: KeyHelper.cs
* -------------------------------------------------------------------------------------------
* All of the functions that deal with keys are in this file. This includes generating keys,
* loading keys from the disk, saving keys to the disk, encoding keys, decoding keys, and
* converting keys to byte arrays.
*********************************************************************************************/

using System.Numerics;

using System.Text.Json;
using System.Security.Cryptography;
using Messenger.Models;

namespace Messenger.Helpers{
    public static class KeyHelper{
        private static int E = 65537;  // The public key exponent

        /// <summary>
        /// Formats a key into a string
        /// </summary>
        /// <param name="exponent">The exponent of the key</param>
        /// <param name="n">The n value of the key</param>
        /// <returns>The key formatted as a Base64 string</returns>
        public static string ConstructKey(BigInteger exponent, BigInteger n){
            try
            {
                // Convert exponent and n to byte arrays (in little endian)
                byte[] exponentBytes = exponent.ToByteArray();
                byte[] nBytes = n.ToByteArray();

                // Get lengths of exponent and n in big endian
                byte[] exponentLength = BitConverter.GetBytes(exponentBytes.Length);
                byte[] nLength = BitConverter.GetBytes(nBytes.Length);
                
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(exponentLength);
                    Array.Reverse(nLength);
                }

                // Concatenate all the byte arrays together
                byte[] keyBytes = exponentLength.Concat(exponentBytes).Concat(nLength).Concat(nBytes).ToArray();

                // Convert the byte array to a base64 string
                string key = Convert.ToBase64String(keyBytes);

                return key;
            }
            catch (Exception ex)
            {
                // Handle exception, possibly by logging and rethrowing or returning a default value
                Console.Error.WriteLine($"An error occurred in ConstructKey: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Deconstructs a key into its exponent and n values
        /// </summary>
        /// <param name="key">The key to use</param>
        /// <note>
        /// Right now this just prints the values, but later it'll return them as a tuple.
        /// I'm not using a tuple right now because I'm only using this function for testing
        /// currently.
        /// </note>
        public static void DeconstructKey(string key){
            try
            {
                byte[] decodedKey = Convert.FromBase64String(key);

                // Get e from the first 4 bytes of the key
                byte[] eBytes = new byte[4];
                Array.Copy(decodedKey,0, eBytes, 0, 4);
                if(BitConverter.IsLittleEndian){
                    Array.Reverse(eBytes);
                }

                int e = BitConverter.ToInt32(eBytes);
                Console.WriteLine("e: " + e);

                // Get E using 'e' as the number of bytes to read
                byte[] EBytes = new byte[e];
                Array.Copy(decodedKey, 4, EBytes, 0, e);

                BigInteger E = new BigInteger(EBytes);
                Console.WriteLine("E: " + E);

                // get n skipping the first 4 + e bytes and reading the next 4 bytes
                byte[] nBytes = new byte[4];
                Array.Copy(decodedKey, 4 + e, nBytes, 0, 4);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(nBytes);
                }
                int n = BitConverter.ToInt32(nBytes);

                
                Console.WriteLine("n: " + n);

                // get N skipping the first 4 + e + 4 bytes and reading the next n bytes
                byte[] NBytes = new byte[n];
                Array.Copy(decodedKey, 4 + e + 4, NBytes, 0, n);    // Note, I could have just said e + 8 but it clouds up how the bytes are partitioned
                BigInteger N = new BigInteger(NBytes);

                Console.WriteLine("N: " + N);
            }
            catch (Exception ex)
            {
                // Handle exception, possibly by logging and rethrowing or returning a default value
                Console.Error.WriteLine($"An error occurred in DeconstructKey: {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// Generates a RSA public and private key
        /// </summary>
        /// <param name="keySize">The size of the key to generate</param>
        /// <returns>A tuple containing the public and private keys</returns>
        public static void GenerateKeys(int keySize){
            try
            {
                // Ensure the key size is positive
                if(keySize <= 0){
                    throw new ArgumentException("Key size must be positive.", nameof(keySize));
                }

                // Initialize a random number generator
                Random rand = new Random();                         

                // Initialize a new prime number generator
                PrimeGenerator generator = new PrimeGenerator();    

                // Get a random percentage between 0.2 and 0.3
                double percentOffset = rand.NextDouble() * 0.1 + 0.2;   

                // Get a random sign (1 or -1) so the offset is +- 20-30%
                int result = rand.Next(2) == 1 ? 1 : -1;                

                // Apply the sign to the offset
                percentOffset *= result;                                

                // Generate p and q (two prime numbers that add up to the key size in bits)
                int bitLengthP = keySize / 2 + (int)Math.Floor(keySize * percentOffset);
                int bitLengthQ = keySize - bitLengthP;

                // Generate p and q as prime numbers with the specified bit lengths
                BigInteger p = generator.GeneratePrime(bitLengthP);
                BigInteger q = generator.GeneratePrime(bitLengthQ);

                // N = p * q
                BigInteger N =  p * q;              

                // r = (p - 1) * (q - 1)
                BigInteger r = (p - 1) * (q - 1);   

                // D = modInverse(E, r) Note: D is the private key exponent
                BigInteger D = modInverse(E, r);    

                // E is already set outside of this function

                // Create the public and private keys
                string publicKeyString = ConstructKey(E, N);
                string privateKeyString = ConstructKey(D, N);

                // Create PublicKey and PrivateKey objects
                PublicKey publicKey = new PublicKey(key : publicKeyString);
                PrivateKey privateKey = new PrivateKey(key : privateKeyString);

                // Save the keys to the disk
                Save<PublicKey>("public.key", publicKey);
                Save<PrivateKey>("private.key", privateKey);
            }
            catch (Exception ex)
            {
                // Handle any errors that occurred during key generation
                Console.Error.WriteLine($"An error occurred in GenerateKeys: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Loads a key from the disk
        /// </summary>
        /// <param name="filename">The name of the file to load the key from</param>
        /// <returns>The key</returns>
        public static T Load<T>(string filename) where T : class
        {
            try{
                string keyJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), filename));
                T? key = JsonSerializer.Deserialize<T>(keyJson);
                if (key == null) {
                    throw new Exception($"Failed to deserialize {typeof(T).Name}.");
                }
                return key;
            }
            catch (IOException e){
                throw new FileNotFoundException($"Failed to load {typeof(T).Name} file: {filename}", e);
            }
            catch (JsonException e){
                throw new InvalidDataException($"Failed to deserialize {typeof(T).Name} from file: {filename}", e);
            }
        }


        /// <summary>
        /// Saves a key to the disk
        /// </summary>
        /// <param name="filename">The name of the file to save the key to</param>
        /// <param name="key">The key to save</param>
        public static void Save<T>(string filename, T key) where T : class
        {
            try{
                string keyJson = JsonSerializer.Serialize(key);
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), filename), keyJson);
            }
            catch (JsonException e){
                throw new Exception($"Failed to serialize the key: {key}", e);
            }
            catch (IOException e){
                throw new Exception($"Failed to write key to file: {filename}", e);
            }
        }


        /// <summary>
        /// Converts a key into a byte array
        /// </summary>
        /// <param name="key">The key to convert</param>
        /// <returns>The key as a byte array</returns>
        public static byte[] ToByteArray(string key){
            return null;
        }

        /// <summary>
        /// A modInverse function given to us in the rubric
        /// </summary>
        /// <param name="a">The first number</param>
        /// <param name="n">The second number</param>
        /// <returns>The modInverse of the two numbers</returns>
        static BigInteger modInverse(BigInteger a, BigInteger n){
            try
            {
                if(a <= 0 || n <= 0){
                    throw new ArgumentException("Input values must be positive.");
                }

                BigInteger i = n, v = 0, d = 1;
                while (a>0) {
                    BigInteger t = i/a, x = a;
                    a = i % x;
                    i = x;
                    x = d;
                    d = v - t*x;
                    v = x;
                }
                v %= n;
                if (v<0) v = (v+n)%n;
                return v;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred in modInverse: {ex.Message}");
                throw;
            }
        }
    }
}
