﻿using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class GrapplingHookRope : MonoBehaviour
{
    void Update()
    {
        GetComponent<LineRenderer>().SetPositions(new Vector3[] {
            new Vector3(transform.position.x, transform.position.y, 0.1f),
            new Vector3(transform.parent.position.x, transform.parent.position.y, 0.1f)
        });
    }
}
