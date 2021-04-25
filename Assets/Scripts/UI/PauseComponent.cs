using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseComponent : OpenUI
{
    private bool isPaused = false;

    protected override void Start()
    {
        //No Trigger Check
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            EnableUI(!isPaused);
    }

    protected override void EnableUI(bool enable)
    {
        base.EnableUI(enable);

        Time.timeScale = enable ? 0f : 1f;
        isPaused = enable;
    }

    public void Pause(bool pause)
    {
        EnableUI(pause);
    }
}
