using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeIcon : MonoBehaviour
{
    void Start()
    {
        UpdateColor();
    }

    public void UpdateColor()
    {

        var materialName = PlayerPrefs.GetString("BALL");
        if (materialName == null || materialName == "")
        {
            materialName = "HouseBall";
        }
        var material = Resources.Load<Material>("Balls/" + materialName);
        GetComponent<Renderer>().material = material;

    }

}
