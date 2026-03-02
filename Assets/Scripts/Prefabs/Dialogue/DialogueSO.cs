using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum PortraitSide { Left, Right }

[CreateAssetMenu(fileName = "New Dialogue SO", menuName = "ScriptableObjects/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public List<DialogueLine> dialogueLines = new();

    [Header("Ending of Dialogue")]
    public UnityEvent OnDialogueEnd;

    public PortraitSide GetSide(Speaker speaker, PortraitSide fallback = PortraitSide.Left)
    {
        DialogueManager manager = DialogueManager.Instance;
        switch (speaker)
        {
            case Speaker.Boss:
                return manager.bossSO.defaultSide;
            case Speaker.Player:
                return manager.playerSO.defaultSide;
            default:
                return fallback;
        }
    }

    public DialogueLine GetLine(int index)
    {
        if (index < 0 || index >= dialogueLines.Count)
            return null;

        return dialogueLines[index];
    }

    public bool HasNextLine(int index)
    {
        return index >= -1 && index < dialogueLines.Count - 1;
    }
}

[Serializable]
public class DialogueLine
{
    public Speaker speaker;

    [TextArea(2, 6)]
    public string line;

    [Header("Dialogue Image")]
    public Sprite image;
    [Header("Dialogue Position and Scale. Leave at (0,0) and (1,1) for no change)")]
    public Vector2 position = Vector2.zero;
    public Vector2 scale = Vector2.one;

    public void ApplySpriteSettings()
    {
        DialogueSprite.Instance.SetSprite(image);
        if (position != Vector2.zero)
            DialogueSprite.Instance.SetPosition(position);
        if (scale != Vector2.one)
            DialogueSprite.Instance.SetScale(scale);
    }
}

public enum Speaker { Boss, Player };