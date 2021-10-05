using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class DialogueNode : DialogueBranch {

   
    public string npcName;
    public Sprite npcSprite;
    public bool startPoint;
    public Color nameColor;

    public override bool IsStartPoint()
    {
        return startPoint;
    }

    
    /*
    // Use this for initialization
    protected override void Init() {
		base.Init();
		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
    */
}