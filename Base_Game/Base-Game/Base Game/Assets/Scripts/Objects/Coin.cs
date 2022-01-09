using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : PowerUp
{
    public Inventory2 playerInventory;

    // Start is called before the first frame update
    void Start()
    {
        powerupSignal.Raise();
        playerInventory = GameObject.Find("ItemManager").GetComponent<Inventory2>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
        public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            playerInventory.coins += 1;
            powerupSignal.Raise();
            Destroy(this.gameObject);
        }
    }
}
