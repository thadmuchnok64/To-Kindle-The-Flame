using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DialogueChoice : MonoBehaviour
{
    // Start is called before the first frame update
    public bool makeFrown, makeContent, makeNeutral, makeShocked, makeVisible,closeDialogue;
    public string thisText;
    public GameObject[] choices;
    public string[] skillReq; public int[] skillValueReq;
        public string characterDialogue;
        private Text DialogueOnScreen;
    private Text myText;
    private Transform par;
    public bool qualified;
    public bool hasPause;
    private GameObject window;
    //private CharacterArt character;

        void Awake()
    {
        window = transform.parent.gameObject;
      //  character = GameObject.Find("RipleyArt").GetComponent<CharacterArt>();
        
        qualified = true;
        par = transform.parent;
        myText = GetComponent<Text>();
        myText.text = thisText;
        DialogueOnScreen = GameObject.Find("DialogueBox").GetComponent<Text>();
        /*
        if (characterDialogue.Contains("(guy/girl)"))
        {
            if (p.isMale)
            {
              characterDialogue =  characterDialogue.Replace("(guy/girl)", "guy");
            }
            else
            {
                characterDialogue = characterDialogue.Replace("(guy/girl)", "girl");
            }
        }
        */
        if (skillReq.Length > 0)
        {
            GetComponent<Button>().interactable = false;
            qualified = false;
            string s;
            for(int x = 0; x<skillReq.Length; x++)
            {
                /*
                if ((int)p.GetType().GetField(skillReq[x]).GetValue(p) >= skillValueReq[x])
                {
                    s = skillReq[x];
                    s = char.ToUpper(s[0]) + s.Substring(1);
                    myText.text = GetComponent<Text>().text + " [" + s + "]";
                    GetComponent<Button>().interactable = true;
                    qualified = true;
                    
                }*/

            }

        }
    }

    public IEnumerator ShiftWindowRight()
    {
        Vector3 pos = window.transform.position;
        pos = new Vector3(pos.x + 2, pos.y, pos.z);
        for (int x = 0; x < 100; x++)
        {
            window.transform.position = new Vector3((pos.x + window.transform.position.x * 9) / 10, (pos.y + window.transform.position.y * 9) / 10, window.transform.position.z);
            yield return new WaitForSeconds(.0175f);
        }
    }

    public IEnumerator CloseDialogueWindow()
    {
        SpriteRenderer windowRenderer = window.GetComponent<SpriteRenderer>();
        SpriteRenderer overlay = GameObject.Find("DialogueOverlay").GetComponent<SpriteRenderer>();
        Vector3 pos = window.transform.position;
        overlay.color = new Color(1, 1, 1, 1);
        pos = new Vector3(pos.x + 15, pos.y, pos.z);

        windowRenderer.color = new Color(1, 1, 1, 1);
       // p.lockMovement = false;
        yield return new WaitForSeconds(.01f);
     //   character.StartCoroutine(character.TransitionToInvisible());
        for (int x = 0; x < 100; x++)
        {
            window.transform.position = new Vector3((pos.x + window.transform.position.x * 9) / 10, (pos.y + window.transform.position.y * 9) / 10, window.transform.position.z);
            windowRenderer.color = new Color(windowRenderer.color.r, windowRenderer.color.g, windowRenderer.color.b, windowRenderer.color.a - .01f);
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, overlay.color.a - .01f);
            yield return new WaitForSeconds(.0175f);
        }
       
        Destroy(gameObject);
    }

    public IEnumerator OpenDialogueWindow()
    {
        SpriteRenderer windowRenderer = window.GetComponent<SpriteRenderer>();
        SpriteRenderer overlay = GameObject.Find("DialogueOverlay").GetComponent<SpriteRenderer>();
        Vector3 pos = window.transform.position;
        pos = new Vector3(0, 0, pos.z);
        windowRenderer.color = new Color(1, 1, 1, 0);
        for (int x = 0; x < 100; x++)
        {
            window.transform.position = new Vector3((pos.x + window.transform.position.x * 9) / 10, (pos.y + window.transform.position.y * 9) / 10, window.transform.position.z);
            windowRenderer.color = new Color(windowRenderer.color.r, windowRenderer.color.g, windowRenderer.color.b, windowRenderer.color.a + .01f);
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, overlay.color.a + .01f);

            yield return new WaitForSeconds(.0175f);
        }

    }

    public void ChooseDialogueOption()
    {
        if (makeVisible)
        {
        //    character.becomeVisible();
            StartCoroutine(ShiftWindowRight());
        }
        if (makeContent)
        {
         //   character.makeContent();
        }
        if (makeFrown)
        {
          //  character.makeFrown();
        }
        if (makeNeutral)
        {
         //   character.makeNeutral();
        }
        if (makeShocked)
       {
          //  character.makeShocked();
        }
    //    DialogueOnScreen.text =  DialogueOnScreen.text + "\n\n\t";
        if (thisText != "Continue")
        {
            DialogueOnScreen.text+="<color=#34abef>" + thisText + "</color> \n\n";
        }
        GameObject dia;
        float offset = 0;
        GameObject[] oldOnes= GameObject.FindGameObjectsWithTag("Dialogue");
        for(int x = 0; x <oldOnes.Length; x++)
        {

            if (oldOnes[x] != this.gameObject)
            {
                Destroy(oldOnes[x]);
            }

        }
        if (!hasPause)
        {
            int y = 0;
            for (int x = 0; x < choices.Length; x++)
            {

                dia = Instantiate(choices[x], par, true);
                dia.GetComponent<RectTransform>().anchoredPosition = new Vector2(.4f, (-2.0f - (x - y) * .8f) - (offset * .6f));
                dia.GetComponent<RectTransform>().localScale = new Vector3(.025f, .025f, 1);
                Canvas.ForceUpdateCanvases();
                if (dia.GetComponent<DialogueChoice>().qualified == true)
                {
                    offset += dia.GetComponent<Text>().cachedTextGenerator.lines.Count;
                    offset -= 1;
                }
                else
                {
                    Destroy(dia);
                    y++;
                }

            }
        }
        GetComponent<Text>().text = "";
        GetComponent<Button>().enabled = false;

        StartCoroutine(WriteToTextBox());
    }

    private IEnumerator WriteToTextBox()
    {

        if (hasPause)
        {
            yield return new WaitForSeconds(3);
        }
        bool isClosing = false;
        for(int i = 0; i<characterDialogue.Length; i++)
        {
            //short pause : @
            if (characterDialogue[i] != '@'&& characterDialogue[i] != '*')
            {
                if (isClosing)
                {
                    DialogueOnScreen.text = DialogueOnScreen.text.Substring(0, DialogueOnScreen.text.Length - 8);
                }
                DialogueOnScreen.text = DialogueOnScreen.text + characterDialogue[i];
                if (isClosing)
                {
                    DialogueOnScreen.text += "</color>";
                }
            } else if (characterDialogue[i] == '*' && isClosing == false)
            {
            isClosing = true;
                DialogueOnScreen.text += "<color=#EFA310>*</color>";
            }else if (characterDialogue[i] == '*' && isClosing)
            {
            isClosing = false;
                DialogueOnScreen.text = DialogueOnScreen.text.Substring(0, DialogueOnScreen.text.Length - 8);

                DialogueOnScreen.text += "*</color>";
        }
            else
            {
                yield return new WaitForSeconds(1);
                DialogueOnScreen.text = DialogueOnScreen.text + "\n\n";

            }
            yield return new WaitForSeconds(.005f);
        }
        if (closeDialogue)
        {
            yield return new WaitForSeconds(.75f);
            StartCoroutine(CloseDialogueWindow());

        }
        if (hasPause)
        {

            GameObject dia;
            float offset = 0;
            int y = 0;
            for (int x = 0; x < choices.Length; x++)
            {

                dia = Instantiate(choices[x], par, true);
                dia.GetComponent<RectTransform>().anchoredPosition = new Vector2(.4f, (-2.0f - (x - y) * .8f) - (offset * .6f));
                dia.GetComponent<RectTransform>().localScale = new Vector3(.025f, .025f, 1);
                Canvas.ForceUpdateCanvases();
                if (dia.GetComponent<DialogueChoice>().qualified == true)
                {
                    offset += dia.GetComponent<Text>().cachedTextGenerator.lines.Count;
                    offset -= 1;
                }
                else
                {
                    Destroy(dia);
                    y++;
                }

            }
        }
        if(!closeDialogue)
        Destroy(gameObject);
    }

}
