using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FacingDirection
{
	Down,
    Left,
    Right,
    Up
}

public class SignNPC : MonoBehaviour
{
    private Transform target;
    public bool playerInRange;
    private Animator anim;

    // FacingDirection setters
    public FacingDirection facingDir;
    private float xDir;
    private float yDir;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        anim = this.GetComponent<Animator>();
        anim.SetBool("isIdle", true);
        SetFacingDirection();
        anim.SetFloat("move X", xDir);
        anim.SetFloat("move Y", yDir);
    }

    // Update is called once per frame
    void Update()
    {
        
        // GH: This code makes it so that the SignNPC looks in the direction
        //     of the Player when the Player is in range        
        if(playerInRange)
        {
            xDir = target.position.x - transform.position.x;
            yDir = target.position.y - transform.position.y;
            // GH: get the difference between this.position and target.position   
            Vector3 temp = new Vector3(xDir, yDir, 0);

            // GH: set the difference to the anim's "move X" and "move Y" values
            anim.SetFloat("move X", temp.x);
            anim.SetFloat("move Y", temp.y);
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
            playerInRange = false;
            SetFacingDirection();
            //anim.SetFloat("move X", 0f);
            //anim.SetFloat("move Y", -1f);
            anim.SetFloat("move X", xDir);
            anim.SetFloat("move Y", yDir);
        }
    }

    private void SetFacingDirection()
    {
        switch(facingDir)
        {
            case FacingDirection.Up:
                xDir = 0f;
                yDir = 1f;
                break;
            case FacingDirection.Left:
                xDir = -1f;
                yDir = 0f;
                break;
            case FacingDirection.Right:
                xDir = 1f;
                yDir = 0f;
                break;
            case FacingDirection.Down:
                xDir = 0f;
                yDir = -1f;
                break;
            default:
                xDir = 0f;
                yDir = -1f;
                break;
        }
    }
}
