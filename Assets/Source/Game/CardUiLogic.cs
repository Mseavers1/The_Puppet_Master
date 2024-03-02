using System;
using DG.Tweening;
using Source.Utility;
using Unity.VisualScripting;
using UnityEngine;

namespace Source.Game
{
    public class CardUiLogic : MouseInteraction
    {
        // Bug -- redrawing happens when the deck is empty but doesnt account for the cards in the hand...
        
        private GameObject[] _cardObjs;
        private GameUIHandler _handler;
        public bool isAnimating = false;
        
        public CardUiLogic(GameObject[] cards, GameUIHandler handler) : base("Group C")
        {
            _cardObjs = cards;
            _handler = handler;
        }

        public override void OnLeftClick(string name)
        {
            throw new System.NotImplementedException();
        }

        public override void OnLeftClick(GameObject item)
        {
            var gm = GameObject.FindWithTag("GameManager").GetComponent<GameGm>();
            if (!gm.IsPlayerTurn()) return;

            ClickedIndex = int.Parse(item.name.Split(' ')[1]);
            
            // Check requirement
            var card = gm.GetCardInHand(ClickedIndex - 1);
            
            if (!StaticHolder.PlayerStats.PlayCard(card.GetManaCost(), card.GetStaminaCost()))
            {
                _cardObjs[ClickedIndex - 1].transform.DOShakePosition(0.2f, 3, 20, 2);
                return;
            }
            
            // Hide Card
            isAnimating = true;
            _cardObjs[ClickedIndex - 1].transform.DOMoveY(-500, 0.1f).SetEase(Ease.InOutSine);
            
            // Play card
            if (card.GetTypeName() == 'W')
            {
                var enemy = gm.GetCurrentEnemy();
                enemy.TakeDamage(card.GetDamage());
                
                if (enemy.IsDead()) gm.NextTurn();
            } 
            else if (card.GetTypeName() == 'D')
            {
                
            }
            else if (card.GetTypeName() == 'S')
            {
                // Does do damage?
                if (card.GetDamage() != 0)
                {
                    var enemy = gm.GetCurrentEnemy();
                    enemy.TakeDamage(card.GetDamage());

                    if (enemy.IsDead()) gm.NextTurn();
                }

                // Has Special?
                if (card.GetSpecial() != "")
                {
                    StaticHolder.ParseSpecial(card.GetSpecial(), StaticHolder.PlayerStats);
                }
            }
            else // Misc
            {
                
            }

            // Update UI
            gm.UpdateManaIcon();
            gm.UpdateStaminaIcon();
            gm.UpdateHealthIcon();

            // Redraw card
            _handler.ReturnCardPosition(ClickedIndex - 1);
            
        }

        public override void OnMouseHover(string name)
        {
            throw new System.NotImplementedException();
        }

        public override void OnMouseHover(GameObject item)
        {
            var gm = GameObject.FindWithTag("GameManager").GetComponent<GameGm>();
            if (!gm.IsPlayerTurn()) return;
            
            var i = int.Parse(item.name.Split(' ')[1]);
            
            OnSwitch(i);
        }

        public override void OnSwitch(int index)
        {
            if (isAnimating) return;

            if (SelectedIndex == index) return;

            SelectedIndex = index;
            
            var dur = 0.1f;
            switch (index)
            {
               case 1:
                   _cardObjs[0].transform.DOMoveY(0, dur).SetEase(Ease.InOutSine);
                   _cardObjs[1].transform.DOMoveY(-230, dur).SetEase(Ease.InOutSine);
                   _cardObjs[2].transform.DOMoveY(-230, dur).SetEase(Ease.InOutSine);
                   _cardObjs[3].transform.DOMoveY(-230, dur).SetEase(Ease.InOutSine);
                   _cardObjs[4].transform.DOMoveY(-230, dur).SetEase(Ease.InOutSine);
                   break;
               case 2:
                   _cardObjs[1].transform.DOMoveY(0, dur).SetEase(Ease.InOutSine);
                   _cardObjs[0].transform.DOMoveY(-230, dur).SetEase(Ease.InOutSine);
                   _cardObjs[2].transform.DOMoveY(-230, dur).SetEase(Ease.InOutSine);
                   _cardObjs[3].transform.DOMoveY(-230, dur).SetEase(Ease.InOutSine);
                   _cardObjs[4].transform.DOMoveY(-230, dur).SetEase(Ease.InOutSine);
                   break;
               case 3:
                   _cardObjs[2].transform.DOMoveY(0, dur).SetEase(Ease.InOutSine);
                   _cardObjs[1].transform.DOMoveY(-230, dur).SetEase(Ease.InOutSine);
                   _cardObjs[0].transform.DOMoveY(-230, dur).SetEase(Ease.InOutSine);
                   _cardObjs[3].transform.DOMoveY(-230, dur).SetEase(Ease.InOutSine);
                   _cardObjs[4].transform.DOMoveY(-230, dur).SetEase(Ease.InOutSine);
                   break;
               case 4:
                   _cardObjs[3].transform.DOMoveY(0, dur).SetEase(Ease.InOutSine);
                   _cardObjs[1].transform.DOMoveY(-230, dur).SetEase(Ease.InOutSine);
                   _cardObjs[2].transform.DOMoveY(-230, dur).SetEase(Ease.InOutSine);
                   _cardObjs[0].transform.DOMoveY(-230, dur).SetEase(Ease.InOutSine);
                   _cardObjs[4].transform.DOMoveY(-230, dur).SetEase(Ease.InOutSine);
                   break;
               case 5:
                   _cardObjs[4].transform.DOMoveY(0, dur).SetEase(Ease.InOutSine);
                   _cardObjs[1].transform.DOMoveY(-230, dur).SetEase(Ease.InOutSine);
                   _cardObjs[2].transform.DOMoveY(-230, dur).SetEase(Ease.InOutSine);
                   _cardObjs[3].transform.DOMoveY(-230, dur).SetEase(Ease.InOutSine);
                   _cardObjs[0].transform.DOMoveY(-230, dur).SetEase(Ease.InOutSine);
                   break;
               default:
                   _cardObjs[0].transform.DOMoveY(-230, dur).SetEase(Ease.InOutSine);
                   _cardObjs[1].transform.DOMoveY(-230, dur).SetEase(Ease.InOutSine);
                   _cardObjs[2].transform.DOMoveY(-230, dur).SetEase(Ease.InOutSine);
                   _cardObjs[3].transform.DOMoveY(-230, dur).SetEase(Ease.InOutSine);
                   _cardObjs[4].transform.DOMoveY(-230, dur).SetEase(Ease.InOutSine);
                   break;
            }
        }

        public override void OnSwitchClick(int index)
        {
            throw new System.NotImplementedException();
        }
    }
}
