using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


namespace Source.Soul_Shop
{
    public class CloudGenerator : MonoBehaviour
    {
        public GameObject[] cloudPrefabs;
        public GameObject[] cloudHeights;
        public int maxCloudSpawning;
        public int currentClouds = 0;
        private bool _delay = true;
        
        private void Start()
        {
            SpawnClouds();
        }

        private void Update()
        {
            if (currentClouds < maxCloudSpawning / 1.1f && !_delay)
            {
                _delay = true;
                SpawnClouds();
            }
        }

        private void SpawnClouds()
        {
            for (var i = 0; i < maxCloudSpawning; i++)
            {
                var cloud = Instantiate(cloudPrefabs[Random.Range(0, cloudPrefabs.Length)], new Vector2(CalcX(), Random.Range(-100, 1100)), Quaternion.identity);
                cloud.name = "Cloud";
                cloud.GetComponent<CloudAI>().SpawnCloud(Random.Range(100, 500), this);
                cloud.GetComponent<CloudAI>().SetAlpha(Random.Range(.5f, .9f));
                cloud.transform.SetParent(cloudHeights[Random.Range(0, cloudHeights.Length)].transform);
                
                if (cloud.transform.position.x < 0) cloud.GetComponent<CloudAI>().SetDirection(1);
                else cloud.GetComponent<CloudAI>().SetDirection(-1);
            }
            
            currentClouds += maxCloudSpawning;
            _delay = false;
        }

        private static float CalcX()
        {
            var rand = Random.Range(0, 2);

            return rand == 0 ? Random.Range(-2000, -400) : Random.Range(2300, 4000);
        }
        
    }
}
