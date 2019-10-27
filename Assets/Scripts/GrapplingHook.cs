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

    private void DestroyAttachedPresents()
    {
        for (int i=0; i<transform.childCount; ++i)
        {
            var child = transform.GetChild(i);
            if (child.GetComponent<Present>() != null)
                GameObject.Destroy(child.gameObject);
        }
    }

    private void OnDisable() => DestroyAttachedPresents();

    private void FixedUpdate()
    {
        // Doing this here ensures noone else needs to care about enable order...
        TargetCrosshair.SetActive(state == State.MovingToTarget);
        if (state == State.Idle)
            return;

        if (transform.position.y > maxHeight)
            state = State.Retracting;

        if (state == State.Retracting &&
            Vector3.Distance(transform.parent.position, transform.position) < 0.02f)
        {
            state = State.Idle;
            DestroyAttachedPresents();
            transform.localRotation = Quaternion.identity;
            return;
        }

        var targetDirection = (targetPos - transform.parent.transform.position).normalized;
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

            var targetDirection = (targetPos - transform.parent.transform.position);
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90.0f);

            TargetCrosshair.transform.position = targetPos;
        }
        else if (state == State.MovingToTarget && context.phase == InputActionPhase.Canceled)
            state = State.Retracting;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Present>() != null)
        {
            other.gameObject.GetComponent<Present>().enabled = false;
            other.transform.parent = transform;
            other.rigidbody.simulated = false;
        }
    }
}
