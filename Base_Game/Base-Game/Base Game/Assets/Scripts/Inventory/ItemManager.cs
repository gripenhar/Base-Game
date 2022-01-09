using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Inventory2 myInventory;
    public List<InventoryItem> allItems;


    // Start is called before the first frame update
    void Start()
    {
        myInventory = this.GetComponent<Inventory2>();
        //myInventory.AddItem(allItems[0]);
        //myInventory.AddItem(allItems[4]);
        //myInventory.AddItem(allItems[3]);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
