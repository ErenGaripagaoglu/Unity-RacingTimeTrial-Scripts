using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearChangeSound : MonoBehaviour
{
    //Dependencies
    private float speedRatio;
    private float gas;
    private int gearN;
    private CarController carController;

    //Pause
    private static bool GameIsPaused = false;
    [SerializeField] private GameObject pauseMenu;
    //

    [SerializeField] private AudioSource AirOutSound;
    private float cooldown = 0;

    // Start is called before the first frame update
    void Start()
    {
        carController = GetComponent<CarController>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldown += Time.deltaTime;

        VerifyDependencies();
        CheckPause();
        playSound();
    }

    private void playSound() {

        if (gearN <= 0) {
            return;
        }

        if (speedRatio < 0.7f) {
            return;
        }

        if (gas > 0.9f) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.W)) {
            return;
        }

        if (cooldown < 5f) {
            return;
        }

        cooldown = 0f;
        AirOutSound.Play();
        
        //Debug.Log("Works");
    }

    void VerifyDependencies()
    {
        if (carController)
        {
            speedRatio = carController.GetSpeedRatio();
            gas = carController.gasInput;
            gearN = carController.gearNum;
        }
    }

    void CheckPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                GameIsPaused = false;
                AirOutSound.UnPause();
            }
            else
            {
                GameIsPaused = true;
                AirOutSound.Pause();
            }
        }

        if (pauseMenu.activeSelf)
        {
            AirOutSound.Pause();
        }
        else if (!pauseMenu.activeSelf)
        {
            AirOutSound.UnPause();
        }
    }
}
