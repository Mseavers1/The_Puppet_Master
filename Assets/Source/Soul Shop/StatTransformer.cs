using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

namespace Source.Soul_Shop
{
    public class StatTransformer : MonoBehaviour
    {
        private CanvasRenderer _renderer;
        public TMP_Text[] statValues;
        public Material material;
        public GameObject[] positions;
        public Texture2D texture2D;
        private Dictionary<string, int> _statDictionary = new();

        public float[] stats;
        private List<Vector2> defaultPositions = new();

        public void IncrementStat(string stat)
        {
            stats[_statDictionary[stat]]++;
            UpdateMesh();
        }
        
        public void ReduceStat(string stat)
        {
            stats[_statDictionary[stat]]--;
            UpdateMesh();
        }
        

        private void Awake()
        {
            _renderer = GetComponent<CanvasRenderer>();
            _statDictionary.Add("Vitality", 0);
            _statDictionary.Add("Intelligence", 4);
            _statDictionary.Add("Luck", 1);
            _statDictionary.Add("Endurance", 3);
            _statDictionary.Add("Speed", 2);
            _statDictionary.Add("Agility", 5);
            _statDictionary.Add("Strength", 6);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(2))
            {
                UpdateMesh();
            }
        }

        private void Start()
        {
            foreach (var pos in positions) defaultPositions.Add((pos.transform.position));

            UpdateMesh();
        }

        private void UpdateDisplay()
        {
            for (var i = 0; i < stats.Length; i++)
            {
                statValues[i].text = stats[i].ToString(CultureInfo.InvariantCulture);
            }
        }

        private void UpdateMesh()
        {
            UpdateDisplay();

            for (var i = 1; i < positions.Length; i++)
            {
                var statSlope = (defaultPositions[i].x - positions[0].transform.position.x) / 200;
                var x = statSlope * stats[i - 1] + positions[0].transform.position.x;
                var slope = (positions[0].transform.position.y - defaultPositions[i].y) / (positions[0].transform.position.x - defaultPositions[i].x);
                var b = positions[0].transform.position.y - (slope * positions[0].transform.position.x);
                var y = slope * x + b;
                positions[i].transform.position = (new Vector2(x, y));
            }
            
            _renderer.Clear();
            GenerateMesh();
        }

        private void GenerateMesh()
        {
            var vertices = new Vector3[]
            {
                (positions[0].transform.position),
                (positions[1].transform.position),
                (positions[2].transform.position),
                (positions[3].transform.position),
                (positions[4].transform.position),
                (positions[5].transform.position),
                (positions[6].transform.position),
                (positions[7].transform.position)
            };

            var triangles = new int[]
            {
                0, 1, 2,
                0, 2, 3,
                0, 3, 4,
                0, 4, 5,
                0, 5, 6,
                0, 6, 7,
                7, 1, 0

            };

            var uvs = new Vector2[]
            {
                Vector2.zero, 
                Vector2.one, 
                Vector2.one, 
                Vector2.one, 
                Vector2.one, 
                Vector2.one, 
                Vector2.one, 
                Vector2.one
            };

            var mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            
            this.GetComponent<MeshFilter>().mesh = mesh;

            _renderer.materialCount = 1;
            _renderer.SetMaterial(material, texture2D);
            _renderer.SetMesh(mesh);
        }
    }
}
