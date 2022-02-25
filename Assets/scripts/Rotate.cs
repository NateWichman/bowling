using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed = 1f;
    // Update is called once per frame


    void Update()
    {
        transform.Rotate(0, Time.deltaTime * speed, 0, Space.Self);
    }
}
