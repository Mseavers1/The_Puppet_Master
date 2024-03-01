using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Newtonsoft.Json;
using Source.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Game
{
    public class GameGm : MonoBehaviour
    {
        public GameObject player, enemy;

        public GameObject battleText, cardCanvas, icons, enemyHp;
        public GameObject[] cards;
        
        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }


        // Plays the selected event
        public void LoadEvent(int eventID)
        {
            var gameEvent = LoadEventData(eventID);
            cardCanvas.SetActive(true);
            
            // Load and play the starting audio
            _source.clip = Resources.Load<AudioClip>("Voices/" + eventID + "_start");
            _source.Play();
            var length = _source.clip.length;
            
            // Load Mobs (Only one for now)
            var mob = LoadMobData(gameEvent.Mobs[0].Id);
            
            // Set Enemy Skin
            var enemyImage = mob.HasSpriteSheet ? Resources.LoadAll<Sprite>("Mobs/" + mob.FileName)[mob.FileIndex] : Resources.Load<Sprite>("Mobs/" + mob.FileName);
            enemy.transform.GetChild(0).GetComponent<Image>().sprite = enemyImage;
            enemy.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(enemyImage.rect.width, enemyImage.rect.height);

            // Move Characters in
            player.transform.DOMoveY(800, length).SetEase(Ease.InOutSine);
            enemy.transform.DOMoveY(900, length).SetEase(Ease.InOutSine);
            
            // Move Card in position
            foreach (var card in cards)
                card.transform.DOMoveY(0, length).SetEase(Ease.InOutSine);
            
            // Move Icons in position
            icons.transform.DOMoveY(930, length).SetEase(Ease.InOutSine);
            
            // Move enemy hp in position
            enemyHp.transform.DOMoveX(1880, length).SetEase(Ease.InOutSine);
            

            // Play Battle Start sound
            Invoke(nameof(PlayBattleStart), length);
        }

        private void PlayBattleStart()
        {
            _source.clip = Resources.Load<AudioClip>("Voices/Battle_Start");
            _source.Play();
            battleText.transform.DOMoveX(-500, _source.clip.length).SetEase(Ease.InOutSine);
            
            // Make Characters shake
            player.transform.DOShakePosition(1, 5, 10, 20, false, false).SetLoops(-1).timeScale = 0.2f;
            enemy.transform.DOShakePosition(1, 5, 10, 20, false, false).SetLoops(-1).timeScale = 0.2f;
            
            // After Sound, Begin Battle Mode
            Invoke(nameof(StartBattle), _source.clip.length);
        }

        private void StartBattle()
        {
            
        }



        private static GameEvents LoadEventData(int id)
        {
            var txt = (TextAsset)Resources.Load("GameEvents", typeof(TextAsset));
            var events = JsonConvert.DeserializeObject<List<GameEvents>>(txt.text) ?? throw new Exception("Empty Json!");

            foreach (var gameEvent in events.Where(gameEvent => gameEvent.Id == id))
            {
                return gameEvent;
            }

            throw new Exception("Unable to find event with the ID of " + id);
        }
        
        private static Mobs LoadMobData(int id)
        {
            var txt = (TextAsset)Resources.Load("Mobs", typeof(TextAsset));
            var mobs = JsonConvert.DeserializeObject<List<Mobs>>(txt.text) ?? throw new Exception("Empty Json!");

            foreach (var mob in mobs.Where(gameEvent => gameEvent.ID == id))
            {
                return mob;
            }

            throw new Exception("Unable to find mob with the ID of " + id);
        }
        
    }
    
    internal class Mobs
    {
        public int ID;
        public string Name;
        public bool HasSpriteSheet;
        public string FileName;
        public int FileIndex;

    }
}

