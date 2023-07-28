// Robert Tetreault (rrt2850@g.rit.edu)
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Messenger.Models;

namespace Messenger.Helpers{
    public static class ServerHelper{
        private static HttpClient client = new HttpClient();
        private const string baseAddress = "http://kayrun.cs.rit.edu:5000/";

        /// <summary>
        /// Gets a users public key from the server
        /// </summary>
        /// <param name="email">The email address of the user to get the key for</param>
        /// <returns>The public key of the user</returns>
        public static PublicKey GetKey(string email){
            return null;
        }

        /// <summary>
        /// Sends a user's public key to the server and sets the email address as a valid receiver.
        /// </summary>
        /// <param name="email">The email address of the user to send the key to.</param>
        /// <returns>A task representing the asynchronous operation with the response message or error.</returns>
        public static async Task<string> SendKey(string email)
        {
            try{
                PublicKey key = KeyHelper.Load<PublicKey>("public.key");    // Load the public key from the disk
                key.email = email;                                          // Set the email address of the public key
                string json = JsonSerializer.Serialize(key);                // Serialize the public key to JSON
                StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json"); // Format JSON for server
                HttpResponseMessage response = await client.PutAsync(baseAddress + $"Key/{email}", content);    // Send the key to the server

                if (response.IsSuccessStatusCode){
                    // Handle successful response (2xx)

                    Console.WriteLine($"Key sent to {email}");
                    // Read the response message as a string
                    string result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(result);
                    // Add the email address to the private key
                    PrivateKey privateKey = KeyHelper.Load<PrivateKey>("private.key");  // Load the private key from the disk
                    privateKey.email.Add(email);                                        // Add the email address to the private key 
                    KeyHelper.Save<PrivateKey>("private.key", privateKey);              // Save the private key to the disk

                    return result;
                }
                else{
                    // Handle non-successful response (4xx and 5xx)
                    Console.WriteLine($"Error sending key: {response.StatusCode} - {response.ReasonPhrase}");
                    return $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                }
            }
            catch (Exception ex){
                // Handle exceptions that may occur during the request
                Console.WriteLine($"Error sending key: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }

        /// <summary>
        /// Gets a users message from the server
        /// </summary>
        /// <param name="email">The email address of the user to get the message for</param>
        public static void GetMessage(string email){
            return;
        }

        /// <summary>
        /// Sends a message to a user through the server
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="email">The email address of the user to send the message to</param>
        public static void SendMessage(Message message, string email){
            return;
        }
    }
}