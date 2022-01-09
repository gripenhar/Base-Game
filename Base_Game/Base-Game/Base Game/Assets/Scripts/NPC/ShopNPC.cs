using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNPC : PowerUp
{
    public Inventory2 playerInventory;
    //public Inventory playerInventory1;
    public InventoryItem RedPotion;
    public InventoryItem GreenPotion;
    private bool wasAbleToBuy;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = GameObject.Find("ItemManager").GetComponent<Inventory2>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCoins(int moneyBack)
    {
        playerInventory.IncreaseCoins(moneyBack);
    }

    /*
    public void SubtractCoins(int itemCost)
    {
        wasAbleToBuy = playerInventory1.ReduceCoins(itemCost);
        if(wasAbleToBuy)
        {
            
            powerupSignal.Raise();
        }
        else
        {
            Debug.Log("Could not afford to buy, so did not buy.");
        }
    }
    */
    public void AddRedPotion()
    {
        playerInventory.itemList.Add(RedPotion);
        //if(playerInventory && RedPotion)
        //{
        //    if(playerInventory.itemList.Contains(RedPotion))
        //    {
        //        RedPotion.numberHeld += 1;
        //    }
        //    else
        //    {
        //        
        //        RedPotion.numberHeld = 1;
        //    }
        //}
    }

    public void AddGreenPotion()
    {
        playerInventory.itemList.Add(GreenPotion);
        //if(playerInventory && GreenPotion)
        //{
        //    if(playerInventory.itemList.Contains(GreenPotion))
        //    {
        //        GreenPotion.numberHeld += 1;
        //    }
        //    else
        //    {
        //        
        //        GreenPotion.numberHeld = 1;
        //    }
        //}
    }
}
