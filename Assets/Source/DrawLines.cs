using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DrawLines : MonoBehaviour
{
    public Vector3 direction;
    public Color color = Color.blue;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawLine(transform.position, transform.position + direction);
    }
}
