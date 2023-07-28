// Robert Tetreault (rrt2850@g.rit.edu)

/*******************************************************************************************
* File: Program.cs
* -----------------------------------------------------------------------------------------
* This file contains the main entry point for the application and some miscellaneous stuff
* TODO - rewrite this description when the program is finished and I know what
* "miscellaneous stuff" I added
********************************************************************************************/
using Messenger.Models;
using Messenger.Helpers;
using System.IO;
using System.Text;
using System;
using System.Security.Cryptography;
using System.Numerics;

class Program{
    
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    static async Task Main(string[] args){
        //KeyHelper.GenerateKeys(1024);
        //Console.WriteLine("Keys generated");

        //await ServerHelper.SendKey("rrt2850@g.rit.edu");
        //await ServerHelper.GetKey("rrt2850@g.rit.edu");
        //await ServerHelper.SendMessage("rrt2850@g.rit.edu", "lets goooooo");
        await ServerHelper.GetMessage("rrt2850@g.rit.edu");
    }
}
