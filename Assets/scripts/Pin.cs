using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    public GameObject PinHeight;
    // Start is called before the first frame update

    public bool hasFallen = false;
    void Start()
    {
    }

    public void Destroy()
    {
        GameObject.Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        /* probably need to calculate if a pin has fallen differently, sometimes it gets blown up in the air,
        and wont get below the Gamemanagers pin height before the round ends */
        if (!hasFallen && PinHeight.transform.position.y < GameManager.Instance.PinHeight)
        {
            GameManager.Instance.PinFall();
            hasFallen = true;
        }
    }
}
