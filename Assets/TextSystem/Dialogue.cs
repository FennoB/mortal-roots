using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    public string title = "";

    [TextArea(3, 10)] // makes text field appear more suitable in the unity inspector
    public string[] sentences;
    public Sprite sprite;
    public string mode;

    public static Dialogue Create(string mode = "", Sprite sprite = null, string title = "")
    {
        Dialogue p = new Dialogue();
        p.sentences = new string[0];
        p.mode = mode;
        p.sprite = sprite;
        p.title = title;
        return p;
    }

    public Dialogue Sentence(string sentence)
    {
        string wrapped = "";
        string[] parts = sentence.Split(' ');
        string line = parts[0];
        for (int p = 1; p < parts.Length; ++p)
        {
            string part = parts[p];
            if ((line + " " + part).Length <= DialogueWriter.main.lettersPerLine)
            {
                line += " " + part;
            }
            else
            {
                wrapped += line + "\n";
                line = part;
            }
        }
        wrapped += line;

        List<string> list = new List<string>(sentences);
        list.Add(wrapped);
        sentences = list.ToArray();
        return this;
    }

    public void Show()
    {
        DialogueWriter.main.Clear();
        DialogueWriter.main.StartDialogue(this);
    }
}
