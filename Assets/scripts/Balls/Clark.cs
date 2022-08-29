using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clark : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.Shooting.AddListener(OnShoot);
    }

    private void OnShoot()
    {
        _animator.SetBool("isRunning", true);
    }

    void Destroy()
    {
        GameManager.Instance.Shooting.RemoveListener(OnShoot);
    }

}
