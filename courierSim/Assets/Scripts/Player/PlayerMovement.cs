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
    public bool isGround;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Camera playerCamera;
    private float rotationX = 0;
    private float rotationY = 0;

    private Rigidbody rb;

    public float distance = 5.0f;
    public float rotationSpeed = 5.0f;
    public float resetSpeed = 2.0f;
    public Vector2 rotationLimits = new Vector2(-80f, 80f);

    private void Start()
    {
        Camera.main.transform.SetParent(headObj.transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, 0);

        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        // araç modu ayarları
        if (!interact.instance.isMotor)
        {
            Move();
            if (!phoneMenu.instance.isPhoneActive || phoneMenu.instance.isMapActive)
            {
                cameraRotation();
            }

            this.transform.SetParent(null);
            anims.SetBool("drive", false);
            rb.useGravity = true;

            Camera.main.transform.SetParent(headObj.transform);
            Camera.main.transform.localPosition = new Vector3(0, -0.5f, -0.1f);
        }
        else
        {
            anims.SetBool("drive", interact.instance.isMotor);
            rb.useGravity = false;
            if (interact.instance.isChangeCameraPov)
            {
                if (!phoneMenu.instance.isPhoneActive)
                {
                    cameraRotation();
                    Camera.main.transform.SetParent(headObj.transform);
                    Camera.main.transform.localPosition = new Vector3(0, -0.5f, -0.1f);
                }
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
        velocity = Vector3.Lerp(velocity, targetVelocity, speed);

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
    }

    private void cameraRotation()
    {
        // Kamera hareketi
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;
        
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -80, 80);
        rotationY -= mouseX;
        
        
        if (!interact.instance.isChangeCameraPov)
        {
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation = Quaternion.Euler(0, -rotationY, 0);
        }
        else
        { rotationY = Mathf.Clamp(rotationY, -150, 150);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, -rotationY, 0);
        }
    }
}