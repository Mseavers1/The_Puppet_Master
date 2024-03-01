using UnityEngine;
using UnityEngine.Serialization;

namespace Source.Game
{
    /*
     * By Nebula Coding
     * Edited and Tweaks for project -- Michael Seavers
     * https://www.youtube.com/watch?v=nADIYwgKHv4&t=259s
     */
    public class MapSpriteSelector : MonoBehaviour
    {
        public Sprite 	spU, spD, spR, spL;

        [FormerlySerializedAs("spUD")] public Sprite 	spUd;

        [FormerlySerializedAs("spRL")] public Sprite 	spRl;

        [FormerlySerializedAs("spUR")] public Sprite 	spUr;

        [FormerlySerializedAs("spUL")] public Sprite 	spUl;

        [FormerlySerializedAs("spDR")] public Sprite 	spDr;

        public Sprite 	spDL;

        [FormerlySerializedAs("spULD")] public Sprite 	spUld;
        [FormerlySerializedAs("spRUL")] public Sprite 	spRul;
        [FormerlySerializedAs("spDRU")] public Sprite 	spDru;
        public Sprite 	spLDR;
        [FormerlySerializedAs("spUDRL")] public Sprite 	spUdrl;

        public bool up, down, left, right;
        public int type; // -1: Entered, 0: normal, 1: story, 2: mini-boss, 3: secret, 4: trap, 5: loot, 6: special, 7: secret boss, 8: start
        public Color[] colors;
        SpriteRenderer _rend;
        void Start () {
            _rend = GetComponent<SpriteRenderer>();
            PickSprite();
            PickColor();
        }

        void PickSprite(){ //picks correct sprite based on the four door bools
            if (up){
                if (down){
                    if (right){
                        if (left){
                            _rend.sprite = spUdrl;
                        }else{
                            _rend.sprite = spDru;
                        }
                    }else if (left){
                        _rend.sprite = spUld;
                    }else{
                        _rend.sprite = spUd;
                    }
                }else{
                    if (right){
                        if (left){
                            _rend.sprite = spRul;
                        }else{
                            _rend.sprite = spUr;
                        }
                    }else if (left){
                        _rend.sprite = spUl;
                    }else{
                        _rend.sprite = spU;
                    }
                }
                return;
            }
            if (down){
                if (right){
                    if(left){
                        _rend.sprite = spLDR;
                    }else{
                        _rend.sprite = spDr;
                    }
                }else if (left){
                    _rend.sprite = spDL;
                }else{
                    _rend.sprite = spD;
                }
                return;
            }
            if (right){
                if (left){
                    _rend.sprite = spRl;
                }else{
                    _rend.sprite = spR;
                }
            }else{
                _rend.sprite = spL;
            }
        }

        private void PickColor(){ //changes color based on what type the room is
            _rend.color = colors[type];
        }
    }
}
