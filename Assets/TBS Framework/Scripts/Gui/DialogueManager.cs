using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class Speaker
    {
        public GameObject bust;
        public string name;
    }

    public Speaker speaker1;
    public Speaker speaker2;

    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueBox;

    public float typeWriteSpeed = 0.05f;
    public float fadeDuration = 0.5f;

    [System.Serializable]
    public class DialogueLine
    {
        public int speakerIndex;
        [TextArea(3, 10)]
        public string dialogue;
    }

    public DialogueLine[] dialogueLines;

    private int currentLineIndex;
    private Speaker currentSpeaker;
    private Speaker nextSpeaker;

    public event EventHandler DialogueEnded;

    private void Start()
    {
        currentLineIndex = 0;
        SetDialogueBoxAlpha(0f);
        SetBustAlpha(speaker1.bust, 0f);
        SetBustAlpha(speaker2.bust, 0f);
        StartDialogue();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (dialogueText.text == dialogueLines[currentLineIndex].dialogue)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[currentLineIndex].dialogue;
            }
        }
    }

    private void StartDialogue()
    {
        currentLineIndex = 0;
        StartCoroutine(FadeDialogueBox(1f));
        DisplayLine();
    }

    private void DisplayLine()
    {
        DialogueLine line = dialogueLines[currentLineIndex];

        // Determine the current and next speakers based on the speakerIndex
        if (line.speakerIndex == 1)
        {
            currentSpeaker = speaker1;
            nextSpeaker = speaker2;
        }
        else if (line.speakerIndex == 2)
        {
            currentSpeaker = speaker2;
            nextSpeaker = speaker1;
        }

        // Set the speaker's name
        speakerNameText.text = currentSpeaker.name;

        // Fade in the current speaker's bust
        StartCoroutine(FadeBust(currentSpeaker.bust, 1f));

        // Fade out the next speaker's bust if it was previously visible
        if (GetBustAlpha(nextSpeaker.bust) > 0f)
        {
            StartCoroutine(FadeBust(nextSpeaker.bust, 0f));
        }

        // Fade in the dialogue box
        StartCoroutine(FadeDialogueBox(1f));

        StartCoroutine(TypewriteText(line.dialogue));
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
            StartCoroutine(FadeBust(currentSpeaker.bust, 0f));
            StartCoroutine(FadeDialogueBox(0f));

            // Invoke the DialogueEnded event
            DialogueEnded?.Invoke(this, EventArgs.Empty);
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

    private IEnumerator FadeBust(GameObject bust, float targetAlpha)
    {
        float startAlpha = GetBustAlpha(bust);
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            SetBustAlpha(bust, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetBustAlpha(bust, targetAlpha);
    }

    private float GetBustAlpha(GameObject bust)
    {
        CanvasGroup canvasGroup = bust.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            return canvasGroup.alpha;
        }
        return 1f;
    }

    private void SetBustAlpha(GameObject bust, float alpha)
    {
        CanvasGroup canvasGroup = bust.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = alpha;
        }
    }

    private IEnumerator FadeDialogueBox(float targetAlpha)
    {
        CanvasGroup canvasGroup = dialogueBox.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = dialogueBox.AddComponent<CanvasGroup>();
        }

        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }

    private float GetDialogueBoxAlpha()
    {
        CanvasGroup canvasGroup = dialogueBox.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            return canvasGroup.alpha;
        }
        return 1f;
    }

    private void SetDialogueBoxAlpha(float alpha)
    {
        CanvasGroup canvasGroup = dialogueBox.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = alpha;
        }
    }
}