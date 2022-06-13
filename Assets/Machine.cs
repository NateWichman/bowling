using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    public Transform EndPoint;
    private bool _isSweeping = false;
    private float _time = 0f;
    private float _duration = 1f;
    private Rigidbody _rb;

    public static Machine Instance;

    private Vector3 _startPos;


    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;
    }

    // Start is called before the first frame update
    void Start()
    { 
        _startPos = transform.position;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isSweeping)
        {
            _time += Time.deltaTime;
            _rb.MovePosition(Vector3.Lerp(_startPos, EndPoint.position, _time / _duration));

            if (_time > _duration)
            {
                transform.position = _startPos;
                _time = 0f;
                _isSweeping = false;
                gameObject.SetActive(false);
            }
        }
    }


    public void SweepPins()
    {
        gameObject.SetActive(true);
        GetComponent<Animation>().Play("MachineCrank");
    }

    public void OnAnimationFinish()
    {
        _isSweeping = true;
    }
}
