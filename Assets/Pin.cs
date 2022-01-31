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

        if (!hasFallen && (PinHeight.transform.position.y < GameManager.Instance.PinHeight
            || PinHeight.transform.position.y > GameManager.Instance.PinMax))
        {
            GameManager.Instance.PinFall();
            hasFallen = true;
        }
    }
}
