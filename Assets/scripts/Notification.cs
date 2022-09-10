using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notification : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI Text;


    public void SetMessage(string message)
    {
        Text.SetText(message);
    }
}
