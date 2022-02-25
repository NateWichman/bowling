using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CustomizePanel : MonoBehaviour
{
    [SerializeField]
    private GameObject _cell;

    private UnlockManager UM;

    private class Item
    {
        public string Name;
        public string SubText;
        public bool Locked;
        public Material Material;

        public Item(string name, string subText, bool locked, Material material)
        {
            Name = name;
            SubText = subText;
            Locked = locked;
            Material = material;
        }
    };

    private List<Item> _items;
    // Start is called before the first frame update
    void Start()
    {
        Init();
        UnlockManager.Instance.UpdateEvent.AddListener(Reset);
    }

    void Reset(bool val)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        Init();
    }

    void Init()
    {
        UM = UnlockManager.Instance;
        _items = new List<Item> {
        new Item("House Ball", "", false, Resources.Load<Material>("Balls/HouseBall")),
        new Item("Bronze", "Score at least 100 points", !UM.Unlocks.bronze, Resources.Load<Material>("Balls/Bronze")),
        new Item("Silver", "Score at least 150 points", !UM.Unlocks.silver, Resources.Load<Material>("Balls/Silver")),
        new Item("Gold", "Score at least 200 points", !UM.Unlocks.gold, Resources.Load<Material>("Balls/Gold")),
        new Item("Emerald", "Score at least 250", !UM.Unlocks.emerald, Resources.Load<Material>("Balls/Emerald")),
        new Item("Diamond", "Score a perfect 300", !UM.Unlocks.gold, Resources.Load<Material>("Balls/Diamond"))
    };

        _items.ForEach(item => AddItem(item));
    }


    private void AddItem(Item item)
    {
        var obj = Instantiate(_cell);
        obj.GetComponent<CustomizeItem>().Initialize(
            item.Name,
            item.SubText,
            item.Locked,
            item.Material
        );
        obj.transform.SetParent(transform, false);
    }
}
