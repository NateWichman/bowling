using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clark : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    private Rigidbody _ballRigidBody;
    private bool _isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.layer == 5) return; // ui
        GameManager.Instance.Shooting.AddListener(OnShoot);
        var ball = GameObject.FindGameObjectWithTag("BALL");
        _ballRigidBody = ball.GetComponent<Rigidbody>();
    }

    private void OnShoot()
    {
        if (gameObject.layer == 5) return; // ui
        _isRunning = true;
        _animator.SetBool("isRunning", true);
        _animator.speed = 3;
    }

    void Update()
    {
        if (!_isRunning) return;

        if (gameObject.layer == 5) return; // ui

        _animator.speed = _ballRigidBody.velocity.magnitude / 40f;
    }

    void Destroy()
    {
        if (gameObject.layer == 5) return; // ui
        GameManager.Instance.Shooting.RemoveListener(OnShoot);
    }

}
