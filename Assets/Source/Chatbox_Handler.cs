using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Chatbox_Handler : MonoBehaviour
{
    public string textInput;
    public bool playing;
    public bool canContinue { set; get; }

    private GameObject chatbox, player, playerPoint, chatPoint;
    private float smoothFactor = 3f;
    private bool movePlayer = false;
    private TMP_Text text;
    private Chatbox_Reader reader;

    public void StartChat()
    {
        canContinue = true;
        playing = true;

        chatbox.transform.position = chatPoint.transform.position;
        chatbox.SetActive(true);

        movePlayer = true;
        player.GetComponent<Player_Movement>().EnableMovement(false);

        //reader.Start();
        reader.Next();
    }

    public void QDebug(string msg)
    {
        Debug.Log(msg);
    }

    public void ContinueChat()
    {
        switch (reader.GetMode())
        {
            case 0:
                reader.Next();
                break;
            case 1:
                Display(reader.NextDialogue());
                break;
            default: break;
        }
        //reader.Play();
    }

    public void Display(string message)
    {
        text.text = message;
    }

    public void EndChat()
    {
        playing = false;

        chatbox.SetActive(false);
        player.GetComponent<Player_Movement>().EnableMovement();
    }

    public void ChangeSpeaker(string speaker)
    {

    }

    public void GenerateChoices(Dictionary<string, string> choices)
    {
        
        // Temporary!
        // Suppose to generate the proper choices
        // The following should be presented under each click depending what choice the user selects
        var selectedChoicePath = choices["Meh"];

        reader.SelectChoice(selectedChoicePath);
    }

    private void FixedUpdate()
    {
        if (movePlayer)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, playerPoint.transform.position, smoothFactor * Time.fixedDeltaTime);

            if (Vector3.Distance(player.transform.position, playerPoint.transform.position) <= .3f) movePlayer = false;

        }
    }

    private void Start()
    {
        //reader = new Chatbox_Reader(AssetDatabase.GetAssetPath(textInput), text, this);
        reader = new Chatbox_Reader("Assets/Resources/Texts/" + textInput + ".txt", this);
    }

    private void Awake()
    {
        chatbox = transform.GetChild(2).gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        playerPoint = transform.GetChild(0).gameObject;
        chatPoint = transform.GetChild(1).gameObject;
        text = chatbox.transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>();
    }
}
