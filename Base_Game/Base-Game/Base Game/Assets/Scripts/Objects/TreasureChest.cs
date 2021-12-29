using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureChest : Interactable
{
	[Header ("Contents")]
	public Item contents;
	public Inventory playerInventory;
	public bool isOpen;
	public BoolValue storedOpen;

	[Header("Signals and Dialog")]
	public SignalSender raiseItem;
	public GameObject dialogBox;
	public Text dialogText;

	[Header("Animation")]
	private Animator anim;

	void Start()
	{
		anim = GetComponent<Animator>();
		isOpen = storedOpen.RuntimeValue;
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
		// Dialog window on
		dialogBox.SetActive(true);

		// Dialog text = content text
		dialogText.text = contents.itemDescription;

		// Add contents to the Inventory
		playerInventory.AddItem(contents);
		playerInventory.currentItem = contents;

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
			// Dialog off
			dialogBox.SetActive(false);

			// Raise the signal to the player to stop animating
			raiseItem.Raise();
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
