using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treeShake : Interactable
{
    public Animator anim;
    public bool isShaking;
    public GameObject leaves;
    public LootTable treeLoot;
    public GameObject lootLocation;
    public GameObject dropSpot;
    public bool looted;

    public GameObject itemHolder;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float speed = 6f;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        isShaking = false;
        startPosition = lootLocation.transform.position;
        endPosition = dropSpot.transform.position;
        looted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("attack") && playerInRange)
        {
            anim.SetBool("shaking", true);
            leaves.SetActive(true);
            isShaking = true;
            StartCoroutine(shakeCo()); 
            if(looted == false){
                MakeLoot();
            }
        }

        if(looted && itemHolder.transform.position != endPosition)
        {
            Vector3 temp = Vector3.MoveTowards(itemHolder.transform.position, endPosition, speed * Time.deltaTime);
            itemHolder.transform.position = temp;
        }
    }

    private IEnumerator shakeCo()
    {
        yield return new WaitForSeconds(1f);        
        anim.SetBool("shaking", false);
        leaves.SetActive(false);
        isShaking = false;
    }

    private void MakeLoot()
    {
        if(treeLoot != null)
        {
            PowerUp current = treeLoot.LootPowerUp();
            if(current != null)
            {
                var myNewItem = Instantiate(current.gameObject, lootLocation.transform.position, Quaternion.identity);
                myNewItem.transform.parent = itemHolder.transform;
            }
        }
        looted = true;
    }
}
