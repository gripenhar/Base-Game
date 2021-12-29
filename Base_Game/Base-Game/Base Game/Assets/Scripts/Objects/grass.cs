using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grass : MonoBehaviour
{

    private Animator anim;
    public bool playerInRange;
    public float waitSpeed;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            anim.SetBool("touched", true);
            playerInRange = true;
            StartCoroutine(GrassCo());
            //Debug.Log("in range");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            //anim.SetBool("touched", false);
            playerInRange = false;;
        }
    }

    private IEnumerator GrassCo()
    {
    yield return new WaitForSeconds(waitSpeed);    
    anim.SetBool("touched", false);
    }
}
