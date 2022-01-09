using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinTextManager : MonoBehaviour
{
	public Inventory2 playerInventory;
	public TextMeshProUGUI coinDisplay;

	void Start()
	{
		playerInventory = GameObject.Find("ItemManager").GetComponent<Inventory2>();
	}

	public void UpdateCoinCount()
	{
		coinDisplay.text = "" + playerInventory.coins;
	}

	void Update()
	{
		coinDisplay.text = "" + playerInventory.coins;
	}
}
