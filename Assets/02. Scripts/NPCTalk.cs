using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/NPC Talk")]
public class NPCTalk : ScriptableObject
{
    [TextArea(2, 5)]
    public string[] dialogueLines;
}
