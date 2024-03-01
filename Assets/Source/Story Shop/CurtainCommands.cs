using System;
using UnityEngine;
using DG.Tweening;

namespace Source.Story_Shop
{
    public class CurtainCommands : MonoBehaviour
    {
        public bool isAnimating;
        
        public void StartCurtainAnimation(float duration)
        {
            isAnimating = true;
            //transform.DOMoveY(-10, duration).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutBack).onComplete = Unlock;
            transform.DOMoveY(-10, duration).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine).onComplete = Unlock;
        }

        public void CloseCurtains(float duration)
        {
            transform.DOMoveY(-10, duration);
        }

        public void OpenCurtains(float duration)
        {
            transform.DOMoveY(1000, duration);
        }

        private void Unlock()
        {
            isAnimating = false;
        }
    }
}
