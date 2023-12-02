using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Unity.VisualScripting;

public class Chatbox_Reader
{
    private LinkedList<string> instructions = new();
    private Stack<string> processed = new();
    private Queue<string> dialogue = new();

    private string currentLabel;
    private Chatbox_Handler handler;
    private byte mode; // 0 - Processing // 1 - Chat // 2 - Waiting for input

    public Chatbox_Reader(string path, Chatbox_Handler handler) 
    {
        this.handler = handler;
        Load(path);
        mode = 0;
    }

    // Processes the next instruction
    public void Next()
    {
        var instruction = NextInstruction();
        Command(instruction);
    }

    public string NextDialogue()
    {
        var message = dialogue.Dequeue();

        if (dialogue.Count == 0) { mode = 0; }

        return message;
    }

    public byte GetMode()
    {
        return mode;
    }

    public void SelectChoice(string label)
    {
        mode = 0;
        GoTo(label);
    }

    // Get the first line and remove it from the list
    private string NextInstruction()
    {
        var instruction = instructions.First.Value;
        instructions.RemoveFirst();
        processed.Push(instruction);
        return instruction;
    }

    private void Command(string instruction)
    {
        var command = instruction.Split(' ')[0].ToLower();

        // Check if its the ending command
        if (command == "end") 
        {
            handler.EndChat();
            return;
        }

        // Gets the parms from the commands
        var instruct = instruction.Substring(command.Length + 1);

        switch (command)
        {
            case "label":
                // Updates the current label
                UpdateLabel(instruct);
                Next();
                break;
            case "speaker":
                // Change the current speaker (effects) TODO
                Next();
                break;
            case "message":
                // How many of the next lines will be messages
                Message(int.Parse(instruct));
                break;
            case "goto":
                // Moves up or down the instructions to find the inserted label
                GoTo(instruct);
                currentLabel = instruct;
                Next();
                break;
            case "choices":
                // How many of the next lines will be avaliable choices for a sinle-line question
                Choices(int.Parse(instruct));
                break;
            case "if":
                // Conditional statements
                If(instruct);
                break;
            case "give":
                Give(instruct);
                Next();
                break;
            default: throw new Exception("Command is not defined: " + command + " in instruction " + instruction);
        }

    }

    private void Give(string parm)
    {
        var parms = parm.Split(' ');
        var itemName = parms[0].Replace("_", " ");
        var itemType = parms[1];

        handler.GenerateItem(itemName, itemType);
    }

    private void If(string parm)
    {
        var parms = parm.Split(' ');
        var numberOfConditions = MathF.Floor(parms.Length / 4);
        var values = new List<bool>();
        var symbols = new List<bool>();

        // For each conditional statement recieve it is true or false
        for ( var i = 0; i < numberOfConditions + 1; i++ )
        {
            var condition = parms[0 + (3 * i) + i];
            var isEqual = parms[1 + (3 * i) + i] == "=";
            var value = parms[2 + (3 * i) + i];

            if ( i != 0)
            {
                symbols.Add(parms[3 + (4 * (i-1))] == "&");
            }

            values.Add(ConditonalStatement(condition, isEqual, value));
        }

        // Check if only 1 conditional
        if (symbols.Count == 0)
        {
            if (values[0]) ConditionTrue(); else ConditionFalse();
            return;
        }

        // Compute the statement
        var answer = values[0];
        var index = 1;

        foreach ( var symbol in symbols )
        {
            if (symbol)
            {
                answer &= values[index];
            } 
            else
            {
                answer |= values[index];
            }

            index++;
        }

        // Check if the answer is true or not
        if (answer)
        {
            ConditionTrue();
            return;
        }

        ConditionFalse();
    }

    private bool ConditonalStatement(string condition, bool isEqual, string value)
    {
        // Find the condition statement
        switch (condition)
        {
            case "name":
                return NameCondition(isEqual, value);
            case "skill":
                return SkillCondition(isEqual, value);
            default: throw new Exception("The condition in conditional statement (" + condition + ") is not valid!");
        }
    }

    private bool SkillCondition(bool isEqual, string value)
    {
        //UnityEngine.Debug.Log(value);
        var hasSkill = HoldingOfSkills.ContainSkill(value);
        // UnityEngine.Debug.Log("Has? " + hasSkill);

        return hasSkill;

        if (isEqual)
        {
            if (hasSkill) return true;

            return false;
        }

        return false;
    }

    private bool NameCondition(bool isEqual, string value)
    {
        var name = UnityEngine.GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Movement>().playerName;
        if (isEqual)
        {
            if (name == value)
            {
                return true;
            } 

            return false;
        }

        if (name != value)
        {
            return true;
        }

        return false;
    }

    private void ConditionTrue()
    {
        Next();
    }

    private void ConditionFalse()
    {
        NextInstruction();
        Next();
    }

    // Prints question and waits for the user to respond based on the available choices
    private void Choices(int lines)
    {
        var choices = new Dictionary<string, string>();

        // Goes through the next X lines to generate the choices
        for(int i = 0; i < lines; i++)
        {
            var line = NextInstruction();
            var label = line.Split(' ')[0];
            var message = line.Substring(label.Length + 1);

            choices.Add(message, label);
        }

        // Grabs the nextl line which is used for the displayed message
        UploadToDialogue(NextInstruction());
        handler.Display(NextDialogue());

        // Changes the mode to waiting so the player can not move onto the next instruction
        mode = 2;
        handler.GenerateChoices(choices);

    }

    // Checks if the label is ahead or has already past
    private void GoTo(string label)
    {
        if (label[0] > currentLabel[0])
        {
            // Check forward
            Forward(label);
            return;
        }

        // Check backward
        Reverse(label);
    }

    // Reverts instructions until it reaches the inserted label
    private void Reverse(string label)
    {
        while (processed.Peek().ToLower() != "label " + label)
        {
            instructions.AddFirst(processed.Pop());
        }
    }

    // Continues through the instructions until it reaches the inserted label
    private void Forward(string label)
    {
        while(instructions.First.Value != "label " + label)
        {
            NextInstruction();
        }
    }

    private void UploadToDialogue(string message)
    {
        dialogue.Enqueue(message);
    }

    private void Message(int numLines)
    {
        // Process the next number of lines and upload into dialogue
        for (int i = 0; i < numLines; i++)
        {
            var line = NextInstruction();
            UploadToDialogue(line);
        }

        mode = 1;
        handler.Display(NextDialogue());
    }

    // Updates the current label
    private void UpdateLabel(string label)
    {
        currentLabel = label;
    }

    // Loads all instructions from a file into the list
    private void Load(string path)
    {
        try
        {
            var reader = new StreamReader(path);
            var line = reader.ReadLine();

            while (line != null)
            {
                // Ignore line if blank or comment
                if (line.Length == 0 || line[0] == '/') 
                {
                    line = reader.ReadLine();
                    continue;
                }

                // Add the line into the instructions
                instructions.AddLast(line);
                line = reader.ReadLine();
            }
        }
        catch (Exception e) 
        {
            throw e;
        }
    }
}
