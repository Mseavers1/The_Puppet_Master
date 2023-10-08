using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    [Range(1, 10)]
    public float smoothFactor;
    public GameObject walls;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        var newPos = Vector3.Lerp(cam.transform.position, CalcBoundedPos(), smoothFactor * Time.fixedDeltaTime);
        cam.transform.position = newPos;
    }

    private Vector3 CalcBoundedPos()
    {
        var x = CalcBoundAxis(transform.position.x, true, 2, 3);
        var y = CalcBoundAxis(transform.position.y, false, 0, 1);
        var z = -10;

        return new Vector3 (x, y, z);
    }

    private float CalcBoundAxis(float pos, bool isX, int wallOne, int wallTwo)
    {
        var one = walls.transform.GetChild(wallOne).position;
        var two = walls.transform.GetChild(wallTwo).position;

        if (isX)
        {
            var width = 8.9f;
            return Mathf.Clamp(pos, one.x + width, two.x - width);
        }

        var height = 5;
        return Mathf.Clamp(pos, one.y + height, two.y - height);

    }


}
