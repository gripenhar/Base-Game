using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject
{
	public InventoryItem currentItem;
	public List<Item> items = new List<Item>();
	public int numberOfKeys;
	public int coins;
	public float maxMagic = 10;
	public float currentMagic;

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

	public void OnEnable()
	{
		currentMagic = maxMagic;
	}

	public void ReduceMagic(float magicCost)
	{
		currentMagic -= magicCost;
		if(currentMagic <= 0f)
		{
			currentMagic = 0f;
		}
	}

	public bool CheckForItem(Item item)
	{
		if(items.Contains(item))
		{
			return true;
		}
		return false;
	}

	public void AddItem(Item itemToAdd)
	{
		// chekc if item is a key
		if (itemToAdd.isKey)
		{
			numberOfKeys++;
		}
		else
		{
			if (!items.Contains(itemToAdd))
			{
				items.Add(itemToAdd);
			}
		}
	}
}
