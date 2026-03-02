using UnityEngine;

[CreateAssetMenu(fileName = "New Speaker SO", menuName = "ScriptableObjects/Speaker")]
public class SpeakerSO : ScriptableObject
{
    public string speakerName;
    public Sprite speakerPortrait;
    public PortraitSide defaultSide;
}
