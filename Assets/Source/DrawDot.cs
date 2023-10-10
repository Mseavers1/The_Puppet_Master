using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawDot : MonoBehaviour
{
    public float radius;
    public Color color = Color.white;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
