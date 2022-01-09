using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    public GameManager GM;

    public bool startedQuest;
    private EnemyInfo EnemyOgre;
    public DialogueObject NewFarmerDialogue; // completed quest dialogue
    public DialogueObject PostQuestDialogue; // for after you get the reward
    public DialogueObject AcceptQuestDialogue; //for after accepting quest
    private DialogueActivator Farmer;
    public bool gotReward;
    public bool questCompleted;

    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        EnemyOgre = GameObject.Find("Ogre").GetComponent<Enemy>().enemyInfo;
        Farmer = GameObject.Find("Sign Farmer").GetComponent<DialogueActivator>();
    }

    // Update is called once per frame
    void Update()
    {
            if(startedQuest && EnemyOgre.isAlive)
            {
                Farmer = GameObject.Find("Sign Farmer").GetComponent<DialogueActivator>();
                if(Farmer != null)
                {
                    Farmer.dialogueObject = AcceptQuestDialogue;
                }
            }
            if(startedQuest && !EnemyOgre.isAlive)
            {
                Farmer = GameObject.Find("Sign Farmer").GetComponent<DialogueActivator>();
                if(Farmer != null)
                {
                    Farmer.dialogueObject = NewFarmerDialogue;
                }
            }

        if(gotReward)
        {
            Farmer = GameObject.Find("Sign Farmer").GetComponent<DialogueActivator>();
            if(Farmer != null)
            {
                Farmer.dialogueObject = PostQuestDialogue;
            }
        }
    }

    public void StartQuest()
    {
        //add to quest menu
        //change quest givers dialogue
        startedQuest = true;
        Farmer = GameObject.Find("Sign Farmer").GetComponent<DialogueActivator>();
        if(Farmer != null)
        {
        Farmer.dialogueObject = AcceptQuestDialogue;
        }
    }

    public void CompleteQuest()
    {
        questCompleted = true;
    }

    public void GetReward()
    {
        gotReward = true;
        Farmer = GameObject.Find("Sign Farmer").GetComponent<DialogueActivator>();
        if(Farmer != null)
        {
        Farmer.dialogueObject = PostQuestDialogue;
        CompleteQuest();
        }
    }
}
