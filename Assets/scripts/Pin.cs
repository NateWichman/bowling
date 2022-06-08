using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    public GameObject PinHeight;
    // Start is called before the first frame update

    private AudioSource _audioSource;

    public bool hasFallen = false;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {


        if (collision.gameObject.tag == "BALL" || collision.gameObject.tag == "PIN")
        {
            _audioSource.Play();
        }
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
            _audioSource.Play();
            GameManager.Instance.PinFall();
            hasFallen = true;
        }
    }
}
