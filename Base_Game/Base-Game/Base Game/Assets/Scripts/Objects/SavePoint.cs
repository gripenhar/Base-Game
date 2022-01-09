using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private Transform target;
    public bool playerInRange;
    public Animator anim;
    public GameObject dialogBox;
    public bool isBookOpen;
    private GameProgress game;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(Input.GetButtonDown("attack") && playerInRange)
        {
            if(!isBookOpen)
            {
                isBookOpen = true;
                anim.SetBool("bookOpen", isBookOpen);
            }
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
        }
    }

    public void SaveGame()
    {
        if(isBookOpen)
        {
            //GH: do the actual game save stuff here
            //GameManager GM = GameObject.Find("GameManager").GetComponent<GameManager>();
            //SaveSystem2.Save(GM);
            isBookOpen = false;
            anim.SetBool("bookOpen", isBookOpen);
            //GH: CloseBook needs to be called AFTER the dialogue completely ends
        }
    }


    /*
    public void LoadGame()
    {
        GameData data = SaveSystem2.Load();
        GameManager GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        GM.PartyMembers = data.PartyMembers;
        GM.gameProgress.SallyJoined = data.SallyJoined;

        int i = 0;
        foreach(QuestData questData in data.questDataList)
        {
            GM.questManager.Quests[i].QuestAvailable = questData.QuestAvailable;
            GM.questManager.Quests[i].StartQuest = questData.StartQuest;
            GM.questManager.Quests[i].CompleteQuest = questData.CompleteQuest;
            GM.questManager.Quests[i].GotReward = questData.GotReward;
            i++;
        }
        if(GM.PartyMembers > 1)
        {
            LoadPartyMembers();
        }
        
    }

    public void LoadPartyMembers()
    {
        GameManager GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        // GH: we don't have the data structure right now for dynamically setting the Party Members
        if(GM.gameProgress.SallyJoined)
        {
            GM.partyMembersManager.CreateSally();
        }
    }*/
}
