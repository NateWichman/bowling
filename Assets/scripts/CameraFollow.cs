using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 _startPos;
    private GameObject _ball;

    private Animator _animator;

    private bool _isFollowing = false;

    private float _offset;

    private float _minXpos = -40f;

    void Start()
    {
        _startPos = transform.position;
        _animator = GetComponent<Animator>();
        GameManager.Instance.Resetting.AddListener(Reset);
    }

    void Update()
    {
        if (_isFollowing && _ball != null)
        {
            var newXpos = _ball.transform.position.x + _offset;
            if (newXpos < _minXpos)
                newXpos = _minXpos;


            var newYPos = transform.position.y - 20 * Time.deltaTime;
            newYPos = newYPos < 5 ? 5 : newYPos;



            transform.position = new Vector3(newXpos, newYPos, transform.position.z);

        }
    }

    public void FollowBall()
    {
        _ball = GameObject.FindGameObjectWithTag("BALL");
        _offset = Mathf.Abs(_ball.transform.position.x - transform.position.x);
        _isFollowing = true;
        _animator.SetTrigger("PAN");

    }

    private void Reset()
    {
        _isFollowing = false;
        transform.position = _startPos;
        _animator.SetTrigger("RESET");
    }
}
