
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]

public class DialogueObject : ScriptableObject
{
	//[SerializeField] [TextArea] private string[] dialogue;
	[SerializeField] public Dialogue[] dialogue;
	[SerializeField] private Response[] responses;
	//[SerializeField] private Portrait[] portraits;

	
	//public bool HasPortraits => Portraits != null && Portraits.Length > 0;
	//public Portrait[] Portraits => portraits;

	public Dialogue[] Dialogue => dialogue;

	public bool HasResponses => Responses != null && Responses.Length > 0;
	public Response[] Responses => responses;
}
