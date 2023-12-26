using System;
using Source.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source.Menus
{
    public class WarningAuto : MonoBehaviour
    {
        public TextFade[] fades;
        private const float DelayScene = 5, DelayFade = 5;

        private void Start()
        {
            Invoke(nameof(StartFadeOut), DelayFade);
        }

        private void StartFadeOut()
        {
            foreach (var fade in fades)
            {
                fade.isFadeIn = true;
                fade.isFadingOn = true;
                fade.fadeTimeInSeconds = 0;
                fade.fadingSpeed = 0.005f;
                fade.RestartFade();
            }
            
            Invoke(nameof(ChangeScene), DelayScene);
        }
        
        private void ChangeScene()
        {
            SceneManager.LoadScene(1);
        }
    }
}
