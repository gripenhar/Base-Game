using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
	public string PlayerName;
	public bool SallyJoined;
	public int PartyMembers;
	public List<QuestData> questDataList;

	public GameData(GameManager GM)
	{
		PlayerName = GM.gameProgress.PlayerName;
		SallyJoined = GM.gameProgress.SallyJoined;
		PartyMembers = GM.gameProgress.PartyMembers;
		questDataList = new List<QuestData>();
		foreach(Quest quest in GM.questManager.Quests)
		{
			questDataList.Add(new QuestData()
			{
				QuestAvailable = quest.QuestAvailable,
				StartQuest = quest.StartQuest,
				CompleteQuest = quest.CompleteQuest,
				GotReward = quest.GotReward
			});
		}
	}
}

[System.Serializable]
public class QuestData
{
	//public string QuestName;
	public bool QuestAvailable;
	public bool StartQuest;
	public bool CompleteQuest;
	public bool GotReward;
}