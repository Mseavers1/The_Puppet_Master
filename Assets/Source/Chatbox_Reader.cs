using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine.UIElements;

public class Chatbox_Reader
{
    private StreamReader reader;
    private TMP_Text text;
    private Chatbox_Handler handler;
    private int mode = 0; // 0 - NORMAL, 1 - REPEAT

    private Queue<string> dialogue = new();
    private Queue<string> repeat = new();

    public Chatbox_Reader(string path, TMP_Text text, Chatbox_Handler handler) 
    { 
        reader = new StreamReader(path);
        this.text = text;
        this.handler = handler;
    }

    public void Start()
    {
        if (mode == 1)
        {
            dialogue = CopyQueue(repeat);
            Play();
            return;
        }

        var line = reader.ReadLine();

        // Detects if it is not a command
        if (line[0] != '*')
        {
            throw new Exception("Script is broken! It needs to start with a COMMAND. This is invalid: " + line);
        }

        Command(line.Substring(2));
    }

    public void Play()
    {
        // No more to display
        if (dialogue.Count == 0)
        {
            handler.EndChat();
            return;
        }

        text.text = dialogue.Dequeue();
    }
    
    private void Debug(string message)
    {
        UnityEngine.Debug.Log(message);
    }

    private void Command(string command) 
    {
        var parsed = command.Split(' ');
        switch (parsed[0])
        {
            case "CHAT":
                Chat(command.Substring(parsed[0].Length + 1));
                break;
            case "REPEAT":
                Repeat(command.Substring(parsed[0].Length + 1));
                break;
            default: throw new Exception("The command inserted: " + parsed[0] + " is not a valid command!");
        }
    }

    // Whenever player clicks on NPC, the same message will display forever
    private void Repeat(string speaker)
    {
        mode = 1;

        handler.ChangeSpeaker(speaker); // TODO - implement this method

        var dialogue = new Queue<string>();
        var currentLine = reader.ReadLine();

        while (currentLine != "* END REPEAT")
        {
            dialogue.Enqueue(currentLine);
            currentLine = reader.ReadLine();
        }

        this.dialogue = dialogue;
        repeat = CopyQueue(dialogue);

        Play();
    }

    private Queue<string> CopyQueue(Queue<string> copy)
    {
        var newQ = new Queue<string>();
        var array = copy.ToArray();

        foreach (var item in array)
        {
            newQ.Enqueue(item);
        }

        return newQ;
    }

    // Begins chat among the NPC and player
    private void Chat(string speaker)
    {
        handler.ChangeSpeaker(speaker); // TODO - implement this method

        var dialogue = new Queue<string>();
        var currentLine = reader.ReadLine();

        while (currentLine != "* END CHAT")
        {
            dialogue.Enqueue(currentLine);
            currentLine = reader.ReadLine();
        }

        this.dialogue = dialogue;

        Play();
    }

    
}
