using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ChestType
{
	Item,
	Coins
}

public class TreasureChest : Interactable
{
	[Header ("Contents")]
	public ChestType thisChestType;
	public InventoryItem contents;
	public Inventory2 playerInventory;
	public bool isOpen;
	public BoolValue storedOpen;
	public int coinAmount;
	public Sprite coinSprite;
	private SpriteRenderer CoinSpritePlace;

	[Header("Signals and Dialog")]
	public SignalSender raiseItem;
	public GameObject dialogBox;
	public Text dialogText;

	[Header("Animation")]
	private Animator anim;

	void Start()
	{
		anim = GetComponent<Animator>();
		playerInventory = GameObject.Find("ItemManager").GetComponent<Inventory2>();
		//isOpen = storedOpen.RuntimeValue;
		isOpen = false;
		if(isOpen){
			anim.SetBool("opened", true);
		}
	}

	void Update()
	{
		if(Input.GetButtonDown("attack") && playerInRange)
        {
			if(!isOpen)
			{
				// Open the chest
				OpenChest();
			}
			else
			{
				// Chest is already open
				CloseChest();
			}
        }
	}

	public void OpenChest()
	{
		if(thisChestType == ChestType.Item)
		{
			playerInventory.currentItem = contents;
			//contents.numberHeld = 1;
			playerInventory.itemList.Add(contents);
			dialogText.text = contents.itemDescription;
		}
		
		if(thisChestType == ChestType.Coins)
		{
			playerInventory.coins += coinAmount;
			dialogText.text = coinAmount + " coins were inside!";
			CoinSpritePlace = GameObject.Find("Received Item").GetComponent<SpriteRenderer>();
			CoinSpritePlace.sprite = coinSprite;
		}
		// Dialog window on
		dialogBox.SetActive(true);

		// Raise the signal to the player to animate
		raiseItem.Raise();

		// Raise the context clue (to turn it off)
		context.Raise();

		// Set chest to opened (isOpen = true)
		isOpen = true;

		// animate opening chest
		anim.SetBool("opened", true);
		storedOpen.RuntimeValue = isOpen;
	}

	public void CloseChest()
	{
		PlayerMovement playerM = GameObject.Find("Player").GetComponent<PlayerMovement>();
		// Dialog off
		dialogBox.SetActive(false);
		playerM.animator.SetBool("receive item", false);
		if(thisChestType == ChestType.Coins)
		{
			CoinSpritePlace.sprite = null;
		}
		// Raise the signal to the player to stop animating
		raiseItem.Raise();
		playerInRange = false;
	}

	private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger && !isOpen)
        {
            context.Raise();
            playerInRange = true;
            //Debug.Log("in range");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger && !isOpen)
        {
            context.Raise();
            playerInRange = false;
        }
    }
}
