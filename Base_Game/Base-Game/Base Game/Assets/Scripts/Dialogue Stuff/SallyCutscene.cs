using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SallyCutscene : MonoBehaviour
{
    private Transform myTransform;
    public float speed;
    private Rigidbody2D myRigidbody;
    private Animator anim;
    private Transform target;
    public GameObject Sally;
    private PlayerMovement player;
    public SignNPC signNPC;
    //[SerializeField] public DialogueObject dialogueObject2;
    private bool doneTalking;
    //public DialogueActivator NPC;
    //[SerializeField] public DialogueObject dialogueObject3;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        myTransform = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Sally.activeInHierarchy && !doneTalking)
        {
            //face the player
            target = GameObject.Find("Player").GetComponent<Transform>();
            Vector3 temp = new Vector3(target.position.x - transform.position.x,
                                       target.position.y - transform.position.y,
                                       0);
            // GH: set the difference to the anim's "move X" and "move Y" values
            anim.SetFloat("move X", temp.x);
            anim.SetFloat("move Y", temp.y);
            //move to player
            transform.position = Vector2.MoveTowards(transform.position, target.position + new Vector3(0 -1,0,0), speed * Time.deltaTime);
            if(Sally.transform.position == target.position + new Vector3(0 -1,0,0))
            {
                //set her still
                anim.SetBool("isIdle", true);
                player = GameObject.Find("Player").GetComponent<PlayerMovement>();
                player.currentState = PlayerState.walk;

		        doneTalking = true;
                signNPC.GetComponent<SignNPC>().enabled = true;
            }
        }
    }

    /*private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Player") && other.TryGetComponent(out PlayerMovement player))
		{
			
		    foreach(DialogueResponseEvents responseEvents in GetComponents<DialogueResponseEvents>())
		    {
			    if(responseEvents.DialogueObject == dialogueObject2)
			    {
				    player.DialogueUI.AddResponseEvents(responseEvents.Events);
				    break;
			    }
		    }
	
		//player.DialogueUI.ShowDialogue(dialogueObject);
		}

	}*/

}
