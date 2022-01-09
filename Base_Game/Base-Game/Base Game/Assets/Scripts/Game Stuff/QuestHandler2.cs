using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler2 : MonoBehaviour
{
    public GameManager GM;

    public Quest TestQuest;
    public DialogueActivator QuestNPC;
    public Quest activatedQuest;


    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        //EnemyOgre = GameObject.Find("Ogre").GetComponent<Enemy>().enemyInfo;
        QuestNPC = GameObject.Find(TestQuest.QuestNPCName).GetComponent<DialogueActivator>();
    }

    // Update is called once per frame
    void Update()
    {
            if(TestQuest.QuestAvailable)
            {
                QuestNPC = GameObject.Find(TestQuest.QuestNPCName).GetComponent<DialogueActivator>();
                if(QuestNPC != null)
                {
                    QuestNPC.dialogueObject = TestQuest.StartQuestDialogue;
                }
            }
            if(TestQuest.StartQuest && !TestQuest.IsEveryEnemyDead())
            {
                QuestNPC = GameObject.Find(TestQuest.QuestNPCName).GetComponent<DialogueActivator>();
                if(QuestNPC != null)
                {
                    QuestNPC.dialogueObject = TestQuest.AcceptQuestDialogue;
                }
            }
            if(TestQuest.StartQuest && TestQuest.IsEveryEnemyDead())
            {
                QuestNPC = GameObject.Find(TestQuest.QuestNPCName).GetComponent<DialogueActivator>();
                if(QuestNPC != null)
                {
                    QuestNPC.dialogueObject = TestQuest.CompleteQuestDialogue;
                }
            }

        if(TestQuest.GotReward)
        {
            QuestNPC = GameObject.Find(TestQuest.QuestNPCName).GetComponent<DialogueActivator>();
            if(QuestNPC != null)
            {
                QuestNPC.dialogueObject = TestQuest.GotRewardDialogue;
            }
        }
    }

    public void StartQuest()
    {
        //add to quest menu
        //change quest givers dialogue
        TestQuest.StartQuest = true;
        QuestNPC = GameObject.Find(TestQuest.QuestNPCName).GetComponent<DialogueActivator>();
        if(QuestNPC != null)
        {
            QuestNPC.dialogueObject = TestQuest.AcceptQuestDialogue;
        }
    }

    public void CompleteQuest()
    {
        TestQuest.CompleteQuest = true;
    }

    public void GetReward()
    {
        TestQuest.GotReward = true;
        QuestNPC = GameObject.Find(TestQuest.QuestNPCName).GetComponent<DialogueActivator>();
        if(QuestNPC != null)
        {
            QuestNPC.dialogueObject = TestQuest.GotRewardDialogue;
            CompleteQuest();
        }
    }

    public void ActivateQuest()
    {
        if(activatedQuest != null)
        {
        activatedQuest.QuestAvailable = true;
        }

    }


}
