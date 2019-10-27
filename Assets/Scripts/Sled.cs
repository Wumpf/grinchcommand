using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sled : MonoBehaviour
{
    public float Speed = 1.0f;
    public float PresentSpawnFrequency = 0.8f; // in Hz
    public float PresentSpawnFrequencyJitter = 0.5f; // In %

    private float presentSpawnTimeMin => 1.0f / (PresentSpawnFrequency * (1.0f - PresentSpawnFrequencyJitter));
    private float presentSpawnTimeMax => 1.0f / (PresentSpawnFrequency * (1.0f + PresentSpawnFrequencyJitter));

    public GameObject[] PresentPrefabs;


    void Start()
    {
        StartCoroutine(SpawnPresent());
    }

    void Update()
    {
        transform.Translate(Speed * Time.deltaTime, 0, 0);
        float viewportX = Camera.main.WorldToViewportPoint(transform.position).x;
        if (viewportX > 1.2f || viewportX < -0.2f)
            Destroy(this.gameObject);
    }

    IEnumerator SpawnPresent()
    {
        while (this.isActiveAndEnabled)
        {
            yield return new WaitForSeconds(Random.Range(presentSpawnTimeMin, presentSpawnTimeMax));
            Instantiate(PresentPrefabs[Random.Range(0, PresentPrefabs.Length)], transform.position, Quaternion.identity);
        }
    }
}
