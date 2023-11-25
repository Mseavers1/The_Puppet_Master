using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card_Scaling : MonoBehaviour
{
    private Image image;
    private Gamemanager_World gm;
    private bool isHovering = false;

    public void HoverCard(bool hover)
    {
        isHovering = hover;

        if (isHovering) image.rectTransform.localScale = new Vector2(gm.scale * 1.2f, gm.scale * 1.2f);
    }

    private void Awake()
    {
        image = GetComponent<Image>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Gamemanager_World>();
    }

    private void Update()
    {
        if(!isHovering) image.rectTransform.localScale = new Vector3(gm.scale, gm.scale, 1);
    }


}
