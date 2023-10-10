using System;
using System.Diagnostics;
using System.IO;

public class Chatbox_Reader
{
    public Chatbox_Reader(string fileName) 
    { 
        var reader = new StreamReader("./Texts/" + fileName);
        string line = reader.ReadLine();
        UnityEngine.Debug.Log(line);
    }
}
