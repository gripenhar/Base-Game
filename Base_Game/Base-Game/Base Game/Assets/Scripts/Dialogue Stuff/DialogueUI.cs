using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
	//[SerializeField] private GameObject dialogueBox;
	[SerializeField] private GameObject PortraitBG;
	[SerializeField] private TMP_Text PortraitText;
	[SerializeField] private GameObject NoPortraitBG;
	[SerializeField] private TMP_Text NoPortraitText;
	[SerializeField] private GameObject UIPortraitFrameObject;
	[SerializeField] private GameObject UIPortrait;
	//[SerializeField] private DialogueObject testDialogue;


	public bool IsOpen { get; private set; }

	private ResponseHandler responseHandler;
	private TypewriterEffect typewriterEffect;

	private void Start()
	{
		//PortraitText.text = "Hello!\nThis is my second line.";
		//GetComponent<TypewriterEffect>().Run("This is a bit of text!\nHello.", PortraitText);
		typewriterEffect = GetComponent<TypewriterEffect>();
		responseHandler = GetComponent<ResponseHandler>();

		CloseDialogueBox();
		//ShowDialogue(testDialogue);

	}

	public void ShowDialogue(DialogueObject dialogueObject)
	{
		IsOpen = true;
		PortraitBG.SetActive(true);
		UIPortraitFrameObject.SetActive(false);

		StartCoroutine(StepThroughDialogue(dialogueObject));
	}

	public void AddResponseEvents(ResponseEvent[] responseEvents)
	{
		responseHandler = GetComponent<ResponseHandler>();
		responseHandler.AddResponseEvents(responseEvents);

	}

	private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
	{
		for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
		{
			Dialogue dialogue = dialogueObject.Dialogue[i];

			if (dialogueObject.Dialogue[i].HasPortrait)
			{
				PortraitBG.SetActive(true);
				NoPortraitBG.SetActive(false);

				UIPortraitFrameObject.SetActive(true);
				UIPortrait.GetComponent<Image>().sprite = dialogueObject.Dialogue[i].Portrait.PortraitSprite;

				NoPortraitText.text = string.Empty;

				yield return RunTypingEffect(dialogue.dialogueText, PortraitText);
			}
			else
			{
				PortraitBG.SetActive(false);
				NoPortraitBG.SetActive(true);

				UIPortraitFrameObject.SetActive(false);

				PortraitText.text = string.Empty;

				yield return RunTypingEffect(dialogue.dialogueText, NoPortraitText);
			}
			
			///yield return RunTypingEffect(dialogue.dialogueText, );

			
			///textLabel.text = dialogue.dialogueText;
			
			

			if(i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;

			yield return null;
			yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
		}

		if (dialogueObject.HasResponses)
		{
			Debug.Log("DialogueUI is being asked to make buttons.");
			responseHandler.ShowResponses(dialogueObject.Responses);
		}
		else
		{
			CloseDialogueBox();
		}
	}

	private IEnumerator RunTypingEffect(string dialogue, TMP_Text textLabel)
	{
		typewriterEffect.Run(dialogue, textLabel);

		while(typewriterEffect.IsRunning)
		{
			yield return null;
			if(Input.GetKeyDown(KeyCode.Space))
			{
				typewriterEffect.Stop();
				textLabel.text = dialogue;
			}
		}
	}

	public void CloseDialogueBox()
	{
		IsOpen = false;
		//dialogueBox.SetActive(false);
		///
		PortraitText.text = string.Empty;
		NoPortraitText.text = string.Empty;

		///
		PortraitBG.SetActive(false);
		NoPortraitBG.SetActive(false);
		UIPortraitFrameObject.SetActive(false);
	}
}
