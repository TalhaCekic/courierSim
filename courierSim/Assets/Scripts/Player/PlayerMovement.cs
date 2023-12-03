using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1f;
    private float runSpeed = 0;
    public float jumpHeight = 2f;
    public float sensitivity = 2.0f;
    private float verticalRotation = 0.0f;
    private float upDownRange = 60.0f;
    public Animator anims;

    Vector3 velocity;
    public bool isGround;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    

    private Camera playerCamera;
    private float rotationX = 0;

    private Rigidbody rb;

    // public override void OnStartLocalPlayer()
    // {
    //     Camera.main.transform.SetParent(headObj.transform);
    //     Camera.main.transform.localPosition = new Vector3(0, 0, 0);
    //     meshRenderer1.forceRenderingOff = true;
    //     meshRenderer2.forceRenderingOff = true;
    // }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // if(!isLocalPlayer)return;
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // if (isLocalPlayer)
        // {
        //
        // }  

        if (!interact.instance.isMotor)
        {
            Move();
        }
        else
        {
            anims.SetBool("drive",interact.instance.isMotor);
            rb.useGravity = false;
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

        // Kamera hareketi
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        // Kamerayı döndür
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, mouseX, 0);

        // Kamerayı döndür
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, mouseX, 0);
        // float x = Input.GetAxis("Horizontal");
        // float z = Input.GetAxis("Vertical");
        //
        // Vector3 movement = new Vector3(x, 0.0f, z);
        // movement = movement.normalized * speed * Time.deltaTime;
        //
        // transform.Translate(movement);

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