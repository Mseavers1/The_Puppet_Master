using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Source.Story_Shop
{
    public class SignAnimator : MonoBehaviour
    {
        public GameObject[] signs;
        public float seconds, strength, speed;
        public int vibrate, randomness;

        private void Start()
        {
            foreach (var sign in signs)
            {
                sign.transform.DOShakePosition(seconds, strength, vibrate, randomness, false, false).SetLoops(-1).timeScale = speed;
            }
        }

        public void DeleteTween()
        {
            foreach (var sign in signs)
            {
                sign.transform.DOKill();
            }
        }
    }
}
