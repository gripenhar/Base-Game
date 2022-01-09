using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_InputWindow : MonoBehaviour
{
	public TMP_InputField inputField;
	public static string username;

	public GameManager GM;

	void Start()
	{
		//Debug.Log(username);
		//if(username != null)
		//{
			inputField.text = username;
			GM = GameObject.Find("GameManager").GetComponent<GameManager>();
		//}
	}

	public void SaveUsername()
	{
		username = inputField.text;
		Debug.Log(username);
		GM.gameProgress.PlayerName = username;
		SceneManager.LoadScene("OpeningCutscene");
	}

	public void Show()
	{
		gameObject.SetActive(true);
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}

}
