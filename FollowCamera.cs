using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    //Dependencies
    private OffsetSliders offsetSliders;
    private float vOffset;
    private float hOffset;

    private CarController carController;
    private int gearNum;
    private float speed;
    //

    [SerializeField] private Transform cameraFocus;
    [SerializeField] private Transform player;
    private Rigidbody playerRB;

    void Start()
    {
        playerRB = player.GetComponent<Rigidbody>();
        
        carController = playerRB.GetComponent<CarController>();
        offsetSliders = GetComponent<OffsetSliders>();
    }

    void Update()
    {
        VerifyDependencies();
    }

    void VerifyDependencies()
    {
        if (carController)
        {
            gearNum = carController.gearNum;
            speed = carController.speed * 3.6f;
        }
    }

    void LateUpdate()
    {
        vOffset = offsetSliders.VerticalOffset.value * 0.1f;
        hOffset = offsetSliders.HorizontalOffset.value * 0.1f;
    }

    void FixedUpdate()
    {
        if (gearNum>=0)
        {
            DriveCam();
        }

        if (gearNum<0)
        {
            ReverseCam();
        }

        if (Input.GetKey(KeyCode.B))
        {
            LookBack();
        }
    }

    void DriveCam()
    {
        Vector3 playerForward = (playerRB.velocity + player.transform.forward).normalized;
        transform.position = Vector3.Lerp(transform.position,
            player.position + player.transform.TransformVector(0, hOffset, vOffset)
            + playerForward * (-5f),
            speed * Time.deltaTime);
        transform.LookAt(cameraFocus);
    }

    void ReverseCam()
    {
        Vector3 playerForward = ((playerRB.velocity / 10) + player.transform.forward).normalized;
        transform.position = Vector3.Lerp(transform.position,
            player.position + player.transform.TransformVector(0, 1.5f, 8.25f)
            + playerForward * (-5f),
            speed * Time.deltaTime);
        transform.LookAt(player);
    }

    void LookBack()
    {
        if (speed < 1)
        {
            Vector3 playerForward = (playerRB.velocity + player.transform.forward).normalized;
            transform.position = Vector3.Lerp(transform.position, player.position + player.transform.TransformVector(0, 1.5f, 1.2f) + playerForward * (4f), 5 * Time.deltaTime);
            transform.LookAt(player);
        }

        if (speed > 2)
        {
            Vector3 playerForward = ((playerRB.velocity / 10) + player.transform.forward).normalized;
            transform.position = Vector3.Lerp(transform.position, player.position + player.transform.TransformVector(0, 1.5f, 1.2f) + playerForward * (4f), speed * Time.deltaTime);
            transform.LookAt(player);
        }
    }
}
