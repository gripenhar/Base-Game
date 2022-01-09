using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneActivator : MonoBehaviour, IInteractable
{
	[SerializeField] public DialogueObject dialogueObject;
	[SerializeField] private GameObject dialogueBox;
	public BoxCollider2D boxCollider2D;
	public GameObject NPC;
	private PlayerMovement player;
	private bool doneTalking;
	public AudioClip sound;

    // Start is called before the first frame update
    void Start()
    {
       //boxCollider2D = GetComponent<BoxCollider2D>();
	   player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
		if(doneTalking)
		{
			player = GameObject.Find("Player").GetComponent<PlayerMovement>();
			if(!player.DialogueUI.IsOpen)
			{
				if(NPC != null)
				{
					NPC.SetActive(true);
					Destroy(this);
				}
			}
		}
    }

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(!doneTalking && other.CompareTag("Player") && other.TryGetComponent(out PlayerMovement player))
		{
	
			//player.DialogueUI.ShowDialogue(dialogueObject);
			//this.dialogueObject = dialogueObject;
			PlayAudio();
			player.Interactable = this;
			//text doubles?
			Debug.Log("About to call CutSceneActivator.Interact()");
			this.Interact(player);
			if(boxCollider2D != null)
			{
				Destroy(boxCollider2D);
			}
			player.animator.SetBool("moving", false);
			//pauses the player
			player.currentState = PlayerState.interact;
			doneTalking = true;
		}

	}

	//private void OnTriggerExit2D(Collider2D other)
	//{
	//	if(other.CompareTag("Player") && other.TryGetComponent(out PlayerMovement player))
	//	{
	//		if (player.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
	//		{
	//			player.Interactable = null;
	//		}
	//	}
	//}

	public void Interact(PlayerMovement player)
	{
		Debug.Log("We are now inside CutSceneActivator.Interact()");
		foreach(DialogueResponseEvents responseEvents in GetComponents<DialogueResponseEvents>())
		{
			if(responseEvents.DialogueObject == dialogueObject)
			{
				Debug.Log(responseEvents.Events);
				Debug.Log("Add responseEvents");
				player.DialogueUI.AddResponseEvents(responseEvents.Events);
				break;
			}
		}
	
		player.DialogueUI.ShowDialogue(dialogueObject);
	}

	public void PlayAudio()
	{
		if(sound != null)
		{
		AudioSource soundMain = GameObject.Find("Main Camera").GetComponent<AudioSource>();
		soundMain.Stop();
		AudioSource sound = this.GetComponent<AudioSource>();
		sound.Play();
		}
	}
}
