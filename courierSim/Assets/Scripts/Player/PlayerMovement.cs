using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public GameObject headObj;
    public float speed = 1f;
    private float runSpeed = 0;
    public float jumpHeight = 2f;
    public float sensitivity = 2.0f;
    public Animator anims;

    Vector3 velocity;
    public float gravity = -9f;
    public bool isGround;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;


    private Camera playerCamera;
    private float rotationX = 0;

    private Rigidbody rb;

    public float distance = 5.0f; // Distance between the player and camera
    public float rotationSpeed = 5.0f; // Speed at which the camera rotates
    public float resetSpeed = 2.0f; // Speed at which the camera resets its rotation
    public Vector2 rotationLimits = new Vector2(-80f, 80f); // Vertical rotation limits

    private float currentX = 0.0f;
    private float currentY = 0.0f;

    private void Start()
    {
        Camera.main.transform.SetParent(headObj.transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, 0);

        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!interact.instance.isMotor)
        {
            Move();
            cameraRotation();
            this.transform.SetParent(null);
            anims.SetBool("drive", false);
            rb.useGravity = true;

            Camera.main.transform.SetParent(headObj.transform);
            Camera.main.transform.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            // if(!isLocalPlayer)return;
            
            anims.SetBool("drive", interact.instance.isMotor);
            rb.useGravity = false;
            if (interact.instance.isChangeCameraPov)
            {
                cameraRotation();
                Camera.main.transform.SetParent(headObj.transform);
                Camera.main.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }

    private void Move()
    {
        isGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        // Karakterin hareketi
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Hareket vektörünü oluştur
        Vector3 moveDirection = new Vector3(x, 0, z).normalized;

        // Kamera yönüne göre hareketi dönüştür
        Vector3 move = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0) * moveDirection;

        // Karakteri hareket ettir
        float targetSpeed = speed * Mathf.Max(Mathf.Abs(x), Mathf.Abs(z));
        Vector3 targetVelocity = move * targetSpeed;
        velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref velocity, 0.05f);


        // Zamana bağlı hareket
        transform.Translate(velocity * Time.fixedDeltaTime, Space.World);
        //animasyon işlemleri
        if (x > 0 || z > 0 || x < 0 || z < 0)
        {
            if (z > 0)
            {
                anims.SetBool("walk", true);
                anims.SetBool("backWalk", false);
                anims.SetBool("rightWalk", false);
                anims.SetBool("leftWalk", false);
            }

            if (z < 0)
            {
                anims.SetBool("backWalk", true);
                anims.SetBool("walk", false);
                anims.SetBool("rightWalk", false);
                anims.SetBool("leftWalk", false);
            }

            if (x < 0 && z == 0)
            {
                anims.SetBool("leftWalk", true);
                anims.SetBool("rightWalk", false);
                anims.SetBool("walk", false);
                anims.SetBool("backWalk", false);
            }

            if (x > 0 && z == 0)
            {
                anims.SetBool("rightWalk", true);
                anims.SetBool("leftWalk", false);
                anims.SetBool("walk", false);
                anims.SetBool("backWalk", false);
            }

            if (runSpeed > 0)
            {
                anims.SetBool("run", true);
            }
            else
            {
                anims.SetBool("run", false);
            }
        }
        else if (x == 0 && z == 0)
        {
            anims.SetBool("walk", false);
            anims.SetBool("backWalk", false);
            anims.SetBool("run", false);
            anims.SetBool("rightWalk", false);
            anims.SetBool("leftWalk", false);
        }

        //zemin kontrolü
        // if (isGround == false)
        // {
        //     Vector3 move = transform.right * x + transform.forward * z;
        //     CharacterController.Move(move * 2 * Time.deltaTime);
        // }
        //
        // if (isGround == true)
        // {
        //     Vector3 move = transform.right * x + transform.forward * z;
        //     runSpeed = Mathf.Lerp(runSpeed, 1f, Time.deltaTime);
        //     CharacterController.Move(move * runSpeed * Time.deltaTime);
        //     CharacterController.Move(move * speed * Time.deltaTime);
        //
        //
        //     if (Input.GetKey(KeyCode.LeftShift))
        //     {
        //         runSpeed = Mathf.Lerp(runSpeed, 1f, Time.deltaTime);
        //         CharacterController.Move(move * (runSpeed += Time.deltaTime * 2) * Time.deltaTime);
        //     }
        //     else
        //     {
        //         runSpeed = 0;
        //     }
        // }
        //
        // if (isGround && velocity.y < 0)
        // {
        //     velocity.y = -2f;
        // }
        //
        // if (Input.GetButtonDown("Jump") && isGround)
        // {
        //     velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        // }
        //
        // velocity.y += gravity * Time.deltaTime;
        // CharacterController.Move(velocity * Time.deltaTime);
    }

    private void cameraRotation()
    {
        // Kamera hareketi
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        // Kamerayı döndür
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, mouseX, 0);

        // Kamerayı döndür
        // playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        // transform.rotation *= Quaternion.Euler(0, mouseX, 0);

    }
    // private void bend()
    // {
    //     if (Input.GetKey(KeyCode.LeftControl))
    //     {
    //         CharacterController.height = 1f;
    //     }
    //     else if (!Input.GetKey(KeyCode.LeftControl))
    //     {
    //         if (CharacterController.height >= 1f && CharacterController.height < 2.52f)
    //         {
    //             CharacterController.height += Time.deltaTime * 10;
    //         }
    //     }
    // }
}