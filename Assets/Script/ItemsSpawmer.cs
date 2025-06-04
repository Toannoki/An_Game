using UnityEngine;

public class ItemsSpawmer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject itemPrefab; // Prefab item
    public int itemCount = 5;     // Số lượng item muốn tạo
    public Vector3 spawnAreaMin;  // Góc dưới bên trái khu vực spawn
    public Vector3 spawnAreaMax;  // Góc trên bên phải khu vực spawn


    void Start()
    {
        SpawnItems();
    }
    void Update()
    {
        
    }
    void SpawnItems()
    {
        for (int i = 0; i < itemCount; i++)
        {
            Vector3 randomXZ = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                50f,
                Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );

            RaycastHit hit;
            if (Physics.Raycast(randomXZ, Vector3.down, out hit, 100f))
            {
                Vector3 spawnPos = hit.point;

                // Tạo clone tạm thời
                GameObject clone = Instantiate(itemPrefab, spawnPos, Quaternion.identity);

                // Tìm Renderer trong clone
                Renderer rend = clone.GetComponentInChildren<Renderer>();
                if (rend == null)
                {
                    Debug.LogWarning("Không tìm thấy Renderer.");
                    continue;
                }

                // Tính nửa chiều cao
                float halfHeight = rend.bounds.size.y / 2f;

                // Điều chỉnh vị trí: hạ item xuống để nó lún 1/2 vào đất
                clone.transform.position = spawnPos - new Vector3(0, halfHeight * 0.5f, 0);
            }
        }
    }
}
