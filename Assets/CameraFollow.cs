using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 _startPos;
    private GameObject _ball;

    private bool _isFollowing = false;

    private float _offset;

    private float _minXpos = -40f;

    void Start()
    {
        _startPos = transform.position;
    }

    void Update()
    {
        if (_isFollowing && _ball != null)
        {
            var newXpos = _ball.transform.position.x + _offset;
            if (newXpos < _minXpos)
                newXpos = _minXpos;
            transform.position = new Vector3(newXpos, transform.position.y, transform.position.z);
        }

        if (_ball == null)
        {
            _isFollowing = false;
            transform.position = _startPos;
        }
    }

    public void FollowBall()
    {
        _ball = GameObject.FindGameObjectWithTag("BALL");
        _offset = Mathf.Abs(_ball.transform.position.x - transform.position.x);
        _isFollowing = true;
    }
}
