using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GrassDenseGridSpawner : MonoBehaviour
{
    [Tooltip("Your grass prefab")]
    public GameObject grassPrefab;

    [Tooltip("How many clumps per base cell axis. 1 = one clump per prefab size, 2 = two clumps per prefab size, etc.")]
    [Min(1)]
    public int density = 1;

    [Tooltip("Margin to overlap beyond prefab size so no gaps (in world units).")]
    public float overlapMargin = 0f;

    [Tooltip("Y-offset so grass sits exactly on the ground")]
    public float yOffset = 0f;

    private Collider areaCollider;
    private Vector3 prefabExtents;

    void Awake()
    {
        // Measure half-size of the prefab
        var temp = Instantiate(grassPrefab, Vector3.zero, Quaternion.identity);
        var rends = temp.GetComponentsInChildren<Renderer>();
        Bounds b = new Bounds(rends[0].bounds.center, Vector3.zero);
        foreach (var r in rends) b.Encapsulate(r.bounds);
        prefabExtents = b.extents;
        Destroy(temp);
    }

    void Start()
    {
        areaCollider = GetComponent<Collider>();
        SpawnDenseGrid();
    }

    void SpawnDenseGrid()
    {
        Bounds area = areaCollider.bounds;

        // inset by prefab extents so no clumps poke through the fence
        Vector3 min = area.min + new Vector3(prefabExtents.x, 0, prefabExtents.z);
        Vector3 max = area.max - new Vector3(prefabExtents.x, 0, prefabExtents.z);

        // compute cell size: (full prefab size minus any margin) / density
        float cellX = (prefabExtents.x * 2f - overlapMargin) / density;
        float cellZ = (prefabExtents.z * 2f - overlapMargin) / density;

        // how many fit?
        int cols = Mathf.FloorToInt((max.x - min.x) / cellX) + 1;
        int rows = Mathf.FloorToInt((max.z - min.z) / cellZ) + 1;

        // stagger every other row for full coverage
        for (int j = 0; j < rows; j++)
        {
            float z = min.z + j * cellZ;
            float rowOffset = (j % 2) * (cellX * 0.5f);
            for (int i = 0; i < cols; i++)
            {
                float x = min.x + i * cellX + rowOffset;
                if (x > max.x) continue;
                Vector3 pos = new Vector3(x, area.min.y + yOffset, z);
                Instantiate(grassPrefab, pos, Quaternion.identity, transform);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!areaCollider) areaCollider = GetComponent<Collider>();
        if (areaCollider == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(areaCollider.bounds.center, areaCollider.bounds.size);
    }
}
