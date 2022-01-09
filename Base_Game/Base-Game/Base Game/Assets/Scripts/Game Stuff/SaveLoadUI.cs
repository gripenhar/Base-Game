using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;

public class SaveLoadUI : MonoBehaviour
{
    public GameObject SaveLoadScreen;
    public GameObject SlotData1;
    public GameObject SlotData2;
    public GameObject SlotData3;
    public GameObject LoadedText;
    public GameObject SavedText;

    public TMP_Text NameText1;
    public TMP_Text NameText2;
    public TMP_Text NameText3;

    public int currentSaveSlot;

    // Start is called before the first frame update
    void Start()
    {
        //if(File.Exists($@"{Application.persistentDataPath}/game1.weef"))
		//{
        //    GameData tempData = SaveSystem2.Load(1);
        //    NameText1.text = tempData.PlayerName;
        //    SlotData1.SetActive(true);
        //}
        //if(File.Exists($@"{Application.persistentDataPath}/game2.weef"))
		//{
        //    GameData tempData = SaveSystem2.Load(2);
        //    NameText2.text = tempData.PlayerName;
        //    SlotData2.SetActive(true);
        //}
        //if(File.Exists($@"{Application.persistentDataPath}/game3.weef"))
		//{
        //    GameData tempData = SaveSystem2.Load(3);
        //    NameText3.text = tempData.PlayerName;
        //    SlotData3.SetActive(true);
        //}
    }

    // Update is called once per frame
    void Update()
    {

		if(File.Exists($@"{Application.persistentDataPath}/game1.weef"))
		{
            GameData tempData = SaveSystem2.Load(1);
            NameText1.text = tempData.PlayerName;
            SlotData1.SetActive(true);
        }
        if(File.Exists($@"{Application.persistentDataPath}/game2.weef"))
		{
            GameData tempData = SaveSystem2.Load(2);
            NameText2.text = tempData.PlayerName;
            SlotData2.SetActive(true);
        }
        if(File.Exists($@"{Application.persistentDataPath}/game3.weef"))
		{
            GameData tempData = SaveSystem2.Load(3);
            NameText3.text = tempData.PlayerName;
            SlotData3.SetActive(true);
        }
    }

    public void ReturnToMain()
    {
        SaveLoadScreen.SetActive(false);
    }

    public void ExitToStart()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void LoadGame()
    {
        GameData data = SaveSystem2.Load(currentSaveSlot);
        GameManager GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(LoadedTextShow());

        GM.gameProgress.PlayerName = data.PlayerName;
        Debug.Log($@"We should have just loaded PartyMembers = [{data.PartyMembers}]");
        GM.PartyMembers = data.PartyMembers;
        GM.gameProgress.SallyJoined = data.SallyJoined;
        if(GM.gameProgress.SallyJoined && GM.PartyMembers < 2)
        {
            Debug.LogError("LoadGame(): SallyJoined is true, but PartyMembers is less than 2");
        }

        int i = 0;
        foreach(QuestData questData in data.questDataList)
        {
            Debug.Log("Just loaded me up a QUEST");
            GM.questManager.Quests[i].QuestAvailable = questData.QuestAvailable;
            GM.questManager.Quests[i].StartQuest = questData.StartQuest;
            GM.questManager.Quests[i].CompleteQuest = questData.CompleteQuest;
            GM.questManager.Quests[i].GotReward = questData.GotReward;
            i++;
        }

            //TODO: should load into saved scene instead
            SceneManager.LoadScene("SampleScene");

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
    }

    public void SaveGame()
    {
        Debug.Log("About to Save ur game in slot#:" + currentSaveSlot);
        GameManager GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        SaveSystem2.Save(GM, currentSaveSlot);
        StartCoroutine(SavedTextShow());
    }

    public void SetToSlot1()
    {
        currentSaveSlot = 1;
        Debug.Log("Set the currentSaveSlot to 1");
    }

    public void SetToSlot2()
    {
        currentSaveSlot = 2;
        Debug.Log("Set the currentSaveSlot to 2");
    }

    public void SetToSlot3()
    {
        currentSaveSlot = 3;
        Debug.Log("Set the currentSaveSlot to 3");
    }

    IEnumerator LoadedTextShow()
    {
        LoadedText.SetActive(true);
        yield return new WaitForSeconds(1f);
        LoadedText.SetActive(false);
    }

    IEnumerator SavedTextShow()
    {
        SavedText.SetActive(true);
        yield return new WaitForSeconds(1f);
        SavedText.SetActive(false);
    }

}
