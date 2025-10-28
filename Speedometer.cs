using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    //Dependencies
    private CarController carController;
    private int gearN;
    private float engLimit;
    private float gasInput;
    private float speedKMH;

    private float brakeInput;
    private float steerAngle;
    //
    private float speedNormalised;
    private float rpm = 0f;

    [Header("Car Attributes")]
    [SerializeField] Rigidbody carRB;
    [SerializeField] private float maxSpeed = 0.0f;
    [SerializeField] private float maxRPM;

    [Header("NeedleAngles")]
    [SerializeField] private float minSpeedArrowAngle;
    [SerializeField] private float maxSpeedArrowAngle;
    [SerializeField] private float minrpmArrowAngle;
    [SerializeField] private float maxrpmArrowAngle;

    [Header("UI")]
    [SerializeField] private Text speedLabel;
    [SerializeField] private Text gearLabel;
    [SerializeField] private RectTransform arrowSpeed;
    [SerializeField] private RectTransform arrowRPM;
    [SerializeField] private Text telemetryLabel;
    
    void Start()
    {
        carController = carRB.GetComponent<CarController>();
    }

    private void Update()
    {
        VerifyDependencies();
        EngineRPM();
        GaugeAnim();

        //Dev HUD
        if (telemetryLabel) {
            telemetryLabel.text = "Ratio: " + (speedNormalised / engLimit).ToString("F2") + "\n" 
                                + "RPM: " + rpm.ToString("F0") + "\n"
                                + "Gas: " + gasInput.ToString("F0") + "\n" 
                                + "Brake: " + brakeInput.ToString("F0") + "\n"
                                + "Steer: " + steerAngle.ToString("F0");
        }
    }

    void GaugeAnim()
    {
        if (speedLabel) {
            speedLabel.text = (speedKMH.ToString("F0"));
        }
            
        if (arrowSpeed) {
            arrowSpeed.localEulerAngles =
                new Vector3(0, 0, Mathf.Lerp(minSpeedArrowAngle, maxSpeedArrowAngle, speedKMH / maxSpeed));
        }

        if (arrowRPM) {
            arrowRPM.localEulerAngles =
                new Vector3(0, 0, Mathf.Lerp(minrpmArrowAngle, maxrpmArrowAngle, rpm / maxRPM));
        }

        if (gearLabel) {
            switch (gearN)
            {
                case -1: gearLabel.text = ("R"); break;
                case 0: gearLabel.text = ("N"); break;
                default: gearLabel.text = (gearN.ToString()); break;
            }
        }
    }

    void VerifyDependencies()
    {
        if (carController)
        {
            speedKMH = carController.speed * 3.6f;
            gearN = carController.gearNum;
            gasInput = carController.gasInput;
            engLimit = carController.maxSpeed;

            brakeInput = carController.brakeInput;
            steerAngle = carController.steerAngle;
        }

        speedNormalised = carController.speed;
    }

    void EngineRPM()
    {
        switch (gearN)
        {
            case 0: rpm = gasInput * maxRPM; break;
            default: rpm = (speedNormalised / engLimit) * maxRPM; break;
        }
    }
}
