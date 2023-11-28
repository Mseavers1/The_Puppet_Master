using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card_Scaling : MonoBehaviour
{
    // NO LONGERS SCALES!! Now the card is just plop above the line to be visable
    private Image image;
    private Gamemanager_World gm;
    private bool isHovering = false;
    private const float visablilityLine = 12f, defaultLine = -186f;

    public void HoverCard(bool hover)
    {
        isHovering = hover;

        if (isHovering) image.rectTransform.position = new Vector2(image.rectTransform.position.x, visablilityLine);
    }

    private void Awake()
    {
        image = GetComponent<Image>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Gamemanager_World>();
        image.rectTransform.localScale = new Vector2(gm.scale, gm.scale);
    }

    private void Update()
    {
        if(!isHovering) image.rectTransform.position = new Vector2(image.rectTransform.position.x, defaultLine);


    }


}
