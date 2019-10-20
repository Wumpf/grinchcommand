using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SledSpawner : MonoBehaviour
{
    public GameObject SledPrefab;
    public BoxCollider2D SpawnZoneLeft;
    public BoxCollider2D SpawnZoneRight;

    public float SledSpawnFrequency = 0.2f; // in Hz
    public float SledSpawnFrequencyJitter = 0.5f; // In %

    private float presentSpawnTimeMin => 1.0f / (SledSpawnFrequency * (1.0f - SledSpawnFrequencyJitter));
    private float presentSpawnTimeMax => 1.0f / (SledSpawnFrequency * (1.0f + SledSpawnFrequencyJitter));

    void Start()
    {
        StartCoroutine(SpawnSled());
    }

    Vector3 RandomPositionInSpawnZone(BoxCollider2D zone)
    {
        return new Vector3(
            Random.Range(zone.bounds.min.x, zone.bounds.max.x),
            Random.Range(zone.bounds.min.y, zone.bounds.max.y),
            Random.Range(zone.bounds.min.z, zone.bounds.max.z)
        );
    }

    IEnumerator SpawnSled()
    {
        while (this.isActiveAndEnabled)
        {
            bool left = Random.value > 0.5f;
            var spawnZone = left ? SpawnZoneLeft : SpawnZoneRight;
            var newSled = Instantiate(SledPrefab, RandomPositionInSpawnZone(spawnZone), Quaternion.identity);
            newSled.GetComponent<Sled>().Speed *= left ? 1.0f : -1.0f;
            yield return new WaitForSeconds(Random.Range(presentSpawnTimeMin, presentSpawnTimeMax));
        }
    }
}
