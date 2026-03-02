using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueContinueHookup : MonoBehaviour
{
    public void ContinueDialogue(InputAction.CallbackContext context)
    {
        if (context.performed && DialogueManager.Instance.IsDialogueActive)
        {
            DialogueManager.Instance.DisplayNextLine();
        }
    }
}
