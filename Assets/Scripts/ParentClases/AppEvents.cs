using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppEvents
{
    public delegate void MouseCursorEnable(bool enabled);
    public static event MouseCursorEnable MouseCursorEnabled;

    public static void Invoke_OnMouseCursorEnable(bool enabled)
    {
        MouseCursorEnabled?.Invoke(enabled);
    }
}

public class PlayerEvents
{
    public delegate void SeasonChange(int season);
    public static event SeasonChange SeasonChanged;

    public static void Invoke_SeasonChange(int season)
    {
        SeasonChanged?.Invoke(season);
    }

    public delegate void PuzzleComplete();
    public static event PuzzleComplete PuzzleCompleted;

    public static void Invoke_PuzzleComplete()
    {
        PuzzleCompleted?.Invoke();
    }
}