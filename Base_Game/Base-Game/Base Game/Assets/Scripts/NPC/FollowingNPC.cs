using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingNPC : MonoBehaviour
{
    public float speed;
    private Transform target;
    public bool playerInRange;
    public Animator anim;

    private Vector3 change;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    { 

        if(!playerInRange)
        {
            // GH: get the difference between this.position and target.position   
            Vector3 temp = new Vector3(target.position.x - transform.position.x,
                                       target.position.y - transform.position.y,
                                       0);


            // GH: set the difference to the anim's "move X" and "move Y" values
            anim.SetFloat("move X", temp.x);
            anim.SetFloat("move Y", temp.y);
            anim.SetBool("isIdle", false);
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        else
        {
            // GH: get the difference between this.position and target.position   
            Vector3 temp = new Vector3(target.position.x - transform.position.x,
                                       target.position.y - transform.position.y,
                                       0);


            // GH: set the difference to the anim's "move X" and "move Y" values
            anim.SetFloat("move X", temp.x);
            anim.SetFloat("move Y", temp.y);
            anim.SetBool("isIdle", true);
        }
    }

        private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {

            playerInRange = false;;
        }
    }



}
