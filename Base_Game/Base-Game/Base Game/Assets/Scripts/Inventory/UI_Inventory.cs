using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
	/*private Inventory2 inventory;
	private Transform itemSlotContainer;
	private Transform itemSlotTemplate;

	private void Awake(){
		itemSlotContainer = transform.Find("itemSlotContainer");
		itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
	}

	public void SetInventory(Inventory2 inventory) {
		this.inventory = inventory;
		RefreshInventoryItems();
	}

	private void RefreshInventoryItems(){
		int x = 0;
		int y = 0;
		float itemSlotCellSize = 133f;

		foreach (Item2 item in inventory.GetItemList()){
		RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
		itemSlotRectTransform.gameObject.SetActive(true);
		itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
		x++;
		if (x > 3) {
			x = 0;
			y++;
		}
		}
	}*/
}
