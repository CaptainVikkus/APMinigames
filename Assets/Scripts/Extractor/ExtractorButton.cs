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
        switch (controller.mode)
        {
            case ExtractorMode.EXTRACTION:
                if (!collected)
                {
                    controller.CollectResource(row, col);
                    collected = true;
                }
                break;
            case ExtractorMode.SCANNING:
                controller.ScanResource(row, col, 3.0f);
                break;
            case ExtractorMode.COMPLETE:
                break;
            default:
                break;
        }
    }

}
