using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSounds : MonoBehaviour
{
    //Dependencies
    private float speedRatio;
    private int gearbox;
    private float gas;
    private CarController carController;

    //Pause
    private static bool GameIsPaused = false;
    [SerializeField] private GameObject pauseMenu;
    //

    [SerializeField] private AudioSource RunningSound;
    [SerializeField] private float runningMaxVolume;
    [SerializeField] private float runningMaxPitch;
    [SerializeField] private float runningMinPitch;

    void Start()
    {
        carController = GetComponent<CarController>();
    }

    void Update()
    {
        VerifyDependencies();
        CheckPause();
        SoundPlayer();
    }

    void SoundPlayer()
    {
        if (gearbox == 0)
        {
            if (gas == 0)
            {
                RunningSound.volume = 0.3f;
                RunningSound.pitch = runningMinPitch;
            }
            else
            {
                RunningSound.volume = Mathf.Lerp(0.3f, runningMaxVolume, gas);
                RunningSound.pitch = Mathf.Lerp(runningMinPitch, runningMaxPitch + (gas * 0.75f), gas);
            }
        }

        if (0 < gearbox || gearbox > 0)
        {
            RunningSound.volume = Mathf.Lerp(0.3f, runningMaxVolume, speedRatio);
            RunningSound.pitch = Mathf.Lerp(runningMinPitch, runningMaxPitch + (gas * 0.75f), speedRatio);
        }
    }

    void VerifyDependencies()
    {
        if (carController)
        {
            speedRatio = carController.GetSpeedRatio();
            gearbox = carController.gearNum;
            gas = carController.gasInput;
        }
    }

    void CheckPause()
    {
        //ESC with Pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                GameIsPaused = false;
                RunningSound.UnPause();
            }
            else
            {
                GameIsPaused = true;
                RunningSound.Pause();
            }
        }
        //

        //ESC Pause but Resume
        if (pauseMenu.activeSelf)
        {
            RunningSound.Pause();
        }
        else if (!pauseMenu.activeSelf)
        {
            RunningSound.UnPause();
        }
    }
}
