using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance; 

    public GameObject dialogueBox;
    public Image leftCharacterImage;
    public Image rightCharacterImage;
    public TMP_Text leftCharacterName;
    public TMP_Text rightCharacterName;
    public TMP_Text dialogueText;

    private Dialogue currentDialogue;
    private int dialogueIndex;
    private bool isDialogue;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        dialogueBox.SetActive(false);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueBox.SetActive(true);
        currentDialogue = dialogue;
        dialogueIndex = 0;

        leftCharacterImage.sprite = currentDialogue.character1Sprite;
        rightCharacterImage.sprite = currentDialogue.character2Sprite;
        leftCharacterName.text = currentDialogue.character1Name;
        rightCharacterName.text = currentDialogue.character2Name;

        DisplayNextLine();
        isDialogue = true;
        
    }

    private void Update()
    {
        if(isDialogue && Input.GetMouseButtonDown(0))
        {
            DisplayNextLine();
        }
    }
    public void DisplayNextLine()
    {
        
        if (dialogueIndex < currentDialogue.dialogueLines.Count)
        {
            InteractionManagar.instance.interacting = true;
            string line = currentDialogue.dialogueLines[dialogueIndex];
            dialogueText.text = line;

            if (dialogueIndex % 2 == 0)
            {
                // Highlight left character
                leftCharacterImage.color = Color.white;
                rightCharacterImage.color = Color.gray;
            }
            else
            {
                // Highlight right character
                leftCharacterImage.color = Color.gray;
                rightCharacterImage.color = Color.white;
            }

            dialogueIndex++;
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        isDialogue = false;
        InteractionManagar.instance.interacting = false;
        currentDialogue = null;
        dialogueBox.SetActive(false);
    }
}