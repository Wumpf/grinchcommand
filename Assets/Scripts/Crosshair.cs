using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private void Update()
    {
        var mousePos = UnityEngine.InputSystem.Mouse.current.position;
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x.ReadValue(), mousePos.y.ReadValue(), -1.0f));
    }
}