using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CustomizePanel : MonoBehaviour
{
    [SerializeField]
    private GameObject _cell;

    private CustomizeService _customizeService;

    public static CustomizePanel Instance;

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

    void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        _customizeService = CustomizeService.Instance;
        RefreshUI();
    }

    public void RefreshUI()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        _items = new List<Item> {
        new Item("House Ball", "", false, Resources.Load<Material>("Balls/HouseBall")),
        new Item("Bronze", "Score at least 100 points", !_customizeService.Unlocks[KeyEnum.Bronze], Resources.Load<Material>("Balls/Bronze")),
        new Item("Silver", "Score at least 150 points", !_customizeService.Unlocks[KeyEnum.Silver], Resources.Load<Material>("Balls/Silver")),
        new Item("Gold", "Score at least 200 points", !_customizeService.Unlocks[KeyEnum.Gold], Resources.Load<Material>("Balls/Gold")),
        new Item("Ghost", "Get 0 points in a game", !_customizeService.Unlocks[KeyEnum.Ghost], Resources.Load<Material>("Balls/Ghost")),
        new Item("Moon", "Play 10 Games", !_customizeService.Unlocks[KeyEnum.Moon], Resources.Load<Material>("Balls/Moon")),
        new Item("Earth", "Play 100 Games", !_customizeService.Unlocks[KeyEnum.Earth], Resources.Load<Material>("Balls/Earth"))
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
