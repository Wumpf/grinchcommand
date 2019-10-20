﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sled : MonoBehaviour
{
    public float Speed = 1.0f;
    public float PresentSpawnFrequency = 0.8f; // in Hz
    public float PresentSpawnFrequencyJitter = 0.5f; // In %

    private float presentSpawnTimeMin => 1.0f / (PresentSpawnFrequency * (1.0f - PresentSpawnFrequencyJitter));
    private float presentSpawnTimeMax => 1.0f / (PresentSpawnFrequency * (1.0f + PresentSpawnFrequencyJitter));

    public GameObject PresentPrefab;

    private Random random = new Random();

    void Start()
    {
        StartCoroutine(SpawnPresent());
    }

    void Update()
    {
        transform.Translate(Speed * Time.deltaTime, 0, 0);
        if (Camera.main.WorldToViewportPoint(transform.position).x * Mathf.Sign(Speed) > 1.0f)
            Destroy(this.gameObject);
    }

    IEnumerator SpawnPresent()
    {
        while (this.isActiveAndEnabled)
        {
            yield return new WaitForSeconds(Random.Range(presentSpawnTimeMin, presentSpawnTimeMax));
            Instantiate(PresentPrefab, transform.position, Quaternion.identity);
        }
    }
}
