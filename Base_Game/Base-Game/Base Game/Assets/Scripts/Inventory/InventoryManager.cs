using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{

    [Header("Inventory Information")]
    public Inventory2 inventory2;

    [SerializeField] private GameObject blankInventorySlot;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private GameObject useButton;
    public InventoryItem currentItem;

    public void SetTextAndButton(string description, bool buttonActive)
    {
        descriptionText.text = description;
        if(buttonActive)
        {
            useButton.SetActive(true);
        }
        else
        {
            useButton.SetActive(false);
        }
    }

    void MakeInventorySlots()
    {
        inventory2 = GameObject.Find("ItemManager").GetComponent<Inventory2>();
        if(inventory2 != null)
        {
            //inventory2 = this.GetComponent<Inventory2>();
            Debug.Log(inventory2.itemList.Count);
            foreach(InventoryItem invItem in inventory2.itemList)
            {
                //if(invItem.numberHeld > 0)
                //{
                GameObject temp = 
                Instantiate(blankInventorySlot, inventoryPanel.transform.position, Quaternion.identity);
                temp.transform.SetParent(inventoryPanel.transform);
                InventorySlot newSlot = temp.GetComponent<InventorySlot>();
                if(newSlot)
                {
                    newSlot.Setup(invItem, this);
                }
                //}
            }
            //for(int i = 0; i < inventory2.itemList.Count; i ++)
            //{
            //    Debug.Log(inventory2.itemList[i]);
            //    if(inventory2.itemList[i].numberHeld > 0 /*|| 
            //    inventory2.itemList[i].itemName == "Bottle"*/)
            //    {
            //        Debug.Log(inventory2.itemList[i]);
            //        GameObject temp = 
            //        Instantiate(blankInventorySlot, inventoryPanel.transform.position, Quaternion.identity);
            //        temp.transform.SetParent(inventoryPanel.transform);
            //        InventorySlot newSlot = temp.GetComponent<InventorySlot>();
            //        if(newSlot)
            //        {
            //            newSlot.Setup(inventory2.itemList[i], this);
            //        }
            //    }
            //}
        }
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        ClearInventorySlots();
        MakeInventorySlots();
        SetTextAndButton("", false);
    }


    public void SetupDescriptionAndButton(string newDescriptionString, bool isButtonUsable, InventoryItem newItem)
    {
        currentItem = newItem;
        descriptionText.text = newDescriptionString;
        useButton.SetActive(isButtonUsable);
    }

    void ClearInventorySlots()
    {
        for(int i = 0; i < inventoryPanel.transform.childCount; i ++)
        {
            Destroy(inventoryPanel.transform.GetChild(i).gameObject);
        }
    }

    public void UseButtonPressed ()
    {
        if(currentItem)
        {
            currentItem.Use();
            inventory2.itemList.Remove(currentItem);
            //clear all the inventory slots
            ClearInventorySlots();
            //refill all the slots with new numbers
            MakeInventorySlots();
            SetTextAndButton("", false);
        }
    }
}
