using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class GrapplingHookRope : MonoBehaviour
{
    void Update()
    {
        GetComponent<LineRenderer>().SetPositions(new Vector3[] {
            new Vector3(transform.position.x, transform.position.y, 1.0f),
            new Vector3(transform.parent.position.x, transform.parent.position.y, 1.0f)
        });

        var localPos = transform.localPosition;
        if (Mathf.Abs(localPos.x) > 0.01f)
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(localPos.y, localPos.x) * Mathf.Rad2Deg - 90.0f);
        else
            transform.localRotation = Quaternion.identity;
    }
}
