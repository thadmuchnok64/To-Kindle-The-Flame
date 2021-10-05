using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI[] choiceButtons;
    //[SerializeField] TextMeshProUGUI exitButton;
    [SerializeField] string nameColor;
    //[SerializeField] string name;
    [SerializeField] string choiceColor;
    [SerializeField] string oldColor;
    private Coroutine charCoroutine;
    Queue<char> charQueue;
    Stack<Dialogue> dialogues;
    //[SerializeField] AudioClip typeSound, lineSound;
    [SerializeField] Animator anim;
    public bool processRunning;
    //[SerializeField] Animator characterAnim;
    //[SerializeField] AudioSource aud;

    // xNode stuff below this

    public DialogueGraph graph;
    private void Start()
    {
        processRunning = false;
    }
    #region Singleton Pattern
    public static DialogueManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one instance of Dialogue Manager!!! Fix this ya bozo!");
            return;
        }
        instance = this;
    }

    #endregion
    public void StartDialogue (Dialogue d,string name)
    {
        anim.SetBool("Active", true);
        processRunning = true;
        charQueue = new Queue<char>();
        dialogues = new Stack<Dialogue>();
        charQueue.Clear();
        dialogues.Clear();
        dialogues.Push(d);
        dialogueText.text = "<color=#" + nameColor + ">" + name + ":</color> ";
        StartCoroutine(InitiateDialogueDelay());
    }

    public void StartDialogue(DialogueGraph dg )
    {
        charQueue = new Queue<char>();
        charQueue.Clear();
        graph = dg;
        foreach( DialogueBranch node in graph.nodes)
        {
            if (node.IsStartPoint())
            {
                graph.currentNode = node;
                break;
            }
        }
        DialogueNode dn = graph.currentNode as DialogueNode;
        anim.SetBool("Active", true);
        name = dn.npcName;
        dialogueText.text = "<color=#" + ColorUtility.ToHtmlStringRGBA(dn.nameColor) + ">" + dn.npcName + ":</color> ";
        StartCoroutine(InitiateDialogueDelay());
    }

    private IEnumerator InitiateDialogueDelay()
    {
        yield return new WaitForSeconds(.3f);
        IterateDialogue();
    }
    private void IterateDialogue()
    {

        if (graph.currentNode==null||graph.currentNode.IsEndPoint())
        {
            Exit();
        }
        char[] chars =  graph.currentNode.response.ToCharArray();
        foreach (char c in chars)
        {
            charQueue.Enqueue(c);
        }

        charCoroutine = StartCoroutine(CharScroll());
        for (int y = 0; y < 4; y++)
        {
            choiceButtons[y].gameObject.SetActive(false);
        }
        int x = 0;
        foreach( XNode.NodePort np in graph.currentNode.Outputs)
        {
            if (np.Connection == null)
            {
                break;
            }
            DialogueBranch db = np.Connection.node as DialogueBranch;
            choiceButtons[x].gameObject.SetActive(true);
            string[] split = db.answer.Split('<');
            if (split.Length == 1)
            {
                choiceButtons[x].text = db.answer;
            }
            else {
                choiceButtons[x].text = FixChoiceString(split);
            }

            x++;
        }


        //StartCoroutine(FixChoiceBoxes());
      
    }

    private string FixChoiceString(string[] split)
    {
        for(int x = 0; x < split.Length;x++)
        {
            if (split[x]=="characterName")
            {
                split[x] = "Oliver"; //TEMPORARY. Change this so that it checks what name your character is.
            }
        }
        string finalString="";
        foreach(string s in split)
        {
            finalString += s;
        }
        return finalString;
    }

    private IEnumerator FixChoiceBoxes()
    {
        yield return new WaitForSeconds(.01f);
        RectTransform last;
        for (int y = 1; y < choiceButtons.Length; y++)
        {
            last = choiceButtons[y - 1].GetComponent<RectTransform>();
            choiceButtons[y].GetComponent<RectTransform>().anchoredPosition = new Vector2(last.anchoredPosition.x, last.anchoredPosition.y  -30f - (last.rect.height / 1.5f));
        }
    }

    public void Exit()
    {
        processRunning = false;
        anim.StopPlayback();
        anim.SetBool("Active", false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)&&charQueue!=null)
        {
            if(charCoroutine!=null)
            StopCoroutine(charCoroutine);
            while (charQueue.Count > 0)
            {
                char c = charQueue.Dequeue();
                dialogueText.text += c;
            }
        }
    }
    private void ClearCharQueue()
    {
        if(charCoroutine!=null)
        StopCoroutine(charCoroutine);
        while (charQueue!=null && charQueue.Count > 0)
        {
            char c = charQueue.Dequeue();
            dialogueText.text += c;
        }
        dialogueText.text = "<color=#"+oldColor+">"+ dialogueText.text+"</color>\n\n";
      //  if(SoundMaster.soundOn)
        //aud.PlayOneShot(lineSound);
    }

    /*
    public void ReturnToHub(int x)
    {
        int y = 0;
        while (dialogues.Count > 0)
        {
            if (dialogues.Peek().isHub == true)
            {
                y++;
                if (y >= x)
                {
                    return;
                }
            }
                dialogues.Pop();
        }
        Debug.Log("Reached the beginning of the dialogue branch, and met no hub");
    }
    */
    public void SelectOutput(int i)
    {
        int x = 0;
        DialogueBranch db = null;
        foreach (XNode.NodePort np in graph.currentNode.Outputs)
        {
            if (np.Connection == null)
            {
                break;
            }
            db = np.Connection.node as DialogueBranch;
            if (x == i)
            {
                break;
            }
            x++;
        }
        graph.currentNode = db;
    }


    public void Choice(int choiceNumber)
    {
        dialogueText.text = "";
        ClearCharQueue();
        SelectOutput(choiceNumber);
        //dialogueText.text += "<align=right><color=#" + choiceColor + ">" + graph.currentNode.answer + "</color></align>\n\n";
        //characterAnim.Play(name+graph.currentNode.emotion);
        IterateDialogue();
    }
 
    private IEnumerator CharScroll()
    {
        char brackStart = '<';
        char brackEnd = '>';

        while (charQueue.Count>0)
        {
         //   if (charQueue.Count % 3 == 0)
         //   {
         //       aud.PlayOneShot(typeSound);
         //   }
            char c = charQueue.Dequeue();
           
                dialogueText.text += c;
            
            if (c == brackStart)
            {
                while (c != brackEnd)
                {
                    c = charQueue.Dequeue();
                    dialogueText.text += c;
                }
            }
            yield return new WaitForSeconds(.0135f);
        }
    }
}
