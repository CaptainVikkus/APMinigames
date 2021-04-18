using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackingUI : OpenUI
{
    public HackingGame game;

    protected override void EnableUI(bool enable)
    {
        base.EnableUI(enable);

        game.BuildCode();
    }
}
