using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class carMovement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed = 10;
    public float rotationAmount;
    public GameObject streetWheel;
    public GameObject fender;
    public GameObject stay;
    public TMP_Text speedText;

    public enum ControlMode
    {
        Keyboard,
        Buttons
    };

    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public GameObject wheelEffectObj;
        public ParticleSystem smokeParticle;
        public Axel axel;
    }

    public ControlMode control;

    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;

    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f;

    public Vector3 _centerOfMass;

    public List<Wheel> wheels;

    float moveInput;
    float steerInput;

    private Rigidbody carRb;


    //  public TMPro.TMP_Text speedText;
// private CarLights carLights;

    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;
        // carLights = GetComponent<CarLights>();
    }

    void Update()
    {
        if (interact.instance.isMotor)
        {
            GetInputs();
            AnimateWheels();
            WheelEffects();

            speed = carRb.velocity.magnitude * 3.6f;

            speedText.text = Mathf.Round(speed) + " km/h";
        }
    }

    void LateUpdate()
    {
        Move();
        Steer();
        Brake();
    }

    public void MoveInput(float input)
    {
        // if (!isLocalPlayer) return;
        moveInput = input;
    }

    public void SteerInput(float input)
    {
        // if (!isLocalPlayer) return;
        steerInput = input;
    }

    void GetInputs()
    {
        if (control == ControlMode.Keyboard)
        {
            moveInput = Input.GetAxis("Vertical");
            steerInput = Input.GetAxis("Horizontal");
        }
    }

    void Move()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = moveInput * 600 * maxAcceleration * Time.deltaTime;
        }
    }

    void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var _steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);

                if (speed > 3 && steerInput != 0)
                {
                    rotationAmount = wheel.wheelCollider.steerAngle * 2f;
                    
                    streetWheel.transform.localRotation = Quaternion.Euler(0f, rotationAmount / 2, 0f);
                    fender.transform.localRotation = Quaternion.Euler(0f, rotationAmount / 2f, 0f);
                    stay.transform.localRotation = Quaternion.Lerp(stay.transform.localRotation,
                        Quaternion.Euler(0f, 0f, -rotationAmount / 2f), Time.deltaTime * rotationSpeed);
                }
                else
                {
                    Quaternion currentRotation = this.transform.rotation;
                    float zRotation = 0; 
                    
                    Quaternion newRotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y, zRotation);
                  //  Quaternion newRotation2 = 
                    this.transform.localRotation =Quaternion.Lerp(this.transform.localRotation, newRotation, Time.deltaTime * rotationSpeed);   
                    stay.transform.localRotation = Quaternion.Lerp(stay.transform.localRotation,
                        Quaternion.Euler(0f, 0f, zRotation), Time.deltaTime * rotationSpeed);
                }
            }
        }
    }

    void Brake()
    {
        if (Input.GetKey(KeyCode.Space) || moveInput == 0)
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 300 * brakeAcceleration * Time.deltaTime;
            }

            //   carLights.isBackLightOn = true;
            //   carLights.OperateBackLights();
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }

            //   carLights.isBackLightOn = false;
            //   carLights.OperateBackLights();
        }
    }

    void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }

    // [Command(requiresAuthority = false)]
    void WheelEffects()
    {
        foreach (var wheel in wheels)
        {
            //var dirtParticleMainSettings = wheel.smokeParticle.main;

            if (Input.GetKey(KeyCode.Space) && wheel.axel == Axel.Rear && wheel.wheelCollider.isGrounded == true &&
                carRb.velocity.magnitude >= 10.0f)
            {
                wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = true;
                wheel.smokeParticle.Emit(1);
            }
            else
            {
                //  wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;
            }
        }
    }

    public void MotoPosition()
    {
        if (speed == 0)
        {
        }
    }
}