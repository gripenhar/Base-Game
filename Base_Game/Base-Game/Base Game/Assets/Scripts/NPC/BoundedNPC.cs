using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundedNPC : MonoBehaviour
{
    private Vector3 directionVector;
    private Transform myTransform;
    public float speed;
    private Rigidbody2D myRigidbody;
    private Animator anim;
    public Collider2D bounds;
    private bool isMoving;
    private bool isInRange;
    public float minMoveTime;
    public float maxMoveTime;
    private float moveTimeSeconds;
    public float minWaitTime;
    public float maxWaitTime;
    private float waitTimeSeconds;
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        moveTimeSeconds = Random.Range(minMoveTime, maxMoveTime);
        waitTimeSeconds = Random.Range(minWaitTime, maxWaitTime);
        anim = GetComponent<Animator>();
        myTransform = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();
        ChangeDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isInRange){
            //base.Update();
            if(isMoving)
            {
                moveTimeSeconds -= Time.deltaTime;
                if(moveTimeSeconds<= 0)
                {
                    moveTimeSeconds = Random.Range(minMoveTime, maxMoveTime);
                    isMoving = false;
                }
                if(!isInRange)
                {
                    Move();
                    anim.SetBool("isIdle", false);
                }
            }
            else
            {
                waitTimeSeconds -= Time.deltaTime;
                if(waitTimeSeconds <= 0)
                {
                    ChooseDifferentDirection();
                    isMoving = true;
                    anim.SetBool("isIdle", false);
                    waitTimeSeconds = Random.Range(minWaitTime, maxWaitTime);
                }
            }
        }
        if(isInRange)
        {
            // GH: get the difference between this.position and target.position
            target = GameObject.Find("Player").GetComponent<Transform>();
            Vector3 temp = new Vector3(target.position.x - transform.position.x,
                                       target.position.y - transform.position.y,
                                       0);
            // GH: set the difference to the anim's "move X" and "move Y" values
            anim.SetFloat("move X", temp.x);
            anim.SetFloat("move Y", temp.y);
        }
    }

    private void ChooseDifferentDirection()
    {
        Vector3 temp = directionVector;
        //ChangeDirection();
        int loops = 0;
        while(temp == directionVector && loops < 100)
        {
            loops++;
            ChangeDirection();
        }

    }

    private void Move()
    {
        var temp = myTransform.position + directionVector * speed * Time.deltaTime;
        if(bounds.bounds.Contains(temp))
        {
            myRigidbody.MovePosition(temp);
        }
        else
        {
            ChangeDirection();
        }

    }

    void ChangeDirection()
    {
        if(!isMoving){
            int direction = Random.Range(0, 4);
            switch(direction)
            {
                case 0:
                // Walking to right
                    directionVector = Vector3.right;
                    break;
                case 1:
                // Walking up
                    directionVector = Vector3.up;
                    break;
                case 2:
                // Walking to left
                    directionVector = Vector3.left;
                    break;
                case 3:
                    directionVector = Vector3.down;
                    break;
                default:
                    directionVector = Vector3.up;
                    break;
            }
            updateAnimation();
        }

    }

    void updateAnimation()
    {
        anim.SetFloat("move X", directionVector.x);
        anim.SetFloat("move Y", directionVector.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //anim.SetBool("isIdle", true);
        ChooseDifferentDirection();
    }

        private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            anim.SetBool("isIdle", true);
            isMoving = false;
            isInRange = true;
        }
    }

        private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
        anim.SetBool("isIdle", false);
        isInRange = false;
        }
    }
}
