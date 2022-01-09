using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]

public class Quest : ScriptableObject
{
 public string QuestNPCName;
 public bool QuestAvailable;
 public bool StartQuest;
 public DialogueObject StartQuestDialogue;
 public DialogueObject AcceptQuestDialogue;

 public List<EnemyInfo> Enemies;

 public bool CompleteQuest;
 public DialogueObject CompleteQuestDialogue;

 public bool GotReward;
 public DialogueObject GotRewardDialogue;

	public bool IsEveryEnemyDead()
	{
		foreach(EnemyInfo enemyinfo in Enemies)
		{
			if(enemyinfo.isAlive)
				return false;
		}
		return true;
	}


	public void ResetQuest()
	{
		QuestAvailable = false;
		StartQuest = false;
		CompleteQuest = false;
		GotReward = false;
	}
}
