using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractorButton : MonoBehaviour
{
    public int row;
    public int col;
    public bool collected = false;

    public ExtractorController controller;

    public void Extract()
    {
        if (!collected)
        {
            controller.CollectResource(row, col);
            collected = true;
        }
    }
}
