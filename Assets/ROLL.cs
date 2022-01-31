using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ROLL : MonoBehaviour
{
    public float Speed;

    void FixedUpdate()
    {
        float hM = Input.GetAxis("Horizontal");
        float vM = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(-vM, 0, hM);
        GetComponent<Rigidbody>()
            .AddForce(move * Speed * Time.deltaTime);

        GetComponent<Rigidbody>().AddTorque(new Vector3(-1, 0, 0) * Speed * 10 * Time.deltaTime);
    }
}
