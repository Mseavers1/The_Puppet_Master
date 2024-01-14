using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Source.Soul_Shop
{
    public class CloudAI : MonoBehaviour
    {

        private float _speed;
        private int _direction;
        private CloudGenerator _cloudGenerator;

        public void SetDirection(int dir)
        {
            _direction = dir;
        }

        public void SpawnCloud(float speed, CloudGenerator ctx)
        {
            _speed = speed;
            var rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            
            _cloudGenerator = ctx;
        }

        public void SetAlpha(float alpha)
        {
            var color = GetComponent<Image>().color;
            GetComponent<Image>().color = new Color(color.r, color.b, color.g, alpha);
        }

        private void FixedUpdate()
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(_speed * Time.fixedDeltaTime *_direction, 0));

            if (Vector2.Distance(transform.position, transform.parent.position) >= 3000)
            {
                Destroy(this);
                _cloudGenerator.currentClouds--;
            }
        }
    }
}
