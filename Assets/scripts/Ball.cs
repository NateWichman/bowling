using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public UiManager UIManager;
    public bool IsThrown = false;

    private float MinPower = 1500f;
    private float MaxPower = 4000f;

    private float Power = 1500f;

    private float spin = 0f;
    private float _maxSpin = 2000f;
    private float _minSpin = 0f;

    private bool mouseDown = false;

    private float _powerUnit;
    private float _spinUnit;

    private Rigidbody rb;

    private LineRenderer lineRenderer;
    private AudioSource _audioSource;

    private Vector3 dir;

    void Start()
    {
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

    }


    public void Shoot()
    {
        GameManager.Instance.OnThrow();
        var force = (-dir.normalized) * Power * rb.mass;
        rb.AddForce(force);
        rb.AddTorque(new Vector3(-1, 0, 0) * spin * rb.mass);
        _audioSource.Play();
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


    public void Update()
    {
        if (IsThrown)
        {
            return;
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100))
        {
            lineRenderer.SetPosition(0, new Vector3(transform.position.x, transform.position.y, transform.position.z));
            lineRenderer.SetPosition(1, new Vector3(hit.point.x, transform.position.y, hit.point.z));

            dir = new Vector3(transform.position.x, transform.position.y, transform.position.z) - new Vector3(hit.point.x, transform.position.y, hit.point.z);
        }


        if (Input.GetMouseButtonDown(0))
        {
            mouseDown = true;
        }

        if (mouseDown)
        {
            WindUp();
        }

        if (Input.GetMouseButton(1))
        {
            AddSpin();
        }

        if (Input.GetMouseButtonUp(0))
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

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {

            gameObject.transform.Translate(new Vector3(0f, 0, -1f) * 10f * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Translate(new Vector3(0f, 0, 1f) * 10f * Time.deltaTime);
        }

    }
}
