using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
	public GameManager GM;

	public TMP_Text nameText;
	public TMP_Text levelText;
	public Slider hpSlider;

	public void SetHUD(PlayerInfo playerInfo)
	{
		GM = GameObject.Find("GameManager").GetComponent<GameManager>();

		nameText.text = playerInfo.unitName;
		levelText.text = "LV: " + playerInfo.unitLevel;
		hpSlider.maxValue = playerInfo.maxHP;
		hpSlider.value = playerInfo.currentHP;
	}

	public void SetEnemyHUD(EnemyInfo enemyUnit)
	{
		GM = GameObject.Find("GameManager").GetComponent<GameManager>();

		nameText.text = enemyUnit.enemyName;
		levelText.text = "LV: " + enemyUnit.enemyLevel;
		hpSlider.maxValue = enemyUnit.maxHP;
		hpSlider.value = enemyUnit.currentHP;
	}

	public void SetHP(int hp)
	{
		hpSlider.value = hp;
	}
}
