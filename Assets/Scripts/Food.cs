using UnityEngine;

public class Food : MonoBehaviour
{
    public GameObject gameBoundary;

    private void Start()
    {
        GenerateRandomSpawnPosition();
    }

    public void GenerateRandomSpawnPosition()
    {
        // Grab Bounding box of gameBoundary and generate random spawn position
        Bounds boundingBox = gameBoundary.GetComponent<BoxCollider2D>().bounds;
        float spawnPositionX = Random.Range(boundingBox.max.x, boundingBox.min.x);
        float spawnPositionY = Random.Range(boundingBox.max.y, boundingBox.min.y);

        transform.position = new Vector3(
            Mathf.Round(spawnPositionX),
            Mathf.Round(spawnPositionY),
            0.0f
            );

    }
}
