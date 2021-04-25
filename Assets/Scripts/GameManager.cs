using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : Singleton<GameManager>
{
    public bool CursorActive { get; private set; } = true;
    public AudioMixer mixer;
    public static float Sensitivity = 100;
    public static float Volume = 1f;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void EnableCursor(bool enable)
    {
        CursorActive = enable;
        Cursor.visible = enable;
        Cursor.lockState = enable ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        AppEvents.MouseCursorEnabled += EnableCursor;
    }
    private void OnDisable()
    {
        AppEvents.MouseCursorEnabled -= EnableCursor;
    }

    public static void SetSensitivity(float value)
    {
        Sensitivity = value;
    }

    public void SetVolume(float value)
    {
        Volume = value;
        mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }
}
