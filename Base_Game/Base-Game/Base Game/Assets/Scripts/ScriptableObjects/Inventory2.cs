using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory2 : MonoBehaviour
{
 public List<InventoryItem> itemList = new List<InventoryItem>();
 public int coins;
 public int numberOfKeys;
 public InventoryItem currentItem;


 public void AddItem(InventoryItem item)
 {
	  itemList.Add(item);
 }

 public List<InventoryItem> GetItemList(){
	  return itemList;
 }

 	public bool IncreaseCoins(int coinsBack)
	{
	 coins += coinsBack;
	 return true;
	}

	public bool ReduceCoins(int coinCost)
	{
		if(coins >= coinCost)
		{
			coins -= coinCost;
			return true;
		}
		else
		{
			if(coins < 0)
			{
				Debug.Log("We should never get here. You should never have negative coins.");
				coins = 0;
			}
			return false;
		}
	}


}
