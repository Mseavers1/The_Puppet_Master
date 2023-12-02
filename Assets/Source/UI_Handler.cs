using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Handler : MonoBehaviour
{
    public GameObject openedObj, closedObj;

    public void Open()
    {
        if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<Gamemanager_World>().IsTutorialOn()) return;

        openedObj.SetActive(true);
        closedObj.SetActive(false);
    }

    public void Close()
    {
        openedObj.SetActive(false);
        closedObj.SetActive(true);
    }
}
