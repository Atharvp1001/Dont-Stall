using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private Vector2 spawnAreaMin;
    [SerializeField] private Vector2 spawnAreaMax;
    [SerializeField] private float spawnInterval = 2f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            Spawn();
        }
    }

    private void Spawn()
{
    while (true) // infinite attempts until success
    {
        Vector2 randomPos = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        // Check for any collider at the position
        Collider2D hit = Physics2D.OverlapCircle(randomPos, 0.5f);

        // If nothing is there, or if it’s not an obstacle/spawned object
        if (hit == null || (!hit.CompareTag("obstacle") && !hit.CompareTag("spawned")))
        {
            GameObject newObj = Instantiate(prefabToSpawn, randomPos, Quaternion.identity);
            newObj.tag = "spawned"; // mark it
            return;
        }
    }
}

}
