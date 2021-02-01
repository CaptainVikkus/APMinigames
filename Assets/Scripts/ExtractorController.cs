using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExtractorMode
{
    EXTRACTION,
    SCANNING
}

public class ExtractorController : MonoBehaviour
{
    public ExtractorMode mode;
    public int resourceTotal;
    public ResourceGenerator grid;

    public void Start()
    {
        grid.GenerateNodes();
    }

    //Extraction = 0, Scanning = 1
    public void ChangeMode(int newMode)
    {
        mode = (ExtractorMode)newMode;
    }

    public void AddResource(int resource)
    {
        resourceTotal += resource;
    }
}
