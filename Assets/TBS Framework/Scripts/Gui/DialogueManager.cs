using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public Image speakerImage1;
    public Image speakerImage2;
    public Text speakerNameText1;
    public Text speakerNameText2;
    public Text dialogueText;

    public float typeWriteSpeed = 0.05f;

    private string[] dialogueLines;
    private int currentLineIndex;

    private void Start()
    {
        // Initialize the dialogue lines (you can replace this with your own dialogue system)
        dialogueLines = new string[]
        {
            "Speaker1:Hello! How are you doing today?",
            "Speaker2:I'm doing great, thanks for asking!",
            "Speaker1:That's wonderful to hear.",
            "Speaker2:So, what brings you here?",
            // Add more dialogue lines as needed
        };

        currentLineIndex = 0;
        StartDialogue();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (dialogueText.text == dialogueLines[currentLineIndex])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[currentLineIndex];
            }
        }
    }

    private void StartDialogue()
    {
        currentLineIndex = 0;
        DisplayLine();
    }

    private void DisplayLine()
    {
        string line = dialogueLines[currentLineIndex];
        string[] parts = line.Split(':');

        if (parts.Length == 2)
        {
            string speakerName = parts[0];
            string dialogue = parts[1];

            // Set the speaker's image and name based on the speaker's name
            if (speakerName == "Speaker1")
            {
                speakerImage1.gameObject.SetActive(true);
                speakerImage2.gameObject.SetActive(false);
                speakerNameText1.text = speakerName;
            }
            else if (speakerName == "Speaker2")
            {
                speakerImage1.gameObject.SetActive(false);
                speakerImage2.gameObject.SetActive(true);
                speakerNameText2.text = speakerName;
            }

            StartCoroutine(TypewriteText(dialogue));
        }
    }

    private void NextLine()
    {
        currentLineIndex++;

        if (currentLineIndex < dialogueLines.Length)
        {
            DisplayLine();
        }
        else
        {
            // End of dialogue
            // Add any desired behavior or cleanup here
        }
    }

    private IEnumerator TypewriteText(string text)
    {
        dialogueText.text = "";
        foreach (char c in text.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeWriteSpeed);
        }
    }
}