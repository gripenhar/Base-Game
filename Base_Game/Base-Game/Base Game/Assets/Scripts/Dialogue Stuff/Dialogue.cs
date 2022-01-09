
using UnityEngine;

[System.Serializable]

public class Dialogue
{
	[SerializeField] [TextArea] public string dialogueText;
	[SerializeField] private Portrait portrait;

	public bool HasPortrait => portrait.PortraitSprite != null;
	public Portrait Portrait => portrait;


}
