using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SeedSpawnConfig", menuName = "Seed/Spawn Config", order = 1)]
public class SeedSpawnConfig : ScriptableObject
{
    [Range(0f, 1f)]
    public float spawnProbability = 0.5f;

    public float minSpawnTime = 1f;
    public float maxSpawnTime = 10f;

    public Vector3 spawnAreaSize = new Vector3(10f, 0f, 10f);
}
