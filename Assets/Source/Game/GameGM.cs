using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Newtonsoft.Json;
using Source.Story_Shop;
using Source.Utility;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Source.Game
{
    public class GameGm : MonoBehaviour
    {
        public GameObject player, enemy;

        public GameObject battleText, icons, enemyHp, endTurnSign;
        public CurtainCommands curtainCommands;
        public GameObject[] cards;
        public int currentGameSP;
        public bool endSignLeaving = false;
        
        private AudioSource _source;
        private Deck _deck;
        private Card[] _hand;

        private BattleSimulator _currentBattle;
        private int _currentEventID;
        private float _orinPosPlayer, _orinPosEnemy, _orinPosIcons, _orinPosEnemyIcon;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        public void ShowEndTurnSign()
        {
            endTurnSign.transform.DOKill();
            endTurnSign.transform.DOMoveX(100, 0.2f).SetEase(Ease.InOutSine);
            Invoke(nameof(ShowEndTurnSignTwo), 0.25f);
        }

        private void ShowEndTurnSignTwo()
        {
            endSignLeaving = false;
        }

        public void HideEndTurnSign()
        {
            endSignLeaving = true;
            endTurnSign.transform.DOKill();
            endTurnSign.transform.DOMoveX(-180, 0.2f).SetEase(Ease.InOutSine);
        }

        private void Start()
        {
            currentGameSP = 10;
            
            // Create Deck
            _deck = HoldingOfSkills.GenerateDeck();
            _hand = _deck.GenerateHand();
            
            // Set Display cards to the actual Cards
            var index = 0;
            foreach (var card in cards)
            {
                var currentCard = _hand[index];
                
                // Set Mana Cost
                card.transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<TMP_Text>().text = currentCard.GetManaCost().ToString();
            
                // Set Stamina Cost
                card.transform.GetChild(0).GetChild(1).GetChild(3).GetComponent<TMP_Text>().text = currentCard.GetStaminaCost().ToString();
            
                // Set Name
                card.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = currentCard.GetName();
            
                // Set Level
                card.transform.GetChild(0).GetChild(3).GetComponent<TMP_Text>().text = "lv. " + currentCard.GetLevel();

                // Set Desc
                card.transform.GetChild(0).GetChild(4).GetComponent<TMP_Text>().text = currentCard.GetDesc();

                // Set Type
                card.transform.GetChild(0).GetChild(5).GetComponent<TMP_Text>().text = currentCard.GetFullTypeName();

                index++;
            }
        }

        public void PlayerEndTurn()
        {
            _currentBattle.NextTurn();
        }

        public void NextTurn()
        {
            UpdateHealthIcon();
            
            _currentBattle.NextTurn();
        }

        public bool IsPlayerTurn()
        {
            return _currentBattle.IsPlayerTurn();
        }

        public void ReplaceCardUI(int index)
        {
            var currentCard = _deck.PullCard(_deck.GetTypeIndex(index));
            _hand[index] = currentCard;
                
            // Set Mana Cost
            cards[index].transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<TMP_Text>().text = currentCard.GetManaCost().ToString();
            
            // Set Stamina Cost
            cards[index].transform.GetChild(0).GetChild(1).GetChild(3).GetComponent<TMP_Text>().text = currentCard.GetStaminaCost().ToString();
            
            // Set Name
            cards[index].transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = currentCard.GetName();
            
            // Set Level
            cards[index].transform.GetChild(0).GetChild(3).GetComponent<TMP_Text>().text = "lv. " + currentCard.GetLevel();

            // Set Desc
            cards[index].transform.GetChild(0).GetChild(4).GetComponent<TMP_Text>().text = currentCard.GetDesc();

            // Set Type
            cards[index].transform.GetChild(0).GetChild(5).GetComponent<TMP_Text>().text = currentCard.GetFullTypeName();
        }


        // Plays the selected event
        public void LoadEvent(int eventID)
        {
            var gameEvent = LoadEventData(eventID);
            _currentEventID = eventID;

            // Load and play the starting audio
            _source.clip = Resources.Load<AudioClip>("Voices/" + eventID + "_start");
            _source.Play();
            var length = _source.clip.length;
            
            // Load Mobs (Only one for now)
            var mob = LoadMobData(gameEvent.Mobs[0].Id);

            var mobLevel = Random.Range(gameEvent.Mobs[0].LevelMin, gameEvent.Mobs[0].LevelMax);
            
            // Set Enemy Skin
            var enemyImage = mob.HasSpriteSheet ? Resources.LoadAll<Sprite>("Mobs/" + mob.FileName)[mob.FileIndex] : Resources.Load<Sprite>("Mobs/" + mob.FileName);
            enemy.transform.GetChild(0).GetComponent<Image>().sprite = enemyImage;
            enemy.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(enemyImage.rect.width, enemyImage.rect.height);

            // Move Characters in
            _orinPosEnemy = enemy.transform.position.x;
            _orinPosPlayer = player.transform.position.x;
            
            player.transform.DOMoveY(800, length).SetEase(Ease.InOutSine);
            enemy.transform.DOMoveY(900, length).SetEase(Ease.InOutSine);
            
            // Move Card in position
            foreach (var card in cards)
                card.transform.DOMoveY(-230, length).SetEase(Ease.InOutSine);
            
            // Ensure Icons are correct
            UpdateHealthIcon();
            UpdateManaIcon();
            UpdateStaminaIcon();

            // Move Icons in position
            _orinPosIcons = icons.transform.position.x;
            icons.transform.DOMoveY(930, length).SetEase(Ease.InOutSine);

            // Move enemy hp in position
            _orinPosEnemyIcon = enemyHp.transform.position.x;
            enemyHp.transform.DOMoveX(1880, length).SetEase(Ease.InOutSine);
            
            // Create Battle
            _currentBattle = new BattleSimulator(this, gameEvent.Mobs[0].Id, mobLevel);
            UpdateEnemyHealthIcon(_currentBattle.GetCurrentEnemy());
            
            // Play Battle Start sound
            Invoke(nameof(PlayBattleStart), length);

        }

        public Card GetCardInHand(int index)
        {
            return _hand[index];
        }

        public void UpdateHealthIcon()
        {
            // Display Player Stat in icons
            var stats = StaticHolder.PlayerStats;
            var health = icons.transform.GetChild(3).GetChild(1);
            icons.transform.GetChild(3).GetChild(3).GetComponent<TMP_Text>().text =
                stats.CurrentHealth + " / " + stats.StatValues["Health"];
            
            var x = (float) (stats.CurrentHealth / stats.StatValues["Health"]);

            if (x < 0) x = 0;
            
            health.localScale = new Vector3(health.localScale.x, x, health.localScale.z);
        }
        
        public void UpdateEnemyHealthIcon(EnemyInfo enemy)
        {
            // Display Player Stat in icons
            var health = enemyHp.transform.GetChild(0).GetChild(1);
            var x = (float) (enemy.Stat.CurrentHealth / enemy.Stat.StatValues["Health"]);

            if (x < 0) x = 0;
            
            health.localScale = new Vector3(health.localScale.x, x, health.localScale.z);
        }
        
        public void UpdateStaminaIcon()
        {
            // Display Player Stat in icons
            var stats = StaticHolder.PlayerStats;
            var stamina = icons.transform.GetChild(1).GetChild(1);
            
            icons.transform.GetChild(1).GetChild(3).GetComponent<TMP_Text>().text =
                stats.CurrentStamina + " / " + stats.StatValues["Stamina"];
            
            var x = (float) (stats.CurrentStamina / stats.StatValues["Stamina"]);

            if (x < 0) x = 0;
            
            stamina.localScale = new Vector3(stamina.localScale.x, x, stamina.localScale.z);
        }
        
        public void UpdateManaIcon()
        {
            // Display Player Stat in icons
            var stats = StaticHolder.PlayerStats;
            var mana = icons.transform.GetChild(2).GetChild(1);
            var x = (float) (stats.CurrentMana / stats.StatValues["Mana"]);
            
            icons.transform.GetChild(2).GetChild(3).GetComponent<TMP_Text>().text =
                stats.CurrentMana + " / " + stats.StatValues["Mana"];

            if (x < 0) x = 0;
            
            mana.localScale = new Vector3(mana.localScale.x, x, mana.localScale.z);
        }

        private void PlayBattleStart()
        {
            _source.clip = Resources.Load<AudioClip>("Voices/Battle_Start");
            _source.Play();
            battleText.transform.DOMoveX(-500, _source.clip.length).SetEase(Ease.InOutSine);
            
            // Make Characters shake
            player.transform.DOShakePosition(1, 5, 10, 20, false, false).SetLoops(-1).timeScale = 0.2f;
            enemy.transform.DOShakePosition(1, 5, 10, 20, false, false).SetLoops(-1).timeScale = 0.2f;
            
            Invoke(nameof(StartBattle), _source.clip.length);
        }

        private void StartBattle()
        {
            _currentBattle.NextTurn();
        }

        public EnemyInfo GetCurrentEnemy()
        {
            return _currentBattle.GetCurrentEnemy();
        }
        
        public IEnumerator DoEnemyTurn(EnemyInfo enemy, float delay)
        {
            yield return new WaitForSeconds(delay);
            
            enemy.PlayTurn();
        }

        public void PlayTurnSound()
        {
            _source.clip = Resources.Load<AudioClip>(IsPlayerTurn() ? "Voices/Player_Turn" : "Voices/Enemy_Turn");
            _source.Play();
        }
        
        public float PlayWinSound()
        {
            _source.clip = Resources.Load<AudioClip>("Voices/" + _currentEventID + "_win");
            _source.Play();
            return _source.clip.length;
        }
        
        public float PlayLoseSound()
        {
            _source.clip = Resources.Load<AudioClip>("Voices/" + _currentEventID + "_lose");
            _source.Play();
            return _source.clip.length;
        }

        public IEnumerator ReturnToSelector(float delay)
        {
            player.transform.DOKill();
            enemy.transform.DOKill();
            
            yield return new WaitForSeconds(delay);
            
            GetComponent<MapControl>().TurnOnMap();
            
            player.transform.DOMoveY(2000, 1.5f).SetEase(Ease.InOutSine);
            enemy.transform.DOMoveY(2000, 1.5f).SetEase(Ease.InOutSine);
            enemyHp.transform.DOMoveX(2000, 1.5f).SetEase(Ease.InOutSine);
            icons.transform.DOMoveY(2000, 1.5f).SetEase(Ease.InOutSine);
            HideEndTurnSign();

            foreach (var card in cards)
            {
                card.transform.DOKill();
                card.transform.DOMoveY(-530, 1.5f).SetEase(Ease.InOutSine);
            }
            
            yield return new WaitForSeconds(2f);
            
            curtainCommands.OpenCurtains(1.5f);
        }

        public IEnumerator Lose(float delay)
        {
            yield return new WaitForSeconds(delay);
            player.transform.DOKill();
            enemy.transform.DOKill();
            
            StaticHolder.PlayerStats.Heal();
            StaticHolder.PlayerStats.RestoreMana();
            StaticHolder.PlayerStats.RestoreStamina();

            GlobalResources.SoulEssences += currentGameSP + (GetComponent<MapControl>().GetRoomSpPerRoom() - 1) * 50;
            SceneManager.LoadScene(2);
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

