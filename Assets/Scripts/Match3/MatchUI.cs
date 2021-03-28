using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchUI : OpenUI
{
    public Match3 match;

    protected override void EnableUI(bool enable)
    {
        base.EnableUI(enable);
        if (enable) match.BuildGame();
    }

    protected override void Start()
    {
        //base.Start();
    }
}
