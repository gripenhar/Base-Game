using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNPC : PowerUp
{
    public PlayerInventory playerInventory;
    public Inventory playerInventory1;
    public InventoryItem RedPotion;
    public InventoryItem GreenPotion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SubtractCoins(int itemCost)
    {
        playerInventory1.coins -= itemCost;
        powerupSignal.Raise();
    }

    public void AddRedPotion()
    {
        if(playerInventory && RedPotion)
        {
            if(playerInventory.myInventory.Contains(RedPotion))
            {
                RedPotion.numberHeld += 1;
            }
            else
            {
                playerInventory.myInventory.Add(RedPotion);
                RedPotion.numberHeld += 1;
            }
        }
    }

    public void AddGreenPotion()
    {
        if(playerInventory && GreenPotion)
        {
            if(playerInventory.myInventory.Contains(GreenPotion))
            {
                GreenPotion.numberHeld += 1;
            }
            else
            {
                playerInventory.myInventory.Add(GreenPotion);
                GreenPotion.numberHeld += 1;
            }
        }
    }
}
