﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public UiManager UIManager;
    public bool IsThrown = false;

    public GameObject trail;
    public GameObject spinTrail;

    private float MinPower = 500f;
    private float MaxPower = 4000f;

    private float Power = 500f;

    private float spin = 0f;
    private float _maxSpin = 2000f;
    private float _minSpin = 0f;

    private bool mouseDown = false;

    private float _powerUnit;
    private float _spinUnit;

    private Rigidbody rb;

    private LineRenderer lineRenderer;
    private AudioSource _audioSource;

    private Vector3 dir = new Vector3(1f, 0, 0);
    private InputService _inputService;

    void Start()
    {
        spinTrail.SetActive(false);
        trail.SetActive(false);
        _inputService = InputService.Instance;
        _inputService.InputEvent.AddListener(InputEvent);
        rb = GetComponent<Rigidbody>();
        _powerUnit = (MaxPower - MinPower) / 100f;
        _spinUnit = (_maxSpin - _minSpin) / 100f;
        rb.maxAngularVelocity = 100000;

        _audioSource = GetComponent<AudioSource>();

        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 1f;
        lineRenderer.positionCount = 100;
        lineRenderer.positionCount = 2;

        // randomizing ball position.
        transform.Translate(new Vector3(0, 0, Random.Range(-4.5f, 4.5f)));
        dir.z += Random.Range(-4.5f, 4.5f);
        dir.x = transform.position.x - 30f;


        UpdateLine();

        LoadMaterial();
    }

    void Destroy()
    {
        _inputService.InputEvent.RemoveListener(InputEvent);
    }

    void LoadMaterial()
    {
        string ball = PlayerPrefs.GetString("BALL");

        if (ball == null || ball == "")
        {
            ball = "HouseBall";
        }

        Material material = Resources.Load<Material>("Balls/" + ball);
        GetComponent<Renderer>().material = material;
    }

    private void InputEvent(InputEventStruct data)
    {
        if (IsThrown) return;


        if (data.Type == InputType.POWER)
        {
            if (data.IsDown)
            {
                mouseDown = true;
            }
            else
            {
                IsThrown = true;
                if (mouseDown)
                {
                    Shoot();

                }
                mouseDown = false;
                UIManager.SetSlider(0);
                UIManager.SetSecondarySlider(0);
            }
        }
    }


    public void Shoot()
    {
        GameManager.Instance.OnThrow();
        trail.SetActive(true);
        spinTrail.SetActive(true);
        
        var direction = new Vector3(transform.position.x, transform.position.y, transform.position.z) - new Vector3(dir.x, transform.position.y, dir.z);
        direction = -direction.normalized;
        var force = (direction) * Power * rb.mass;
        rb.AddForce(force);
        rb.AddTorque(new Vector3(-1, 0, 0) * spin * (InputService.Instance.SpinDirection == Direction.LEFT ? 1 : -1) * rb.mass);
        _audioSource.Play();

        StartCoroutine("Timeout");
    }

    private void WindUp()
    {
        Power += _powerUnit * 100f * Time.deltaTime;
        if (Power > MaxPower)
        {
            Power = MaxPower;
            _powerUnit = -1 * _powerUnit;
        }
        else if (Power < MinPower)
        {
            Power = MinPower;
            _powerUnit = -1 * _powerUnit;
        }
        var percent = (Power - MinPower) / Mathf.Abs(_powerUnit);
        UIManager.SetSlider((int)percent);
    }

    private void AddSpin()
    {
        spin += _spinUnit * 100f * Time.deltaTime;
        if (spin > _maxSpin)
        {
            spin = _maxSpin;
            _spinUnit = -1 * _spinUnit;
        }
        else if (spin < _minSpin)
        {
            spin = _minSpin;
            _spinUnit = -1 * _spinUnit;
        }
        var percent = (spin - _minSpin) / Mathf.Abs(_spinUnit);
        UIManager.SetSecondarySlider((int)percent);
    }

    private void UpdateLine()
    {
        lineRenderer.SetPosition(0, new Vector3(transform.position.x, transform.position.y, transform.position.z));
        lineRenderer.SetPosition(1, new Vector3(dir.x, transform.position.y, dir.z));
    }


    public void Update()
    {
        if (IsThrown)
        {
            lineRenderer.enabled = false;
            return;
        }

        if (mouseDown)
        {
            WindUp();
        }

        if (_inputService.IsSpinDown)
        {
            AddSpin();
        }

        if (_inputService.IsLeftDown)
        {
            gameObject.transform.Translate(new Vector3(0f, 0, -1f) * 12.5f * Time.deltaTime);
            UpdateLine();
        }

        if (_inputService.IsRightDown)
        {
            gameObject.transform.Translate(new Vector3(0f, 0, 1f) * 12.5f * Time.deltaTime);
            UpdateLine();
        }

        if (_inputService.IsUpperLeftDown)
        {
            dir.z += -12.5f * Time.deltaTime;
            UpdateLine();
        }

        if (_inputService.IsUpperRightDown)
        {
            dir.z += +12.5f * Time.deltaTime;
            UpdateLine();
        }

    }

    IEnumerator Timeout()
    {
        yield return new WaitForSeconds(20f);
        GameManager.Instance.ForceBallFall();
    }
}
