using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerMoves : MonoBehaviour
{
    public GameObject prevFarmer;

    private GameObject currentTarget;
    public GameObject nextPos;
    private bool reachedNextPos;
    public GameObject endPos;

    public GameObject thisFarmer;
    public float speed;
    private bool moved;

    private Transform myTransform;
    private Rigidbody2D myRigidbody;
    private Animator anim;
    private BoxCollider2D boxcollider;

    private PlayerMovement player;

   

    // Start is called before the first frame update
    void Start()
    {
        prevFarmer.SetActive(false);
        anim = GetComponent<Animator>();
        myTransform = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(thisFarmer.activeInHierarchy && !moved)
        {
            if(!reachedNextPos)
            {
                currentTarget = nextPos;
                anim.SetBool("isIdle", false);
                anim.SetFloat("move X", -1f);
                anim.SetFloat("move Y", 0f);
                
            }
            else if(reachedNextPos)
            {
                currentTarget = endPos;
                anim.SetBool("isIdle", false);
                anim.SetFloat("move X", 0f);
                anim.SetFloat("move Y", 1f);
            }
            // Grab GameObject to get transform.position
            //endPos = GameObject.Find("FarmerMoves_EndPosition");

            thisFarmer.transform.position = Vector2.MoveTowards(thisFarmer.transform.position, 
                                                                currentTarget.transform.position, 
                                                                speed * Time.deltaTime);
        }
        if(nextPos != null && thisFarmer.transform.position == nextPos.transform.position)
        {
            reachedNextPos = true;
        }
        if(endPos != null && thisFarmer.transform.position == endPos.transform.position)
        {
             anim.SetBool("isIdle", true);
             anim.SetFloat("move X", 0f);
             anim.SetFloat("move Y", -1f);
             moved = true;
             player = GameObject.Find("Player").GetComponent<PlayerMovement>();
             player.currentState = PlayerState.walk;
        }
    }
}
