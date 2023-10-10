using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chatbox_Handler : MonoBehaviour
{
    private GameObject chatbox, player, playerPoint, chatPoint;
    private float smoothFactor = 3f;
    private bool movePlayer = false;

    public void StartChat()
    {
        chatbox.transform.position = chatPoint.transform.position;
        chatbox.SetActive(true);

        movePlayer = true;
        player.GetComponent<Player_Movement>().EnableMovement(false);

        var reader = new Chatbox_Reader("Sample");
    }

    private void FixedUpdate()
    {
        if (movePlayer)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, playerPoint.transform.position, smoothFactor * Time.fixedDeltaTime);

            if (Vector3.Distance(player.transform.position, playerPoint.transform.position) <= .3f) movePlayer = false;

        }
    }

    private void Awake()
    {
        chatbox = transform.GetChild(2).gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        playerPoint = transform.GetChild(0).gameObject;
        chatPoint = transform.GetChild(1).gameObject;
    }
}
