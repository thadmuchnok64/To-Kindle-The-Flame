using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class CoreNode : Node {
    /*
	public virtual string GetString()
    {
        return "haha node go brr";
    }
    public virtual Sprite GetSprite()
    {
        return null;
    }
    */
    public virtual bool IsStartPoint()
    {
        return false;
    }
    public virtual bool IsEndPoint()
    {
        return false;
    }
}