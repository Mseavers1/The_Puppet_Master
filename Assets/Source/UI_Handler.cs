using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Handler : MonoBehaviour
{
    public GameObject openedObj, closedObj;

    public void Open()
    {
        openedObj.SetActive(true);
        closedObj.SetActive(false);
    }

    public void Close()
    {
        openedObj.SetActive(false);
        closedObj.SetActive(true);
    }
}
