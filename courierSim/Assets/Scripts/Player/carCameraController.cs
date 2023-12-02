
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class carCameraController : MonoBehaviour
{
    // kamerayı oyuncuya atar ve kamerayı tpp takip
    // public override void OnStartLocalPlayer()
    // {
    //     Camera.main.transform.SetParent(transform);
    //     //Camera.main.transform.localPosition = new Vector3(0, 0, 0);
    // }
    
    public float distance = 5.0f; // Distance between the player and camera
    public float rotationSpeed = 5.0f; // Speed at which the camera rotates
    public float resetSpeed = 2.0f; // Speed at which the camera resets its rotation
    public Vector2 rotationLimits = new Vector2(-80f, 80f); // Vertical rotation limits

    private float currentX = 0.0f;
    private float currentY = 0.0f;

    private void Start()
    {
       // if(!isLocalPlayer)return;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

       // if(!isLocalPlayer)return;
        // Rotate the camera based on player input
        currentX += Input.GetAxis("Mouse X") * rotationSpeed;
        currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;

        currentY = Mathf.Clamp(currentY, rotationLimits.x, rotationLimits.y);

        // Calculate rotation and position
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + this.transform.position;

        //Apply rotation and position to the camera
        Camera.main.transform.rotation = rotation;
        Camera.main.transform.position = position;
    }

}