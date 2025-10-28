using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class CameraRelocate : MonoBehaviour
{
    [SerializeField] private GameObject cameraSetup;
    [SerializeField] private GameObject followFocus;
    [SerializeField] private GameObject followFocusStatic;

    [SerializeField] private GameObject playerCar;

    private void FixedUpdate()
    {
        cameraSetup.transform.position = playerCar.transform.position;
        cameraSetup.transform.rotation = playerCar.transform.rotation;

        followFocus.transform.position = followFocusStatic.transform.position; 
        followFocus.transform.rotation = followFocusStatic.transform.rotation;
    }

}
