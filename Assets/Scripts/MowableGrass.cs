using UnityEngine;
using UnityEngine.Rendering;

public class MowableGrass : MonoBehaviour
{

    [Header("Growth Settings")] 
    private Vector3 originalScale; // Store the original scale of the grass
    public float minHeight = 0.65f; // Minimum height for the grass to be considered "cut"
    public float maxHeight = 2.75f; // Maximum height for the grass to be considered "cut"
    public float regrowDelay = 360f; // Time in seconds before the grass regrows
    public float regrowDuration = 600f; // Time in seconds for the grass to fully regrow

    private float growthStartTime;
    private bool isGrowing = false;
    public bool isCut = false;


    [Header("Zone Tracking")]
    public MowingJobZone assignedZone; // The zone this grass belongs to

    private void Start()
    {
        originalScale = transform.localScale; // Store the original scale of the grass
        SetHeight(maxHeight); // Set initial height to maximum

        if (assignedZone == null)
        {
            assignedZone = GetComponentInParent<MowingJobZone>(); // Try to find the zone in parent objects if not assigned
        }
        if (assignedZone != null)
        {
            assignedZone.RegisterGrass(this); // Register this grass in the assigned zone
        }
       
    }

    private void Update()
    {
        if (isGrowing)
        {
            float elapsed = Time.time - growthStartTime;
            float t = Mathf.Clamp01(elapsed / regrowDuration);
            float currentHeight = Mathf.Lerp(minHeight, maxHeight, t);
            SetHeight(currentHeight);

            if (t >= 1f)
            {
                isGrowing = false;
                isCut = false; // Reset cut state when regrowth is complete
            }
        }
    }
    public void Cut()
    {
        if (isCut) return; // Prevent cutting if already cut

        SetHeight(minHeight); // Set height to minimum
        isCut = true; // Mark as cut
        isGrowing = false; // Stop any ongoing growth

        if (assignedZone != null)
        {
            
            assignedZone.NotifyGrassCut(this); // Notify the assigned zone that this grass has been cut
        }


    }
    
    void StartRegrowth()
    {
        Debug.Log($"Starting regrowth for grass in zone: {assignedZone.name}"); // Log the start of regrowth
        growthStartTime = Time.time; // Record the time when regrowth starts
        isGrowing = true; // Start the growth process
    }

    void SetHeight(float yScale)
    {
        Vector3 baseScale = new Vector3(1f, yScale, 1f); // Base scale for the grass
        transform.localScale = Vector3.Scale(originalScale, baseScale); // Apply the original scale to the new height
    }

    public bool IsStillGrowing()
    {
               return isGrowing; // Return whether the grass is currently growing
    }

   public void BeginRegrowthFromZone()
    {
        growthStartTime = Time.time; // Record the time when regrowth starts
        isGrowing = true; // Start the growth process
    }
}
