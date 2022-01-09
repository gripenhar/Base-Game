using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
	public Enemy[] enemies;
	public pot[] pots;
	public GameObject virtualCamera;

	public virtual void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Player") && !other.isTrigger)
		{
			//Activate all enemies and pots
			for(int i = 0; i < enemies.Length; i++)
			{
				if(enemies[i].enemyInfo != null)
				{
					if(enemies[i].enemyInfo.isAlive)
					{
						ChangeActivation(enemies[i], true);
						//GameManager GM = GameObject.Find("GameManager").GetComponent<GameManager>();
						//GM.currentEnemyIndex = i;
						//GM.currentRoomName = this.name;
					}
					else
					{
						ChangeActivation(enemies[i], false);
					}
				}
				else
				{
					// null enemyInfo
				}
			}
			for(int i = 0; i < pots.Length; i++)
			{
				ChangeActivation(pots[i], true);
			}
			virtualCamera.SetActive(true);
		}
	}

	public virtual void OnTriggerExit2D(Collider2D other)
	{
		if(other.CompareTag("Player") && !other.isTrigger)
		{
			//Deactivate all enemies and pots
			for(int i = 0; i < enemies.Length; i++)
			{
				ChangeActivation(enemies[i], false);
			}
			for(int i = 0; i < pots.Length; i++)
			{
				ChangeActivation(pots[i], false);
			}
			virtualCamera.SetActive(false);
		}
	}

	public void OnDisable()
	{
		virtualCamera.SetActive(false);
	}

	public void ChangeActivation(Component component, bool activation)
	{
		component.gameObject.SetActive(activation);
	}
}
