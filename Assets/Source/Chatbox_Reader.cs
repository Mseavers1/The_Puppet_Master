using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEditor.Search;

public class Chatbox_Reader
{
    private StreamReader reader;
    private TMP_Text text;
    private Chatbox_Handler handler;
    private bool repeatMode;

    public Queue<string> savedLines = new ();

    public Chatbox_Reader(string path, TMP_Text text, Chatbox_Handler handler) 
    { 
        reader = new StreamReader(path);
        this.text = text;
        this.handler = handler;
        repeatMode = false;
    }

    public void NextLine()
    {
        var line = reader.ReadLine();

        // Check to see if the line is a command
        if (line[0] == '*')
        {
            Command(line.Substring(2, line.Length - 2));
            return;
        }

        Chat(line);

    }

    public bool IsRepeating() { return repeatMode; }

    private void Chat(string line)
    {
        if (repeatMode)
        {
            savedLines.Enqueue(line);
        }

        text.text = line;
    }

    private void Command(string command)
    {
        switch (command)
        {
            case "END": // Ends the chatbox
                handler.EndChat();

                if (repeatMode)
                    savedLines.Enqueue("END");

                break;
            case "REPEAT":
                repeatMode = true;
                Chat(reader.ReadLine());
                break;
            default: // Default command is the name of a NPC
                // TODO - Highlight the character who is speaking and un-highlight characters not talking
                Chat(reader.ReadLine());
                break;
        }
    }
}
