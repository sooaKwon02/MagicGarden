using System.Collections;
using UnityEngine;

public class FieldSeedSpawner : MonoBehaviour
{
    public GameObject seedPrefab;
    public GameObject flowerPrefab;
    public SeedSpawnConfig spawnConfig;

    // 씨앗이 꽃으로 바뀌는 데 걸리는 시간 (초입니다.)
    public float timeToBloom = 10f;

    // 밭에 들어온 플레이어가 씨앗을 한 번만 생성
    private bool hasSpawned = false;
    // 플레이어가 밭(트리거 영역)에 들어오면 실행 (Collider의 Is Trigger가 체크된 밭 오브젝트에 붙어 있어야 함)
    private void OnTriggerEnter(Collider other)
    {
        if (!hasSpawned && other.CompareTag("Player"))
        {
            hasSpawned = true;
            // 코루틴을 시작하여 설정한 시간 후 씨앗을 생성하고, 확률에 따라 씨앗을 심는다.
            StartCoroutine(SpawnSeedOnce());
        }
    }

    IEnumerator SpawnSeedOnce()
    {
        // SeedSpawnConfig에 설정한 최소 시간과 최대 시간 사이의 랜덤 대기 후 실행
        float waitTime = Random.Range(spawnConfig.minSpawnTime, spawnConfig.maxSpawnTime);
        yield return new WaitForSeconds(waitTime);

        // 0~1 사이의 랜덤 숫자 생성하여 씨앗 생성 확률 판정
        float roll = Random.Range(0f, 1f);
        if (roll <= spawnConfig.spawnProbability)
        {
            // 씨앗 생성 후, 씨앗이 꽃으로 바뀌는 코루틴 시작
            GameObject seed = SpawnRandomSeed();
            StartCoroutine(ChangeSeedToFlower(seed));
        }
    }

    // 밭 오브젝트의 위치를 기준으로, SeedSpawnConfig에 설정된 영역 내에서 씨앗을 생성
    GameObject SpawnRandomSeed()
    {
        Vector3 center = transform.position;
        Vector3 randomPos = center + new Vector3(
            Random.Range(-spawnConfig.spawnAreaSize.x * 0.5f, spawnConfig.spawnAreaSize.x * 0.5f),
            0f,  // Y축은 밭 표면에 심기 위해 0으로 함 (필요 시 조정)
            Random.Range(-spawnConfig.spawnAreaSize.z * 0.5f, spawnConfig.spawnAreaSize.z * 0.5f)
        );
        return Instantiate(seedPrefab, randomPos, Quaternion.identity);
    }

    // 일정 시간이 지나면 씨앗을 꽃으로 바꾸는 코루틴
    IEnumerator ChangeSeedToFlower(GameObject seed)
    {
        yield return new WaitForSeconds(timeToBloom);
        if (seed != null)
        {
            Vector3 pos = seed.transform.position;
            Destroy(seed);
            Instantiate(flowerPrefab, pos, Quaternion.identity);
        }
    }
}
