using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
	[SerializeField] private GameObject dialogueBox;
	[SerializeField] private TMP_Text textLabel;
	//[SerializeField] private DialogueObject testDialogue;

	public bool IsOpen { get; private set; }

	private ResponseHandler responseHandler;
	private TypewriterEffect typewriterEffect;

	private void Start()
	{
	//textLabel.text = "Hello!\nThis is my second line.";
	//GetComponent<TypewriterEffect>().Run("This is a bit of text!\nHello.", textLabel);
	typewriterEffect = GetComponent<TypewriterEffect>();
	responseHandler = GetComponent<ResponseHandler>();

	CloseDialogueBox();
	//ShowDialogue(testDialogue);

	}

	public void ShowDialogue(DialogueObject dialogueObject)
	{
		IsOpen = true;
		dialogueBox.SetActive(true);
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
			string dialogue = dialogueObject.Dialogue[i];

			yield return RunTypingEffect(dialogue);

			textLabel.text = dialogue;

			if(i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;

			yield return null;
			yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
		}

		if (dialogueObject.HasResponses)
		{
			responseHandler.ShowResponses(dialogueObject.Responses);
		}
		else
		{
				CloseDialogueBox();
		}
	}

	private IEnumerator RunTypingEffect(string dialogue)
	{
		typewriterEffect.Run(dialogue, textLabel);

		while(typewriterEffect.IsRunning)
		{
			yield return null;
			if(Input.GetKeyDown(KeyCode.Space))
			{
				typewriterEffect.Stop();
			}
		}
	}

	public void CloseDialogueBox()
	{
		IsOpen = false;
		dialogueBox.SetActive(false);
		textLabel.text = string.Empty;
	}
}
