using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Source.Utility
{
    public class TextFade : MonoBehaviour
    {
        public bool isFadeIn, isFadingOn;
        [Range(0, 0.2f)] public float fadingSpeed;
        public float fadeTimeInSeconds;

        private float _timer, _alphaTarget;
        private bool _isFadeComplete, _startFading;
        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            RestartFade();
        }

        public void RestartFade()
        {
            _text.alpha = isFadeIn ? 1 : 0;
            _alphaTarget = isFadeIn ? 0 : 1;
            _timer = 0;
            _startFading = false;
            _isFadeComplete = false;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) RestartFade();
        }

        private void FixedUpdate()
        {
            if (isFadingOn && !_isFadeComplete) Timer();
            
            if (_startFading) Fade();
        }

        private void Timer()
        {
            _timer += Time.fixedDeltaTime;

            if (_timer < fadeTimeInSeconds) return;
            
            // Occurs when timer hits the completion time
            _startFading = true;
            isFadingOn = false;
            _isFadeComplete = true;
            _timer = 0;
        }

        private void Fade()
        {
            if (isFadeIn) FadeIn(); else FadeOut();

            if (Math.Abs(_text.alpha - _alphaTarget) >= 0.00001f) return;
            
            _startFading = false;
            _text.alpha = isFadeIn ? 0 : 1;
        }

        private void FadeIn()
        {
            _text.alpha = Mathf.Clamp(_text.alpha - fadingSpeed, 0, 255);
        }

        private void FadeOut()
        {
            _text.alpha = Mathf.Clamp(_text.alpha + fadingSpeed, 0, 255);
        }
    }
}
