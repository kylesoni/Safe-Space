using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMining : MonoBehaviour
{
    public Tilemap map;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse click
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = map.WorldToCell(worldPoint);
            map.SetTile(cellPosition, null); // Remove the tile at the clicked position
        }
    }
}