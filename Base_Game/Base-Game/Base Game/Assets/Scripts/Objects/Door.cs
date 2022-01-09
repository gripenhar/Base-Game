using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType
{
    key,
    enemy,
    button
}

public class Door : Interactable
{
    [Header("Door variables")]
    public DoorType thisDoorType;
    public bool open = false;
    public Inventory2 playerInventory;
    public SpriteRenderer doorSprite;
    public BoxCollider2D physicsCollider;
    public InventoryItem Key;

    void Start()
    {
        playerInventory = GameObject.Find("ItemManager").GetComponent<Inventory2>();
    }

    private void Update()
    {
        if(Input.GetButtonDown("attack"))
        {
            if(playerInRange && thisDoorType == DoorType.key)
            {
                //Does the player have a key?
                Debug.Log(playerInventory.itemList.Contains(Key));
                if(playerInventory.itemList.Contains(Key))
                {
                    //remove a player key
                    playerInventory.itemList.Remove(Key);
                    //If so. then call the open method
                    Open();
                }
                
            }
        }
    }

    public void Open()
    {
        //turn off the door's sprite renderer
        doorSprite.enabled = false;
        //set open to true
        open = true;
        //turn off the door''s box collider
        physicsCollider.enabled = false;
    }

    public void Close()
    {
        //turn on the door's sprite renderer
        doorSprite.enabled = true;
        //set open to false
        open = false;
        //turn on the door''s box collider
        physicsCollider.enabled = true;
    }
}
