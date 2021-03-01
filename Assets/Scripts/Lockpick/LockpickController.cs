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
    [Range(1, 5)]
    [Tooltip("Changes the playback speed of the minigame")]
    public int difficulty = 1;
    #endregion

    #region Events
    public delegate void LockPickEnd(bool won);
    public event LockPickEnd OnLockpickEnd;
    #endregion

    private Material[] nodeMat = new Material[3];
    private int[] choice = { 0, 1, 2 };
    private bool playing = false;

    // Start is called before the first frame update
    void Start()
    {
        nodeMat[(int)Node.GREEN] = green;
        nodeMat[(int)Node.YELLOW] = yellow;
        nodeMat[(int)Node.BLUE] = blue;
    }

    // Update is called once per frame
    void Update()
    {
        if (startNodes[0].isLocked && startNodes[1].isLocked && startNodes[2].isLocked && playing)
        {//Completed
            shutter.Stop();
            if (OnLockpickEnd != null)
            {
                OnLockpickEnd(true);
            }
            playing = false;
        }
    }

    public void Init()
    {
        //Randomiz the Start Node Colors
        RandomizeColors();
        int i = 0;
        foreach (var item in startNodes)
        {
            item.transform.localPosition = item.startPos;
            item.isLocked = false;
            item.nodeType = (Node)choice[i];
            item.GetComponent<MeshRenderer>().material = nodeMat[choice[i++]];
        }
        //Randomize the End Node Colors
        RandomizeColors();
        i = 0;
        foreach (var item in endNodes)
        {
            item.nodeType = (Node)choice[i];
            item.GetComponent<MeshRenderer>().material = nodeMat[choice[i++]];
        }
    }

    private void RandomizeColors()
    {
        for (int i = 0; i < choice.Length -1; i++)
        {
            int t = Random.Range(i, choice.Length);
            var temp = choice[i];
            choice[i] = choice[t];
            choice[t] = temp;
        }
    }

    public void ShutterOpen()
    {
        //Start Checking
        playing = true;
        shutter["ShutterClose"].speed = difficulty/20.0f;
        shutter.Play("ShutterClose");
    }
    public void ShutterClose()
    {
        if (playing) //shutter closed midgame
        {
            playing = false;
            if (OnLockpickEnd != null)
                OnLockpickEnd(false);
        }
    }
}
