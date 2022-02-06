using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public UiManager UIManager;
    public bool IsThrown = false;

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
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.positionCount = 100;
        lineRenderer.positionCount = 2;

        // randomizing ball position.
        transform.Translate(new Vector3(0, 0, Random.Range(-4.5f, 4.5f)));
        dir.z += Random.Range(-4.5f, 4.5f);
        dir.x = transform.position.x - 15f;


        UpdateLine();

    }

    void Destroy()
    {
        Debug.Log("destorying");
        _inputService.InputEvent.RemoveListener(InputEvent);
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

        var direction = new Vector3(transform.position.x, transform.position.y, transform.position.z) - new Vector3(dir.x, transform.position.y, dir.z);
        direction = -direction.normalized;
        var force = (direction) * Power * rb.mass;
        rb.AddForce(force);
        rb.AddTorque(new Vector3(-1, 0, 0) * spin * rb.mass);
        _audioSource.Play();

        StartCoroutine("Timeout");
    }

    private void WindUp()
    {
        Power += _powerUnit * 100f * Time.deltaTime;
        if (Power > MaxPower)
        {
            Power = MaxPower;
        }

        var percent = (Power - MinPower) / _powerUnit;
        UIManager.SetSlider((int)percent);
    }

    private void AddSpin()
    {
        spin += _spinUnit * 100f * Time.deltaTime;
        if (spin > _maxSpin)
        {
            spin = _maxSpin;
        }
        var percent = (spin - _minSpin) / _spinUnit;
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
        yield return new WaitForSeconds(13f);
        GameManager.Instance.ForceBallFall();
    }
}
