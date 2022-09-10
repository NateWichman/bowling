using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{

    float x;
    float y;
    float z;
    // Start is called before the first frame update
    void Start()
    {
        x = transform.eulerAngles.x;
        y = transform.eulerAngles.y;
        z = transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(x, y, z);
    }
}
