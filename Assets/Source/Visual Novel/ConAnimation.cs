using TMPro;
using UnityEngine;

namespace Source.Visual_Novel
{
    public class ConAnimation : MonoBehaviour
    {
        [Range(0.001f, 1)]
        public float speed;
    
        private bool _animationOn;
        private int _currentIndex;
        private TMP_Text _text;
        private double _timer;

        public void AnimationOn()
        {
            _currentIndex = 0;
            _timer = 0;
            _animationOn = true;
        }

        public void AnimationOff()
        {
            _animationOn = false;
        }

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            _currentIndex = 0;
            _timer = 0;
        }

        private void FixedUpdate()
        {
            if (!_animationOn) return;

            _timer += Time.fixedDeltaTime;

            if (!(_timer >= speed)) return;
        
            var text = _text.text.ToLower();
            var newText = "";
            
            for (var i = 0; i < text.Length; i++)
            {
                var letter = text[i];
                
                if (i == _currentIndex) letter = char.ToUpper(letter);
                
                newText += letter;
            }

            _text.text = newText;
            _currentIndex = (_currentIndex + 1) % _text.text.Length;
            _timer = 0;
        }
    }
}
