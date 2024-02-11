using System;
using UnityEngine;
using DG.Tweening;

namespace Source.Story_Shop
{
    public class CurtainCommands : MonoBehaviour
    {
        public void DropCurtains()
        {
            transform.DOMoveY(0, 10);
        }

        public void LiftCurtains()
        {
            transform.DOMoveY(1080, 10);
        }
    }
}
