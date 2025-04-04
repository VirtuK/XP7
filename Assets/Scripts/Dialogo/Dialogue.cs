using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [TextArea(3, 6)]
    public string text;
    [Range(1, 2)]
    public int speakerIndex; // 1 for left, 2 for right
}

[System.Serializable]
public class Dialogue : MonoBehaviour
{
    public Sprite character1Sprite;
    public Sprite character2Sprite;
    public string character1Name;
    public string character2Name;
    public List<DialogueLine> dialogueLines;
}