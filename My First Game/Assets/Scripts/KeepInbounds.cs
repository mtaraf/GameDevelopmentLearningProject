using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepInbounds : MonoBehaviour
{
    void Update()
    {
        Vector3 currentPosition = transform.position;
        if (transform.position.z > 149)
        {
            transform.position = new Vector3(currentPosition.x, currentPosition.y, 149);
        }
        else if (transform.position.x < -49)
        {
            transform.position = new Vector3(-49, currentPosition.y, currentPosition.z);
        }
        else if (transform.position.z < 51)
        {
            transform.position = new Vector3(currentPosition.x, currentPosition.y, 51);
        }
    }
}
