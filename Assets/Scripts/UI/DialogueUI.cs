using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.Events;

public class DialogueUI : MonoBehaviour
{
    [Serializable]
    struct DialogueLine
    {
        public string speaker;
        public string line;
    }

    [Serializable]
    struct DialogueEvent
    {
        public int lineIndex;
        public UnityEvent onLineReached;
    }

    [SerializeField] List<DialogueLine> allDialogueLines;
    [SerializeField] List<DialogueEvent> allDialogueEvents;
    [SerializeField] TextAsset dialogueFile;

    [SerializeField] bool typewriterEffect = true;
    [SerializeField] float typewriterSpeed = 0.05f;

    [SerializeField] TextMeshProUGUI speakerName;
    [SerializeField] TextMeshProUGUI speakerLine;
    int currentLineIndex = -1;

    [SerializeField] UnityEvent onDialogueEnd;
    [SerializeField] UnityEvent onDialogueStart;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (dialogueFile == null)
        {
            Debug.LogError("Dialogue file not assigned!");
        }
        else
        {
            string[] allLines = dialogueFile.text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            allDialogueLines.Clear();
            foreach (string l in allLines)
            {
                string[] split = l.Split(new[] { ':' }, 2);
                if (split.Length < 2)
                    continue;

                DialogueLine line = new DialogueLine();
                line.speaker = split[0].Trim();
                line.line = split[1].Trim();
                allDialogueLines.Add(line);
            }
        }

        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDialogue()
    {
        currentLineIndex = -1;
        NextLine();
    }

    public void NextLine()
    {
        if (currentLineIndex >= allDialogueLines.Count - 1)
        {
            onDialogueEnd?.Invoke();
            return;
        }

        currentLineIndex++;
        if (currentLineIndex >= allDialogueLines.Count)
        {
            return;
        }

        if (typewriterEffect)
        {
            StartCoroutine(DisplayNextLine());
        }
        else
        {
            DialogueLine line = GetLine();
            speakerName.text = line.speaker;
            speakerLine.text = line.line;
        }
    }

    DialogueLine GetLine()
    {
        return allDialogueLines[currentLineIndex];
    }

    IEnumerator NextLineAfterDelay(float delay)
    {
        yield return new WaitForSeconds(typewriterSpeed);
        NextLine();
    }

    IEnumerator DisplayNextLine()
    {
        yield return new WaitForEndOfFrame();

        DialogueLine line = GetLine();
        speakerLine.text = "";

        //if (currentLineIndex == mountainIndexStart)
        //{
        //    CinematicCamera camera = FindFirstObjectByType<CinematicCamera>();
        //    if (camera)
        //    {
        //        preMountainLocation = camera.transform.position;
        //        camera.LerpToPosition(mountainLocation, 2f);
        //    }
        //}
        //else if (currentLineIndex == mountainIndexEnd)
        //{
        //    CinematicCamera camera = FindFirstObjectByType<CinematicCamera>();
        //    if (camera)
        //    {
        //        camera.LerpToPosition(preMountainLocation, 2f);
        //    }
        //}

        if (allDialogueEvents != null)
        {
            foreach (DialogueEvent dialogueEvent in allDialogueEvents)
            {
                if (dialogueEvent.lineIndex == currentLineIndex)
                {
                    dialogueEvent.onLineReached?.Invoke();
                }
            }
        }

        for (int i = 0; i < line.line.Length; i++)
        {
            speakerName.text = line.speaker;
            speakerLine.text += line.line[i];
            yield return new WaitForSeconds(typewriterSpeed);
        }

        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(3);
        NextLine();
    }
}
