// Robert Tetreault (rrt2850@g.rit.edu)
using Messenger.Models;

namespace Messenger.Helpers{
    public static class ServerHelper{
        /// <summary>
        /// Gets a users public key from the server
        /// </summary>
        /// <param name="email">The email address of the user to get the key for</param>
        /// <returns>The public key of the user</returns>
        public static Key GetKey(string email){
            return null;
        }

        /// <summary>
        /// Sends a users public key to the server
        /// </summary>
        /// <param name="key">The key to send</param>
        /// <param name="email">The email address of the user to send the key to</param>
        public static void SendKey(Key key, string email){
            return;
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