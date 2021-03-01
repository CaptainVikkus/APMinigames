using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LockpickUI : OpenUI
{
    public LockpickController controller;

    protected override void Start()
    {
        base.Start();
        controller.OnLockpickEnd += LockPickEnd;
    }
    private void LockPickEnd(bool won)
    {
        EnableUI(false);
    }

    protected override void EnableUI(bool enable)
    {
        base.EnableUI(enable);
        if (enable)
            controller.shutter.Play();
    }
}
