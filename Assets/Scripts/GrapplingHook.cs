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

    private Vector3 targetPos;

    public GameObject TargetCrosshair;

    public float MoveSpeed = 10.0f;
    private const float maxHeight = 2.0f;


    void FixedUpdate()
    {
        // Doing this here ensures noone else needs to care about enable order...
        TargetCrosshair.SetActive(state == State.MovingToTarget);
        if (state == State.Idle)
            return;

        var targetDirection = (targetPos - transform.parent.transform.position).normalized;
        var currentDirection = (targetPos - transform.position);

        if (transform.position.y > maxHeight || Vector3.Dot(targetDirection, currentDirection) < 0.0f)
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
            targetPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y));
            state = State.MovingToTarget;

            TargetCrosshair.transform.position = targetPos;
        }
        else if (state == State.MovingToTarget && context.phase == InputActionPhase.Canceled)
            state = State.Retracting;
    }
}
