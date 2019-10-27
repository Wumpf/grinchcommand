using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameState))]
public class SledSpawner : MonoBehaviour
{
    public GameObject SledPrefab;
    public BoxCollider2D SpawnZoneLeft;
    public BoxCollider2D SpawnZoneRight;

    public float SledSpawnFrequencyJitter = 0.5f; // In %
    public float SledSpeedVariation = 0.3f;

    private float presentSpawnTimeMin => 1.0f / (GetComponent<GameState>().CurrentSledSpawnFrequency * (1.0f - SledSpawnFrequencyJitter));
    private float presentSpawnTimeMax => 1.0f / (GetComponent<GameState>().CurrentSledSpawnFrequency * (1.0f + SledSpawnFrequencyJitter));

    public int NumSledsLeft { get; private set; }

    private void OnEnable()
    {
        NumSledsLeft = GetComponent<GameState>().CurrentNumSledsToSpawn;
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
        while (this.isActiveAndEnabled && NumSledsLeft > 0)
        {
            NumSledsLeft--;
            bool right = Random.value > 0.5f;
            //Debug.Log($"New sled from right={right}");
            var spawnZone = right ? SpawnZoneRight : SpawnZoneLeft;
            var newSled = Instantiate(SledPrefab, RandomPositionInSpawnZone(spawnZone), Quaternion.identity);
            var sledControl = newSled.GetComponent<Sled>();
            sledControl.Speed += Random.Range(0.0f, SledSpeedVariation);
            sledControl.Speed *= right ? 1.0f : -1.0f;
            sledControl.PresentSpawnFrequency = GetComponent<GameState>().CurrentPresentSpawnFrequency;
            if (right)
                newSled.transform.localScale = new Vector3(-newSled.transform.localScale.x, newSled.transform.localScale.y, newSled.transform.localScale.z);
            yield return new WaitForSeconds(Random.Range(presentSpawnTimeMin, presentSpawnTimeMax));
        }
    }
}
