using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Skin : MonoBehaviour
{

    private GameObject _addOn;
    public bool IsUI = false;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SkinChange.AddListener(Respond);
        Respond();
    }
    void Destroy() 
    {
        GameManager.Instance.SkinChange.RemoveListener(Respond); 
    }

    public void Respond() {
        string ball = PlayerPrefs.GetString("BALL");

        if (ball == null || ball == "")
        {
            ball = "HouseBall";
        }
        Material material = Resources.Load<Material>("Balls/" + ball);
          if (_addOn != null)
        {
            Destroy(_addOn);
        }
        GameObject addOn = Resources.Load<GameObject>("AddOns/" + ball);
              if (addOn != null) {
            _addOn = Instantiate(addOn, transform);
            _addOn.transform.SetParent(transform);

            if (IsUI)
            {
                SetLayerRecursively(_addOn, 5);
            }
        }
        GetComponent<Renderer>().material = material;
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform t in obj.transform)
        {
            SetLayerRecursively(t.gameObject, layer);
        }
    }
}
