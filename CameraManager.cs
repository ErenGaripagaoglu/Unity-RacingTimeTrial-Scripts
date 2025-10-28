using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject orbitOrigin;

    [SerializeField] private Camera follower;
    [SerializeField] private Camera orbit;
    [SerializeField] private Camera hood;

    private short selectedCamIndex = 0;
    private int orbitAngle;

    // Start is called before the first frame update
    void Start()
    {
        orbit.enabled = false;
        hood.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (selectedCamIndex < 2)
            {
                selectedCamIndex += 1;
            }
            else if (selectedCamIndex == 2)
            {
                selectedCamIndex = 0;
            }

            switch (selectedCamIndex)
            {
                case 0: FollowActivate(); break;
                case 1: OrbitActivate(); break;
                case 2: HoodActivate(); break;
            }
        }

        if(selectedCamIndex == 1) {
            OrbitAngleCheck();
        }
    }

    void FollowActivate()
    {
        follower.enabled = true;

        orbit.enabled = false;
        hood.enabled = false;
    }

    void OrbitActivate()
    {
        orbit.enabled = true;

        follower.enabled = false;
        hood.enabled = false;
    }

    void HoodActivate()
    {
        hood.enabled = true;

        follower.enabled = false;
        orbit.enabled = false;
    }

    void OrbitAngleCheck()
    {
        if (Input.GetKey(KeyCode.E)) {
            orbitAngle += 5;
        }
        
        if (Input.GetKey(KeyCode.Q)) {
            orbitAngle -= 5;
        }

        orbitOrigin.transform.Rotate(0f, orbitAngle, 0f);
        orbitAngle = 0; //Stop Rotating

        if (Input.GetKey(KeyCode.V)) {
            orbitOrigin.transform.rotation = new Quaternion( 0f, 0f, 0f, 0f); //Reset Rotation
        }
    }
}
