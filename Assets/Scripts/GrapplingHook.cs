using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingHook : MonoBehaviour
{
    enum State
    {
        Idle,
        MovingToTarget,
        Retracting
    }

    private State state = State.Idle;

    private Vector3 target;

    public float MoveSpeed = 10.0f;

    void FixedUpdate()
    {
        if (state == State.Idle)
            return;

        var toTarget = target - transform.position;
        float distance = toTarget.magnitude;
        if (distance < 0.02f)
        {
            if (state == State.MovingToTarget)
            {
                target = transform.parent.position;
                state = State.Retracting;
            }
            else
            {
                state = State.Idle;
                return;
            }
        }

        float movement = Mathf.Min(1.0f, MoveSpeed * Time.fixedDeltaTime / distance);
        transform.Translate(toTarget * movement);
    }

    public void OnFire()
    {
        if (state != State.Idle)
            return;

        var mousePos = Mouse.current.position.ReadValue();
        target = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y));
        state = State.MovingToTarget;
    }
}
