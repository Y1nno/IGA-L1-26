using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text speakerNameText;

    private DialogueSO currentDialogue;
    private int currentDialogueIndex;
    public bool IsDialogueActive => currentDialogue != null;

    private Dictionary<DialogueInstances, DialogueSO> dialogues;
    public List<DialogueSOWithEnum> dialogueList;

    public SpeakerSO playerSO;
    public SpeakerSO bossSO;
    public SpeakerSO NarratorSO;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        dialogues = new Dictionary<DialogueInstances, DialogueSO>();
        foreach (var item in dialogueList)
        {
            if (!dialogues.ContainsKey(item.instance))
            {
                dialogues.Add(item.instance, item.dialogue);
            }
        }
    }

    public void StartDialogue(DialogueSO dialogue)
    {
        dialogueUI.SetActive(true);
        currentDialogue = dialogue;
        GlobalGameManager.Instance.TogglePauseGame(true);
        currentDialogueIndex = -1; // Initialize to -1 to start from the first line at index 0 when checking for next line and incrementing
        DisplayNextLine();
    }

    public void StartDialogue(DialogueInstances instance)
    {
        if (dialogues.TryGetValue(instance, out DialogueSO dialogue))
        {
            StartDialogue(dialogue);
        }
        else
        {
            Debug.LogWarning($"Dialogue instance {instance} not found in the dictionary.");
        }
    }

    public void EndDialogue()
    {
        dialogueUI.SetActive(false);
        currentDialogue.OnDialogueEnd?.Invoke();
        currentDialogue = null;
        DialogueSprite.Instance.ClearSprite();
        GlobalGameManager.Instance.TogglePauseGame(false);
    }

    public void DisplayNextLine()
    {
        if (currentDialogue == null)
        {
            Debug.LogWarning("No dialogue is currently active.");
            return;
        }

        if (!currentDialogue.HasNextLine(currentDialogueIndex))
        {
            EndDialogue();
            return;
        }
        currentDialogueIndex++;
        // Logic to display the next line of dialogue
        DialogueLine line = currentDialogue.GetLine(currentDialogueIndex);
        if (line != null)
        {
            dialogueText.text = line.line;
            speakerNameText.text = GetSOFromEnum(line.speaker).speakerName;
            line.ApplySpriteSettings();
        }
    }

    public SpeakerSO GetSOFromEnum(Speaker speaker)
    {
        switch (speaker)
        {
            case Speaker.Player:
                return playerSO;
            case Speaker.Boss:
                return bossSO;
            case Speaker.None:
                return NarratorSO;
            default:
                Debug.LogWarning("Unknown speaker type.");
                return null;
        }
    }
}

public enum DialogueInstances
{
    TutorialFinish,
    Level1Finish,
}

[System.Serializable]
public struct DialogueSOWithEnum
{
    public DialogueInstances instance;
    public DialogueSO dialogue;
}