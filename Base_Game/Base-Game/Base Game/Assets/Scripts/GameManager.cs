using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public GameProgress gameProgress = null;

    public PlayerMovement player;
    public bool wasBattle;
    public int PartyMembers;
    public PartyMembersManager partyMembersManager;
    public QuestManager questManager;

    public PlayerInfo playerInfo;
    public PlayerInfo SallyInfo;

    public string currentRoomName;
    public int currentEnemyIndex;

    public EnemyInfo currentEnemyBattle;
    public EnemyInfo currentEnemySlot1;
    public EnemyInfo currentEnemySlot2;

    public OverworldEnemyList overworldEnemies; 

    // This function is called when a scene this class is in loads
    // If there is already and Awake function in the class (script) you're putting this in, just take the lines from Debug to DontDestroyOnLoad
    private void Awake()
    {  
        PartyMembers = 1;
        if(gameProgress == null)
        {
            gameProgress = new GameProgress();
        }
        if (instance == null)
        {
            playerInfo.currentHP = playerInfo.maxHP;
            SallyInfo.currentHP = SallyInfo.maxHP;
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        Debug.Log("GM is Awake.");
        DontDestroyOnLoad(gameObject);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        partyMembersManager = GameObject.Find("PartyMembersManager").GetComponent<PartyMembersManager>();
        if(player != null && wasBattle)
        {
            player.LoadPlayer();
            wasBattle = false;
        }
        else
        {
            player.transform.position = player.startingPosition.initialValue;
        }

        partyMembersManager.StartParty();
    }

    /*public void PrepareBattleScene(EnemyInfo eInfo)
    {
        Debug.Log("Now setting current enemy room and info.");
        currentEnemyBattle = eInfo;
        //currentRoomName = roomName;
        //currentEnemyIndex = currentIndex;
    }*/

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        foreach(EnemyInfo enemyinfo in overworldEnemies)
        {
            enemyinfo.isAlive = true;
            enemyinfo.currentHP = enemyinfo.maxHP;
        }

        foreach(Quest quest in questManager.Quests)
        {
            quest.ResetQuest();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // one change
    }

    public void NewGame()
    {
        PartyMembers = 1;
        gameProgress.SallyJoined = false;

        foreach(Quest quest in questManager.Quests)
        {
            quest.ResetQuest();
        }
        Debug.Log("New Game?!!?");
    }
}
