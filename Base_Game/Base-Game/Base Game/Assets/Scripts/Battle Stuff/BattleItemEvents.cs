using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleItemEvents : MonoBehaviour
{
    public List<InventoryItem> allBattleItems;
    private Inventory2 inventory;

    public InventoryItem battleBomb;
    public InventoryItem redHealthPotion;

    
    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.Find("ItemManager").GetComponent<Inventory2>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
