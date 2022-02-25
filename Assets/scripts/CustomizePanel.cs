using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CustomizePanel : MonoBehaviour
{
    [SerializeField]
    private GameObject _cell;

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
        _items = new List<Item> {
        new Item("House Ball", "", false, Resources.Load<Material>("Balls/HouseBall")),
        new Item("Bronze", "Score at least 100 points", true, Resources.Load<Material>("Balls/Bronze")),
        new Item("Silver", "Score at least 150 points", true, Resources.Load<Material>("Balls/Silver")),
        new Item("Gold", "Score at least 200 points", true, Resources.Load<Material>("Balls/Gold")),
        new Item("Diamond", "Score a perfect 300", true, Resources.Load<Material>("Balls/Diamond"))
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
