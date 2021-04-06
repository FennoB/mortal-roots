using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueWriter : MonoBehaviour
{
    // references to UI elements
    public static DialogueWriter main = null;
    public float lettersPerSecond = 20;
    public int lettersPerLine = 12;
    
    //public TMPro.TMP_Text title;
    public TMPro.TMP_Text text;

    private Queue<Dialogue> dialogues;
    private Queue<string> sentences;

    bool waitForSpaceReleased = false;
    public bool writing = false;

    void Start()
    {
        main = this;
        dialogues = new Queue<Dialogue>();
        sentences = new Queue<string>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (writing)
        {
            if (waitForSpaceReleased)
            {
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    waitForSpaceReleased = false;
                }
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                DisplayNextSentence();
            }
        }
    }

    public void StartWriting(Dialogue[] ds)
    {
        dialogues.Clear(); // clearing Queue for tidying up

        foreach (Dialogue d in ds)
        {
            dialogues.Enqueue(d);
        }

        // starting 
        Dialogue first = dialogues.Dequeue();
        StartDialogue(first);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        waitForSpaceReleased = Input.GetKey(KeyCode.Space);
        writing = true;
        gameObject.SetActive(true);
        // updating UI
        //title.text = dialogue.name;
        sentences.Clear();

        // putting all sentences of the dialogue in the queue
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        // dialogue is over if no more sentences need to be displayed
        if (sentences.Count == 0)
        {
            StopAllCoroutines();
            EndDialogue();
            return; // ends function with returning no value
        }

        string nextSentence = sentences.Dequeue(); // gets next sentence to display from queue
        StopAllCoroutines(); // if type sentence is already running when player clicks contiue button, it must be stopped
        StartCoroutine(TypeSentence(nextSentence)); // types sentence to screen
    }

    // coroutine that writes one by one character of a sentence to screen
    private IEnumerator TypeSentence(string sentence)
    {
        text.text = "";
        foreach (char letter in sentence.ToCharArray()) // converting sentence to char array
        {
            text.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
    }

    // this method can also start a new dialogue
    public void EndDialogue()
    {
        if (dialogues.Count != 0)
        {
            Dialogue next = dialogues.Dequeue();
            StartDialogue(next);
        }
        else
        {
            Clear();
        }
    }

    public void Clear()
    {
        StopAllCoroutines();
        dialogues.Clear();
        sentences.Clear();
        writing = false;
        // tidy up name and text to clear screen if this is the last dialogue
        //title.text = "";
        text.text = "";
        gameObject.SetActive(false);
    }
}
