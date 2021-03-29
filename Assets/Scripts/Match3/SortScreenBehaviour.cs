using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class WinHandler
{
    public delegate void Won(Match3.Difficulty difficulty);
    public static event Won OnWin;

    public static void CallWin(Match3.Difficulty difficulty)
    {
        OnWin?.Invoke(difficulty);
    }
}

public class SortScreenBehaviour : MonoBehaviour
{
    public Image easyWin;
    public Image mediumWin;
    public Image hardWin;

    private void Start()
    {
        WinHandler.OnWin += SaveWin;
    }

    private void SaveWin(Match3.Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Match3.Difficulty.Easy:
                easyWin.enabled = true;
                break;
            case Match3.Difficulty.Medium:
                mediumWin.enabled = true;
                break;
            case Match3.Difficulty.Hard:
                hardWin.enabled = true;
                break;
            default:
                break;
        }
    }
}
