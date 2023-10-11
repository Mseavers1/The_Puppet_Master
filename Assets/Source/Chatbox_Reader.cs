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
    private TMP_Text text;
    private Chatbox_Handler handler;
    private int mode = 0; // 0 - NORMAL, 1 - REPEAT

    private LinkedList<string> current;
    private Stack<string> processed;

    private Queue<string> dialogue = new();

    public Chatbox_Reader(string path, TMP_Text text, Chatbox_Handler handler) 
    { 
        this.text = text;
        this.handler = handler;

        current = Load(path);
        processed = new Stack<string>();
    }

    public void Start()
    {
        if (mode == 1)
        {
            Return();
        }

        var line = Next();

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

    private void Debug(Stack<string> s)
    {
        while(s.Count > 0)
            Debug(s.Pop());
    }

    // Go back if command was REPEAT
    private void Return()
    {
        mode = 0;
        // Undo previous lines until it gets to the return statement
        while(processed.Peek().Length < 7 || processed.Peek().Substring(0, 8) != "* REPEAT")
        {
            current.AddFirst(processed.Pop());
        }
        
        // Makes sure to have the return statement added back into the list
        current.AddFirst(processed.Pop());
    }
    
    // Go back based on a label
    private void Return(string label)
    {

    }

    // Go futher in label is ahead
    private void Ahead(string label)
    {

    }

    private string Next()
    {
        var line = current.First.Value;
        current.RemoveFirst();

        processed.Push(line);
        return line;
    }

    private LinkedList<string> Load(string path)
    {
        LinkedList<string> loaded = new();
        
        var reader = new StreamReader(path);
        var line = reader.ReadLine();

        while (line != null)
        {
            loaded.AddLast(line);
            line = reader.ReadLine();
        }
       

        return loaded;
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
        var currentLine = Next();

        while (currentLine != "* END REPEAT")
        {
            dialogue.Enqueue(currentLine);
            currentLine = Next();
        }

        this.dialogue = dialogue;

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
        var currentLine = Next();

        while (currentLine != "* END CHAT")
        {
            dialogue.Enqueue(currentLine);
            currentLine = Next();
        }

        this.dialogue = dialogue;

        Play();
    }

    
}
