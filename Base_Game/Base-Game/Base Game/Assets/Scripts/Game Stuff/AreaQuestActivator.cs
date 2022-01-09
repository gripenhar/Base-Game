using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaQuestActivator : MonoBehaviour
{

    public Quest quest;

    public bool playerInRange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            playerInRange = true;
            //Debug.Log("in range");
            ActivateQuest();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            playerInRange = false;;
        }
    }

    public void ActivateQuest()
    {
        quest.QuestAvailable = true;
    }
}
