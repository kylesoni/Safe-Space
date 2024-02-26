using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;

    public float offset;

    public Tilemap map;

    public int spawnCooldown = 1;
    private float spawnTimer = 0;

    private Day_Night clock;
    private Camera cam;
    private Movement player;

    // Start is called before the first frame update
    void Start()
    {
        clock = FindObjectOfType<Day_Night>();
        cam = FindObjectOfType<Camera>();
        player = FindObjectOfType<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnTimer <= 0)
        {
            SpawnEnemy();
            spawnTimer = spawnCooldown;
        }
        
        if (spawnTimer > 0)
        {
            spawnTimer -= Time.deltaTime;
        }
    }

    private void SpawnEnemy()
    {
        int enemyIndex = Random.Range(0, enemyPrefabs.Length);
        Vector2 newSpawn = GetSpawnPoint();
        var hitColliders = Physics.OverlapSphere(newSpawn, 1);
        if (hitColliders.Length <= 0.1)
        {
            GameObject.Instantiate(enemyPrefabs[enemyIndex], GetSpawnPoint(), Quaternion.identity);
        }
    }

    private Vector2 GetSpawnPoint()
    {
        float counter = 0;
        while (counter < 2)
        {
            float actOffset = offset + 15f;
            float hOffset = Random.Range(15f, actOffset) * Mathf.Sign(Random.Range(-1, 1));
            float vOffset = Random.Range(0, offset) * Mathf.Sign(Random.Range(-1, 1));
            Vector3 spawnPoint = new Vector3(hOffset, vOffset, 0) + player.transform.position;
            Vector3Int cellPosition = map.WorldToCell(spawnPoint);

            if (cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).x > spawnPoint.x || cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).y > spawnPoint.y || cam.ViewportToWorldPoint(new Vector3(1, 1, 0)).x < spawnPoint.x)
            {
                if (map.GetTile(cellPosition) == null)
                {
                    return spawnPoint;
                }
            }
            counter += Time.deltaTime;
        }
        return new Vector2(0, 0);
    }
}
