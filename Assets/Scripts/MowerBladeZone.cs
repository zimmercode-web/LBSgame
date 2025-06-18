using UnityEngine;

public class MowerBladeZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        var grass = other.GetComponent<MowableGrass>();
        if (grass != null)
        {
            grass.Cut(); // Call the Cut method on the MowableGrass component
        }
        
        
    }
}
