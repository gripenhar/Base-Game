using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalInventoryItem : MonoBehaviour
{
    [SerializeField] private Inventory2 inventory2;
    [SerializeField] private InventoryItem thisItem; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && !other.isTrigger)
        {
            AddItemToInventory();
            Destroy(this.gameObject);
        }
    }

    void AddItemToInventory()
    {
        inventory2 = GameObject.Find("ItemManager").GetComponent<Inventory2>();
        inventory2.itemList.Add(thisItem);
        //if(inventory2 && thisItem)
        //{
        //    if(inventory2.itemList.Contains(thisItem))
        //    {
        //        thisItem.numberHeld += 1;
        //    }
        //    else
        //    {
        //        inventory2.itemList.Add(thisItem);
        //        thisItem.numberHeld = 1;
        //    }
        //}
    }
}
