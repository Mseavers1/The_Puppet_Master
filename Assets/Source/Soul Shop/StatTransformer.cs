using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Soul_Shop
{
    public class StatTransformer : MonoBehaviour
    {
        public float offset = 10;
        private CanvasRenderer _renderer;
        public Material material;
        public GameObject[] positions;

        public float[] stats;
        private List<Vector2> defaultPositions = new();

        private void Awake()
        {
            _renderer = GetComponent<CanvasRenderer>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(2))
            {
                UpdateJoint();
                _renderer.Clear();
                GenerateMesh();
            }
        }

        private void Start()
        {
            foreach (var pos in positions) defaultPositions.Add(pos.transform.position);
            
            
            GenerateMesh();
        }

        private void UpdateJoint()
        {
            for (var i = 1; i < positions.Length; i++)
            {
                var statSlope = (defaultPositions[i].x - positions[0].transform.position.x) / 200;
                var x = statSlope * stats[i - 1] + positions[0].transform.position.x;
                var slope = (positions[0].transform.position.y - defaultPositions[i].y) / (positions[0].transform.position.x - defaultPositions[i].x);
                var b = positions[0].transform.position.y - (slope * positions[0].transform.position.x);
                var y = slope * x + b;
                positions[i].transform.position = (new Vector2(x, y));
            }
        }

        private void GenerateMesh()
        {
            var vertices = new Vector3[]
            {
                new Vector3(positions[0].transform.position.x, positions[0].transform.position.y),
                new Vector3(positions[1].transform.position.x, positions[1].transform.position.y),
                new Vector3(positions[2].transform.position.x, positions[2].transform.position.y),
                new Vector3(positions[3].transform.position.x, positions[3].transform.position.y),
                new Vector3(positions[4].transform.position.x, positions[4].transform.position.y),
                new Vector3(positions[5].transform.position.x, positions[5].transform.position.y),
                new Vector3(positions[6].transform.position.x, positions[6].transform.position.y),
                new Vector3(positions[7].transform.position.x, positions[7].transform.position.y),
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

            var mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            
            this.GetComponent<MeshFilter>().mesh = mesh;
            
            _renderer.materialCount = 1;
            _renderer.SetMaterial(material, 0);
            _renderer.SetMesh(mesh);

            transform.position = Camera.main.ScreenToWorldPoint(positions[0].transform.position);
        }
    }
}
