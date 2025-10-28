using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurboSpool : MonoBehaviour
{
    //Dependencies
    private int gearbox;
    private float gas;
    private CarController carController;

    //Pause
    private static bool GameIsPaused = false;
    [SerializeField] private GameObject pauseMenu;
    //

    [SerializeField] private AudioSource SpoolSound;
    private float gasTime;

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
        if (gearbox > 0)
        { 
            if (gas > 0)
            {
                gasTime = gasTime + Time.deltaTime;
            }

            if (gasTime > 0)
            {
                if (gas <= 0)
                {
                    gasTime = 0;

                    SpoolSound.Play();
                }
            }
            //Debug.Log(gasTime);
        }
    }

    void VerifyDependencies()
    {
        if (carController)
        {
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
                SpoolSound.UnPause();
            }
            else
            {
                GameIsPaused = true;
                SpoolSound.Pause();
            }
        }
        //

        //ESC Pause but Resume
        if (pauseMenu.activeSelf)
        {
            SpoolSound.Pause();
        }
        else if (!pauseMenu.activeSelf)
        {
            SpoolSound.UnPause();
        }
    }
}
