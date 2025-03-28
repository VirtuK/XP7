using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue : MonoBehaviour
{
    public Sprite character1Sprite;
    public Sprite character2Sprite;
    public string character1Name;
    public string character2Name;
    [TextArea(3, 6)] public List<string> dialogueLines;
}
