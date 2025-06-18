using UnityEngine;

public class PlayerModelFollow : MonoBehaviour
{
    public Transform playerBody;  // Reference to your PlayerCapsule or CharacterController root

    private Vector3 modelOffset; // Optional: offset if needed to align model

    void Start()
    {
        // Capture initial offset at runtime
        modelOffset = transform.localPosition;
    }

    void LateUpdate()
    {
        // Only rotate on Y axis to match body, prevent pitching weirdness
        Vector3 newEuler = new Vector3(0f, playerBody.rotation.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Euler(newEuler);
    
        transform.position = playerBody.position + modelOffset;
    }
}
