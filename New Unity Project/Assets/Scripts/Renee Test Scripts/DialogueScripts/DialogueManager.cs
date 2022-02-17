using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Animator animator;

    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        Debug.Log("Starting conversation with " + dialogue.name);
        //nameText.text = dialogue.name;
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
        Debug.Log(sentences.Count);
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        Debug.Log(sentences.Count);
        string sentence = sentences.Dequeue();
        Debug.Log(sentence);
        dialogueText.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence) //types out the letters in sentence one by one
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray()) //loops the chars one by one
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(.02f);
        }
    }

    void EndDialogue() //ends dialogue
    {
        animator.SetBool("IsOpen", false); //closes dialogue box
        Debug.Log("End of dialogue");
    }
}
