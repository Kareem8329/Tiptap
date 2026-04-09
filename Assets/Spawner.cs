using System.Globalization;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject circlePrefab;
    private BoxCollider2D boxCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    Vector2 GetSpawnPosition()
    {

        float leftBound = boxCollider.bounds.min.x;
        float rightBound = boxCollider.bounds.max.x;
        float topBound = boxCollider.bounds.max.y;
        float bottomBound = boxCollider.bounds.min.y;
        float randomX = Random.Range(leftBound, rightBound);
        float randomY = Random.Range(bottomBound, topBound);
        Vector2 spawnPosition = new Vector2(randomX, randomY);
        return spawnPosition;
    }

    public void SpawnCircle(int difficulty)
    {
        int numOfCircles = Random.Range(1, difficulty + 1);
        for (int i = 0; i < numOfCircles; i++)
        {
            Vector2 spawnPosition = GetSpawnPosition();
            Instantiate(circlePrefab, spawnPosition, Quaternion.identity);
            circlePrefab.GetComponent<Shrink>().shrinkRate += difficulty * 0.01f;
        }
    }
}
