using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class EnemyInfo : ScriptableObject
{
	public string enemyName;
	public bool isAlive;
	public int enemyLevel;
	public int maxHP;
	public int currentHP;
	public int damage;
	public int coinsHeld;
	public GameObject enemyPrefab;
    private string prefabName;

	//public EnemyStats[] FightEnemies;

    public EnemyInfo(string eName, bool alive, int lvl, int maxHealth, int curHealth, int dmg, string p_fabName)
    {
        enemyName = eName;
        isAlive = alive;
        enemyLevel = lvl;
        maxHP = maxHealth;
        currentHP = curHealth;
        damage = dmg;
        prefabName = p_fabName;
    }

	public bool TakeDamage(int dmg)
	{
		currentHP -= dmg;

		if(currentHP <= 0)
		{
			isAlive = false;
			return true;
		}
		else
		{
			return false;
		}
	}

	public void Heal(int amount)
	{
		currentHP += amount;
		if(currentHP > maxHP)
		{
			currentHP = maxHP;
		}
	}

	public void ResetHP()
	{
		Debug.Log($@"Reset the dead enemy [{enemyName}] hp to maxHP of: [{maxHP}]");
		currentHP = maxHP;
	}
}