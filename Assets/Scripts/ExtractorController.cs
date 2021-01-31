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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMode(ExtractorMode newMode)
    {
        mode = newMode;
    }

    public void AddResource(int resource)
    {
        resourceTotal += resource;
    }
}
