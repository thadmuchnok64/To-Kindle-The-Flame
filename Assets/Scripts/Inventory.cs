using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    static public bool[] collectibles;

    private void Start()
    {
        collectibles = new bool[] { false,false,false,false,false};
    }

    public static void GetCollectible(int x)
    {
        collectibles[x - 1] = true;
    }
}
