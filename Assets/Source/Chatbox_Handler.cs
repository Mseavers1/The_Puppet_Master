using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Chatbox_Handler : MonoBehaviour
{
    public TextAsset textInput;
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

        reader.Start();
    }

    public void ContinueChat()
    {
        reader.Play();
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
        
        var selectedChoicePath = choices["Awsome"]; // TODO - implement proper choices later

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
        reader = new Chatbox_Reader(AssetDatabase.GetAssetPath(textInput), text, this);
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
