using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CarController : MonoBehaviour
{
    [Header("Shared Parameters")]
    //Shared (Speedometer - VehicleSounds - FollowCamera)
    public float gasInput;
    public int gearNum = 0;
    public int MaxGearNum;
    public float maxSpeed;
    public float speed;

    //Telemetry
    public float brakeInput;
    public float steeringInput;
    public float steerAngle; //Getting SteeringAngle from private
    //

    //Dependencies
    private SmokeManager manageSmoke;
    private GearboxPref manageGearbox;
    //
    private Rigidbody carRB;
    private float slipAngle;
    private float steeringAngle;
    private float rpmLevel;
    private float counter = 0;

    [Header("Prefab Childs")]
    public WheelColliders colliders;
    public WheelMeshes wheelMeshes;
    public WheelParticles wheelParticles;

    [Header("Vehicle Attributes")]
    [SerializeField] private bool allWheeldrive;
    [SerializeField] private Vector3 _centerOfMass;

    [SerializeField] private float motorPower;
    [SerializeField] private float brakePower;

    [SerializeField] private AnimationCurve steeringCurve;
    [SerializeField] private AnimationCurve lowSteeringCurve;

    [Header("Smoke Effect Initiate")]
    [SerializeField] private float FfrictionAllowance;
    [SerializeField] private float RfrictionAllowance;
    
    [Header("Slide Parameters")]
    [SerializeField] private float frictionDefault;
    [SerializeField] private float slipPower;
    [SerializeField] private float stockMotorPower;

    [System.Serializable]
    public class WheelColliders
    {
        public WheelCollider FLWheel;
        public WheelCollider FRWheel;
        public WheelCollider RLWheel;
        public WheelCollider RRWheel;
    }

    [System.Serializable]
    public class WheelMeshes
    {
        public MeshRenderer FLWheel;
        public MeshRenderer FRWheel;
        public MeshRenderer RLWheel;
        public MeshRenderer RRWheel;
    }

    [System.Serializable]
    public class WheelParticles
    {
        public ParticleSystem FLWheel;
        public ParticleSystem FRWheel;
        public ParticleSystem RLWheel;
        public ParticleSystem RRWheel;
    }

    void Start()
    {
        carRB = GetComponent<Rigidbody>();
        carRB.centerOfMass = _centerOfMass;
        manageSmoke = GetComponent<SmokeManager>();
        manageGearbox = GetComponent<GearboxPref>();
    }

    void Update()
    {
        speed = carRB.velocity.magnitude;
        slipAngle = Vector3.Angle(transform.forward, carRB.velocity - transform.forward);
        steerAngle = steeringAngle;

        CheckInput();
        ApplyBrake();
        Handbrake();
        ApplySteering();

        HorsePower();
        Transmission();
       
        CheckFriction();
        UpdateWheels();
    }
    
    void CheckInput()
    {
        gasInput = Input.GetAxis("Vertical");
        steeringInput = Input.GetAxis("Horizontal");

        //BrakingOnDrive
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            if (gasInput <= 0)
            {
                brakeInput = Mathf.Abs(gasInput);
                gasInput = 0;
            }
            else
            {
                brakeInput = 0;
            }
        }
        else
        {
            brakeInput = 0;
        }

        //Reverse
        if (gearNum == -1) {
            if (gasInput>=0) {
                gasInput = gasInput * -1;
            }
        }
    }

    void ApplyBrake()
    {
        if (allWheeldrive)
        {
            colliders.FLWheel.brakeTorque = brakeInput * brakePower * 0.7f ;
            colliders.FRWheel.brakeTorque = brakeInput * brakePower * 0.7f ;

            colliders.RLWheel.brakeTorque = brakeInput * brakePower * 0.7f ;
            colliders.RRWheel.brakeTorque = brakeInput * brakePower * 0.7f ;
        }
        else
        {
            colliders.FLWheel.brakeTorque = brakeInput * brakePower * 0.7f ;
            colliders.FRWheel.brakeTorque = brakeInput * brakePower * 0.7f ;

            colliders.RLWheel.brakeTorque = brakeInput * brakePower * 0.4f ;
            colliders.RRWheel.brakeTorque = brakeInput * brakePower * 0.4f ;
        }
    }

    void Handbrake()
    {
        WheelFrictionCurve sRFriction = colliders.RLWheel.sidewaysFriction;

        if (colliders.RRWheel.sidewaysFriction.stiffness != frictionDefault) {
            motorPower = slipPower;
        }
        else 
        { 
            motorPower = stockMotorPower; 
        }


        if (Input.GetKey(KeyCode.Space))
        {
            if (allWheeldrive)
            {
                colliders.FLWheel.brakeTorque = brakePower * 2f;
                colliders.FRWheel.brakeTorque = brakePower * 2f;
                colliders.RLWheel.brakeTorque = brakePower * 2f;
                colliders.RRWheel.brakeTorque = brakePower * 2f;

                //Powerslide
                sRFriction.stiffness = 2.55f;

                colliders.RLWheel.sidewaysFriction = sRFriction;
                colliders.RRWheel.sidewaysFriction = sRFriction;
            }
            else
            {
                colliders.RLWheel.brakeTorque = brakePower * 2f;
                colliders.RRWheel.brakeTorque = brakePower * 2f;

                //Drift
                sRFriction.stiffness = 0.75f;

                colliders.RLWheel.sidewaysFriction = sRFriction;
                colliders.RRWheel.sidewaysFriction = sRFriction;
            }
        }
        else //Default Slip Stiffness
        {
            counter += Time.deltaTime;

            if (counter > 3f)
            {
                sRFriction.stiffness = frictionDefault;

                colliders.RLWheel.sidewaysFriction = sRFriction;
                colliders.RRWheel.sidewaysFriction = sRFriction;

                counter = 0;
            }
        }
    }

    void HorsePower()
    {
        rpmLevel = (GetSpeedRatio() + 0.7f);

        if (rpmLevel > 1.0f) {
            rpmLevel = 1.3f;
        }

        if (gearNum != 0)
        {
            if (allWheeldrive)
            {
                colliders.FLWheel.motorTorque = (motorPower / 3) * gasInput * rpmLevel;
                colliders.FRWheel.motorTorque = (motorPower / 3) * gasInput * rpmLevel;

                colliders.RLWheel.motorTorque = (motorPower * 3/4) * gasInput * rpmLevel;
                colliders.RRWheel.motorTorque = (motorPower * 3/4) * gasInput * rpmLevel;
            }
            else
            {                
                colliders.RLWheel.motorTorque = motorPower * gasInput * rpmLevel;
                colliders.RRWheel.motorTorque = motorPower * gasInput * rpmLevel;
            }
        }

        if (speed>maxSpeed)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                colliders.RLWheel.motorTorque = 0;
                colliders.RRWheel.motorTorque = 0;
                colliders.FLWheel.motorTorque = 0;
                colliders.FRWheel.motorTorque = 0;
            }
        }
    }

    void Transmission()
    {
        switch (manageGearbox.gearboxIndex)
        {
            case 0: { ManualTrans(); break; }
            case 1: { AutomaticTrans(); break; }
        }

        if (MaxGearNum==5) {
            switch (gearNum)
            {
                case 0: { maxSpeed = 0; } break;    //0   kmh
                case 1: { maxSpeed = 6; } break;    //21  kmh
                case 2: { maxSpeed = 15; } break;   //54  kmh
                case 3: { maxSpeed = 27; } break;   //97  kmh
                case 4: { maxSpeed = 35; } break;   //126 kmh
                case 5: { maxSpeed = 72; } break;   //260 kmh
                default: { maxSpeed = 7; } break;   //21  kmh
            }
        }

        if (MaxGearNum==6) {
            switch (gearNum)
            {
                case 0: { maxSpeed = 0; } break;    //0   kmh
                case 1: { maxSpeed = 7; } break;    //21  kmh
                case 2: { maxSpeed = 11; } break;   //40  kmh
                case 3: { maxSpeed = 25; } break;   //90  kmh
                case 4: { maxSpeed = 39; } break;   //140 kmh
                case 5: { maxSpeed = 55; } break;   //198 kmh
                case 6: { maxSpeed = 72; } break;   //260 kmh
                default: { maxSpeed = 7; } break;   //21  kmh
            }
        }
    }

    void ManualTrans() 
    {
        // kmh:3,6
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (gearNum < MaxGearNum)
            {
                gearNum = gearNum + 1;
                Debug.Log("Shift Up " + gearNum);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (gearNum >= 0)
            {
                gearNum = gearNum - 1;
                Debug.Log("Shift Down " + gearNum);
            }
        }
    }
    void AutomaticTrans() 
    {
        //N
        if (gearNum == 0)
        {
            if (Input.GetAxis("Vertical") != 0)
            {
                if (gasInput > 0)
                {
                    gearNum = gearNum + 1;
                }

                if (brakeInput>0)
                {
                    gearNum = gearNum - 1;
                }
            }
        }

        //D
        if (gearNum > 0)
        {
            if (GetSpeedRatio() > 0.90f && gearNum < MaxGearNum)
            {
                gearNum = gearNum + 1;
                Debug.Log("Shift Up " + gearNum);
            }
            if (GetSpeedRatio() < 0.10f && gearNum > 1)
            {
                gearNum = gearNum - 1;
                Debug.Log("Shift Down " + gearNum);
            }
            
            //D -> N
            if (brakeInput == 1)
            {
                if (speed < 0.1f && gearNum > 0)
                {
                    gearNum = 0;
                }
            }
        }

        //R -> N
        if (speed < 0.1f && gasInput == 0f && brakeInput != 1f)
        {
            gearNum = 0;
        }
    }

    void ApplySteering()
    {
        if (speed<3) {
            steeringAngle = steeringInput * lowSteeringCurve.Evaluate(speed);
        }
        else
        {
            steeringAngle = steeringInput * steeringCurve.Evaluate(speed);
        }
        
        //Steering Assist
        if (slipAngle < 120f) {
            steeringAngle+=Vector3.SignedAngle(transform.forward, (carRB.velocity / 2.5f)+transform.forward, Vector3.up);
            //steeringAngle+=Vector3.SignedAngle(transform.forward, carRB.velocity+transform.forward, Vector3.up); //Backup
        }
        steeringAngle = Mathf.Clamp(steeringAngle, -120f, 120f);
        colliders.FLWheel.steerAngle = steeringAngle;
        colliders.FRWheel.steerAngle = steeringAngle;
    }

    void CheckFriction()
    {
        if (manageSmoke.smokeAllow==1) {
            WheelHit[] wheelHits = new WheelHit[4];
            colliders.FLWheel.GetGroundHit(out wheelHits[0]);
            colliders.FRWheel.GetGroundHit(out wheelHits[1]);
            colliders.RLWheel.GetGroundHit(out wheelHits[2]);
            colliders.RRWheel.GetGroundHit(out wheelHits[3]);
            if ((Mathf.Abs(wheelHits[0].sidewaysSlip) + Mathf.Abs(wheelHits[0].forwardSlip) > FfrictionAllowance)){
                wheelParticles.FLWheel.Play();
            }
            else
            {
                wheelParticles.FLWheel.Stop();
            }
            if ((Mathf.Abs(wheelHits[1].sidewaysSlip) + Mathf.Abs(wheelHits[1].forwardSlip) > FfrictionAllowance)){
                wheelParticles.FRWheel.Play();
            }
            else
            {
                wheelParticles.FRWheel.Stop();
            }
            if ((Mathf.Abs(wheelHits[2].sidewaysSlip) + Mathf.Abs(wheelHits[2].forwardSlip) > RfrictionAllowance)){
                wheelParticles.RLWheel.Play();
            }
            else
            {
                wheelParticles.RLWheel.Stop();
            }
            if ((Mathf.Abs(wheelHits[3].sidewaysSlip) + Mathf.Abs(wheelHits[3].forwardSlip) > RfrictionAllowance)){
                wheelParticles.RRWheel.Play();
            }
            else
            {
                wheelParticles.RRWheel.Stop();
            }
        }
        else 
        {
            wheelParticles.FLWheel.Stop();
            wheelParticles.FRWheel.Stop();
            wheelParticles.RLWheel.Stop();
            wheelParticles.RRWheel.Stop();
        }        
    }

    void UpdateWheels()
    {
        UpdateWheel(colliders.FLWheel, wheelMeshes.FLWheel);
        UpdateWheel(colliders.FRWheel, wheelMeshes.FRWheel);
        UpdateWheel(colliders.RLWheel, wheelMeshes.RLWheel);
        UpdateWheel(colliders.RRWheel, wheelMeshes.RRWheel);
    }

    void UpdateWheel(WheelCollider coll, MeshRenderer wheelMesh)
    {
        Quaternion rot;
        Vector3 pos;
        coll.GetWorldPose(out pos, out rot);
        wheelMesh.transform.position = pos;
        wheelMesh.transform.rotation = rot;
    }

    public float GetSpeedRatio()
    {
        var gas = Mathf.Clamp(gasInput, (speed/maxSpeed), 1f);
        return speed*gas / maxSpeed;
    }
}