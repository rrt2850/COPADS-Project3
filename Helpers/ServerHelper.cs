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
        /// <returns>The public key of the user on success</returns>
        public static async Task<string> GetKey(string email){
            try{
                HttpResponseMessage response = await client.GetAsync(baseAddress + $"Key/{email}"); // Get the key from the server

                if (response.IsSuccessStatusCode){
                    // Handle successful response (2xx)
                    
                    string result = await response.Content.ReadAsStringAsync();         // Read the response message as a string
                    PublicKey? key = JsonSerializer.Deserialize<PublicKey>(result);     // Deserialize the key from JSON

                    // Check if the key was successfully deserialized
                    if (key == null){
                        throw new Exception("Failed to deserialize public key.");
                    }

                    // Save the public key to a file
                    string filename = email + ".key";
                    KeyHelper.Save<PublicKey>(filename, key);

                    return key.key;
                }
                else{
                    // Handle non-successful response (4xx and 5xx)
                    return $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                }
            }
            catch (Exception ex){
                // Handle exceptions that occur uring the request
                Console.WriteLine($"Error getting key: {ex.Message}");
                return $"Error: {ex.Message}";
            }
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
        public static async Task<string> GetMessage(string email){
            try{
                // Get the message from the server
                HttpResponseMessage response = await client.GetAsync(baseAddress + $"Message/{email}");

                if (response.IsSuccessStatusCode){   
                    // Handle successful response (2xx)

                    string result = await response.Content.ReadAsStringAsync();

                    // Deserialize the message JSON to an object
                    Message? message = JsonSerializer.Deserialize<Message>(result);

                    if (message == null){
                        throw new Exception("Failed to deserialize message.");
                    }
                    
                    // Attempt to load the private key from the disk
                    string filename = "private.key";
                    PrivateKey? privateKey = KeyHelper.Load<PrivateKey>(filename);

                    if (privateKey == null){
                        return "Message can't be decoded, private key not found.";
                    }

                    if (!privateKey.email.Contains(email)){
                        return "Message can't be decoded, private key not found.";
                    }

                    // Assuming that the 'Decode' function exists that takes encoded message and private key, 
                    // and returns the decoded message
                    string? decodedMessage = MessageHelper.DecryptMessage(message.content, privateKey);
                    
                    if (decodedMessage == null){
                        throw new Exception("Failed to decrypt message.");
                    }

                    Console.WriteLine($"Decoded message: {decodedMessage}");
                    return $"Decoded message: {decodedMessage}";
                }
                // Handle non-successful response (4xx and 5xx)
                return $"Error: {response.StatusCode} - {response.ReasonPhrase}";
            }
            catch (Exception ex){
                // Handle exceptions that may occur during the request
                Console.WriteLine($"Error getting message: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }

        /// <summary>
        /// Sends a message to a user through the server
        /// </summary>
        /// <param name="email">The email address of the user to send the message to</param>    
        /// <param name="plaintext">The message to send</param>
        public static async Task<string> SendMessage(string email, string plaintext){
            try{
                string filename = email + ".key";

                // Load the reciever's public key
                if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), filename))){
                    return $"Error: No public key found for {email}. Download the key first.";
                }
                
                // Since the key exists, load it
                PublicKey recieverKey = KeyHelper.Load<PublicKey>(filename);

                // Encrypt the plaintext message with the reciever's public key and base64 encode it
                string? encryptedMessage = MessageHelper.EncryptMessage(recieverKey, plaintext);
                if (encryptedMessage == null){
                    throw new Exception("Failed to encrypt message.");
                }

                // Create the JSON body of the request
                var message = new { email = email, content = encryptedMessage };
                string json = JsonSerializer.Serialize(message);
                StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                // Send the encrypted message to the server
                HttpResponseMessage response = await client.PutAsync(baseAddress + $"Message/{email}", content);

                if (response.IsSuccessStatusCode)
                {   
                    Console.WriteLine($"hell yeah");
                    // Handle successful response (2xx)
                    return "Message sent successfully.";
                }
                else
                {
                    // Handle non-successful response (4xx and 5xx)
                    throw new Exception($"Error sending message: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions that may occur during the request
                Console.WriteLine($"Error sending message: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }

    }
}