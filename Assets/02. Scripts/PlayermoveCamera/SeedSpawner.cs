using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SeedSpawner : MonoBehaviour
{
    // 생성할 씨앗 프리팹 (Inspector에서 할당)
    public GameObject seedPrefab;

    // 씨앗 생성 설정 (ScriptableObject)
    public SeedSpawnConfig spawnConfig;

    void Start()
    {
        if (seedPrefab == null)
        {
            Debug.LogError("SeedPrefab이 할당되지 않았습니다!");
            return;
        }
        if (spawnConfig == null)
        {
            Debug.LogError("SpawnConfig가 할당되지 않았습니다!");
            return;
        }

        StartCoroutine(SpawnSeedOnce());
    }

    IEnumerator SpawnSeedOnce()
    {
        // 지정한 시간 범위 내에서 랜덤한 대기 시간 후 진행
        float waitTime = Random.Range(spawnConfig.minSpawnTime, spawnConfig.maxSpawnTime);
        yield return new WaitForSeconds(waitTime);

        // 0~1 사이의 랜덤 숫자를 생성하여 확률 판정
        float roll = Random.Range(0f, 1f);
        if (roll <= spawnConfig.spawnProbability)
        {
            SpawnRandomSeed();
        }
        else
        {
            Debug.Log("씨앗 생성 실패: 확률 판정 실패 (Roll: " + roll + ")");
        }
    }

    void SpawnRandomSeed()
    {
        // 이 오브젝트(SeedSpawner)의 위치를 기준으로 spawnConfig에 설정된 영역 내에서 랜덤 위치 계산
        Vector3 center = transform.position;
        Vector3 randomPos = center + new Vector3(
            Random.Range(-spawnConfig.spawnAreaSize.x * 0.5f, spawnConfig.spawnAreaSize.x * 0.5f),
            //Random.Range(-spawnConfig.spawnAreaSize.y * 0.5f, spawnConfig.spawnAreaSize.y * 0.5f),
            Random.Range(-spawnConfig.spawnAreaSize.z * 0.5f, spawnConfig.spawnAreaSize.z * 0.5f)
        );

        Instantiate(seedPrefab, randomPos, Quaternion.identity);
    }
}
