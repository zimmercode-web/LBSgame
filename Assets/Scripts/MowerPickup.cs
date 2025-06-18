using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MowerPickup : MonoBehaviour
{
    public float maxSpeed = 10f; // Max speed of the mower
    public bool isBlocked = false; // Flag to block pickup
    public float pickupDistance = 3f;
    public TextMeshProUGUI pickupPrompt;
    public Transform mowerHoldPoint;

    private GameObject player;
    private Rigidbody rb;
    private bool isHeld = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();

        if (pickupPrompt != null)
            pickupPrompt.gameObject.SetActive(false);

        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (!isHeld && distance < pickupDistance)
        {
            Debug.Log("Press 'E' to pick up the mower");
            if (pickupPrompt != null && !pickupPrompt.gameObject.activeSelf)
                pickupPrompt.gameObject.SetActive(true);

            if (Keyboard.current.eKey.wasPressedThisFrame)
                PickUpMower();
        }
        else if (!isHeld && pickupPrompt != null)
        {
            pickupPrompt.gameObject.SetActive(false);
        }

        if (isHeld && Keyboard.current.qKey.wasPressedThisFrame)
            DropMower();

        if (isHeld && mowerHoldPoint != null)
        {
            Vector3 targetPosition = mowerHoldPoint.position;
            Quaternion targetRotation = mowerHoldPoint.rotation;

            Vector3 toTarget = targetPosition - rb.position;
            float moveSpeed = 20f; // Adjust as needed for responsiveness
            Vector3 desiredVelocity = toTarget * moveSpeed;
            float maxSpeed = 10f;  // Max units per second (experiment with this)
            desiredVelocity = Vector3.ClampMagnitude(desiredVelocity, maxSpeed);
            rb.linearVelocity = desiredVelocity;

            
            float maxRotationPerFrame = 180f; // degrees per second
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, maxRotationPerFrame * Time.deltaTime));
        }
    }


    void PickUpMower()
    {
        isHeld = true;
        rb.isKinematic = false;
        rb.detectCollisions = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        transform.SetParent(null); // detach from any previous parent

        if (pickupPrompt != null)
            pickupPrompt.gameObject.SetActive(false);
    }

    void DropMower()
    {
        isHeld = false;
        rb.isKinematic = false;
        rb.detectCollisions = true;

        transform.SetParent(null);
    }

    private void OnCollisionEnter(Collision collision)
    {
        isBlocked = true; // Set the flag when a collision occurs
    }
    
    private void OnCollisionExit(Collision collision)
    {
        isBlocked = false; // Reset the flag when no longer colliding
    }
}