using UnityEngine;

[System.Serializable]

public class Portrait
{
	[SerializeField] private Sprite portraitSprite;

	public Sprite PortraitSprite => portraitSprite;

}
