using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Items")]
public class InventoryItem : ScriptableObject
{
	public string itemName;
	public string itemDescription;
	public Sprite itemImage;

	public bool usable;
	public bool unique;
	public bool inBattle;
	public UnityEvent thisEvent;

	//public InventoryItem InventoryItem(string name, string desc, Sprite img, int numOfItems, bool consumable, bool isUnique, UnityEvent event)
	//{
	//	itemName = name;
	//	itemDescription = desc;
	//	itemImage = img;
	//	numberHeld = numOfItems;
	//	usable = consumable;
	//	unique = isUnique;
	//	thisEvent = event;
	//}
	
	public void Use()
	{
		thisEvent.Invoke();
	}
}
