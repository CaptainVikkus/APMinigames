using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNodeController : MonoBehaviour
{
    public enum Level
    {
        NONE,
        LOW,
        MEDIUM,
        HIGH
    }

    public int resource;
    public int maxResource = 100;
    public Material High;
    public Material Medium;
    public Material Low;
    public Material None;

    public GameObject plane;

    //Set the value of the node and plane material
    public void SetLevel(Level level)
    {
        switch (level)
        {
            case Level.NONE:
                resource = 0;
                plane.GetComponent<MeshRenderer>().material = None;
                break;
            case Level.LOW:
                resource = maxResource / 4;
                plane.GetComponent<MeshRenderer>().material = Low;
                break;
            case Level.MEDIUM:
                resource = maxResource / 2;
                plane.GetComponent<MeshRenderer>().material = Medium;
                break;
            case Level.HIGH:
                resource = maxResource;
                plane.GetComponent<MeshRenderer>().material = High;
                break;
            default:
                break;
        }

    }

    public int Collect()
    {
        int collected = resource;
        SetLevel(Level.NONE);
        Scan();
        return collected;
    }

    public void Scan()
    {
        plane.SetActive(true);
    }
}
