using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsActive : MonoBehaviour
{
    public BoxCollider2D boxCollider2D;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
     GameManager GM = GameObject.Find("GameManager").GetComponent<GameManager>();
     boxCollider2D = GetComponent<BoxCollider2D>();
        if(GM.PartyMembers > 1)
        {
            Destroy(boxCollider2D);
        }
    }
}
