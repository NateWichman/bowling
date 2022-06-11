using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    private float speed = 20f;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Time.deltaTime * speed, 0, Space.Self);
    }
}
