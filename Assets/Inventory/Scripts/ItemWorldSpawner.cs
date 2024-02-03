using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorldSpawner : MonoBehaviour
{
    public Item item;

    private void Start() // Change from Awake and worked!
    {
        ItemWorld.SpawnItemWorld(transform.position, item);
        Destroy(gameObject);
    }
}
