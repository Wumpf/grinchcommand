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

    private Vector3 targetDirection;

    public float MoveSpeed = 10.0f;
    private const float maxHeight = 2.4f;

    void FixedUpdate()
    {
        if (state == State.Idle)
            return;

        if (transform.position.y > maxHeight)
            state = State.Retracting;

        if (state == State.Retracting &&
            Vector3.Distance(transform.parent.position, transform.position) < 0.01f)
        {
            state = State.Idle;
            return;
        }

        if (state == State.Retracting)
            transform.Translate(-targetDirection * MoveSpeed * Time.fixedDeltaTime, Space.World);
        else
            transform.Translate(targetDirection * MoveSpeed * Time.fixedDeltaTime, Space.World);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (state == State.Idle && context.phase == InputActionPhase.Started)
        {
            var mousePos = Mouse.current.position.ReadValue();
            targetDirection = (Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y)) - transform.parent.position).normalized;
            state = State.MovingToTarget;
        }
        else if (state == State.MovingToTarget && context.phase == InputActionPhase.Canceled)
            state = State.Retracting;
    }
}
