using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    //public string name;
    public DialogueGraph dialogueGraph;

    public void TriggerDialogue(Collider2D collision)
    {
        if (!DialogueManager.instance.processRunning && Input.GetKeyDown(KeyCode.E) && collision.gameObject.tag == "Dialogue")
        {
            DialogueManager.instance.StartDialogue(dialogueGraph);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TriggerDialogue(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerDialogue(collision);
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            TriggerDialogue();
        }
    }
    */
}
