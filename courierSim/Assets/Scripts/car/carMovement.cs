using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class carMovement : MonoBehaviour
{
    public static carMovement instance;
    public float speed;
    public float Topspeed;
    public float rotationSpeed = 10;
    public float rotationAmount;
    public GameObject streetWheel;
    public GameObject fender;
    public GameObject stay;
    public TMP_Text speedText;
    public Slider speedSlider;
    private ColorBlock colors;
    public bool isSkid;

    public float carDamageValue;
    public bool isVeryDamage;

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

    private float currentX = 0.0f;
    private float currentY = 0.0f;

    //  public TMPro.TMP_Text speedText;
// private CarLights carLights;

    void Start()
    {
        instance = this;
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
            skid();

            speed = carRb.velocity.magnitude * 3.6f;
            speedText.text = Mathf.Round(speed) + " km/h";
            speedSlider.value = speed;
            colors = speedSlider.colors;
            if (speed + 5 > Topspeed)
            {
                colors.normalColor = new Color(179 / 255f, 87 / 2055f, 87 / 2055f);
            }
            else
            {
                colors.normalColor = Color.white;
            }

            speedSlider.colors = colors;
        }
    }

    void LateUpdate()
    {
        DamageValueSystem();
        if (interact.instance.isMotor && !interact.instance.isMechanic && carDamageValue <100)
        {
            Move();
            Steer();
            Brake();
            skid();
            damageSystem();
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 300 * brakeAcceleration * Time.deltaTime;
                wheel.wheelCollider.motorTorque = 0;
            }
        }
    }

    public void MoveInput(float input)
    {
        moveInput = input;
    }

    public void SteerInput(float input)
    {
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
        if (!isSkid)
        {
            if (speed < Topspeed)
            {
                foreach (var wheel in wheels)
                {
                    if (wheel.axel == Axel.Rear)
                    {
                        wheel.wheelCollider.motorTorque = moveInput * 600 * maxAcceleration * Time.deltaTime;
                    }
                }
            }
            else
            {
                foreach (var wheel in wheels)
                {
                    if (wheel.axel == Axel.Rear)
                    {
                        wheel.wheelCollider.motorTorque = 0;
                    }
                }
            }
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

                if (steerInput != 0 )
                {
                    rotationAmount = wheel.wheelCollider.steerAngle * 2f;

                    Quaternion currentRotation = stay.transform.localRotation;
                    float newZRotationValue = currentRotation.eulerAngles.z + rotationAmount / 2f;
                    streetWheel.transform.localRotation = Quaternion.Euler(0f, rotationAmount / 2, 0f);
                    fender.transform.localRotation = Quaternion.Euler(0f, rotationAmount / 2f, 0f);
                    stay.transform.localRotation = Quaternion.Lerp(stay.transform.localRotation,
                        Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y,
                            -rotationAmount / 2f), Time.deltaTime * rotationSpeed);
                }
                else if( !interact.instance.isMechanic)
                {
                    Quaternion currentRotation = this.transform.rotation;
                    float zRotation = 0;
                    Quaternion newRotation = Quaternion.Euler(currentRotation.eulerAngles.x,
                        currentRotation.eulerAngles.y, zRotation);

                    this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, newRotation,
                        Time.deltaTime * rotationSpeed);
                }

       

                // hıza göre direksiyon sertliği
                float speedLimit = 40;
                maxSteerAngle = Mathf.Lerp(20, 10, Mathf.InverseLerp(0, speedLimit, speed));
            }
        }
    }

    void Brake()
    {
        if (!isSkid)
        {
            if (Input.GetKey(KeyCode.S) && moveInput != 0)
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
    }

    void skid()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            isSkid = true;
            foreach (var wheel in wheels)
            {
                if (wheel.axel == Axel.Front)
                {
                    wheel.wheelCollider.brakeTorque = 30000;
                }

                if (wheel.axel == Axel.Rear)
                {
                    wheel.wheelCollider.motorTorque = 30000;
                    wheel.wheelCollider.sidewaysFriction.stiffness.Equals(0);
                }
            }
        }
        else
        {
            isSkid = false;
            foreach (var wheel in wheels)
            {
                if (wheel.axel == Axel.Front && moveInput == 0)
                {
                    wheel.wheelCollider.brakeTorque = 0;
                    //print("freni :" + wheel.wheelCollider.brakeTorque);
                }

                if (wheel.axel == Axel.Rear && moveInput == 0)
                {
                    wheel.wheelCollider.motorTorque = 0;
                    wheel.wheelCollider.sidewaysFriction.stiffness.Equals(100);
                    //print("motor torku :" + wheel.wheelCollider.motorTorque);
                }
                // else if (wheel.axel == Axel.Rear && moveInput !=0)
                // {
                //     //wheel.wheelCollider.motorTorque = 0;
                //     wheel.wheelCollider.sidewaysFriction.stiffness.Equals(2);
                //     //print("motor torku :" + wheel.wheelCollider.motorTorque);
                // }
            }
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

    private void damageSystem()
    {
        if (speed > 5)
        {
            carDamageValue += Time.deltaTime /100;
        }
    }

    private void DamageValueSystem()
    {
        if (carDamageValue > 90)
        {
            isVeryDamage = true;
            if (carDamageValue > 100)
            {
                carDamageValue = 100;
            }
        }
        else
        {
            isVeryDamage = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ground"))
        {
            carDamageValue += Time.deltaTime*speed/2;
        }
    }
}