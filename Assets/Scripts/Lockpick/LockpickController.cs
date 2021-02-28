using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockpickController : MonoBehaviour
{
    public enum Node
    {
        GREEN,
        YELLOW,
        BLUE
    }

    #region GamePieces
    [Header("GamePieces")]
    public List<LockNode> startNodes = new List<LockNode>(3);
    public List<LockNode> endNodes = new List<LockNode>(3);
    public Animation shutter;
    public Material green;
    public Material yellow;
    public Material blue;
    #endregion
    #region Settings
    [Range(1, 10)]
    [Tooltip("Changes the playback speed of the minigame")]
    public int difficulty = 1;
    #endregion

    private Material[] nodeMat = new Material[3];

    // Start is called before the first frame update
    void Start()
    {
        nodeMat[(int)Node.GREEN] = green;
        nodeMat[(int)Node.YELLOW] = yellow;
        nodeMat[(int)Node.BLUE] = blue;

        shutter.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        //Randomiz the Start Node Colors
        RandomizeColors();
        int i = 0;
        foreach (var item in startNodes)
        {
            item.GetComponent<MeshRenderer>().material = nodeMat[i++];
        }
        //Randomize the End Node Colors
        RandomizeColors();
        i = 0;
        foreach (var item in endNodes)
        {
            item.GetComponent<MeshRenderer>().material = nodeMat[i++];
        }
    }

    private void RandomizeColors()
    {
        for (int i = 0; i < nodeMat.Length -1; i++)
        {
            int t = Random.Range(i, nodeMat.Length);
            var temp = nodeMat[i];
            nodeMat[i] = nodeMat[t];
            nodeMat[t] = temp;
        }
    }
}
