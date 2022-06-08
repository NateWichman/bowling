using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CustomizeItem : MonoBehaviour
{
    [SerializeField]
    private GameObject _ball;

    [SerializeField]
    private GameObject _lockIcon;

    [SerializeField]
    private TextMeshProUGUI _nameText;

    [SerializeField]
    private TextMeshProUGUI _subText;
    private Material _material;

    private bool _isLocked;

    public void Initialize(
        string name,
        string subtext,
        bool locked,
        Material material
    )
    {

        _isLocked = locked;
        _nameText.SetText(name);
        _subText.SetText(subtext);
        _lockIcon.SetActive(locked);
        _ball.GetComponent<Renderer>().material = material;
        _material = material;
    }

    public void SetMaterial()
    {
        if (_isLocked) return;

        GameManager.Instance.SetMaterial(_material);
        GameManager.Instance.OnEndCustomize();
    }
}
