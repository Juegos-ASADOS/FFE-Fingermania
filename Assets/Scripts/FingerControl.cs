using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class FingerControl : MonoBehaviour
{
    bool leftAttacking, rightAttacking;
    Vector2 leftAxes = new Vector2(), rightAxes = new Vector2();

    Vector3 lHeadIniPos;

    [SerializeField]
    Vector3 leftHeadLimits;

    [SerializeField]
    float maxForce, forceFactor;

    [SerializeField]
    Transform leftHead, leftAttackBone, rightMoveBone, rightAttackBone;

    [SerializeField]
    Rigidbody leftRB;

    private void Start()
    {
        lHeadIniPos = leftHead.position;
    }

    public void OnLeftFingerMove(CallbackContext context)
    {
        leftAxes = context.ReadValue<Vector2>();

        //leftMoveBone.rotation = Quaternion.Euler(leftAxes.y * 45f, 0, -leftAxes.x * 45f);
    }

    public void OnRightFingerMove(CallbackContext context)
    {
        Debug.Log("Right " + context.ReadValue<Vector2>());
    }

    public void OnLeftFingerAttack(CallbackContext context)
    {
        if (context.ReadValue<float>() >= 0.9 && !leftAttacking)
        {
            leftAttacking = true;
        }
        else if (leftAttacking && context.ReadValue<float>() <= 0.1)
            leftAttacking = false;
    }

    public void OnRightFingerAttack(CallbackContext context)
    {
        if (context.ReadValue<float>() >= 0.9 && !rightAttacking)
        {
            rightAttacking = true;
        }
        else if (rightAttacking && context.ReadValue<float>() <= 0.1)
            rightAttacking = false;
    }

    public void FixedUpdate()
    {
        if (Mathf.Abs(leftAxes.x) >= 0.1 || Mathf.Abs(leftAxes.y) >= 0.1)
        {
            Vector3 xForce = leftRB.GetAccumulatedForce();
            if (Mathf.Abs(xForce.x) < maxForce)
                leftRB.AddForce(new Vector3(leftAxes.x * forceFactor, 0, 0));
            if (Mathf.Abs(xForce.z) < maxForce)
                leftRB.AddForce(new Vector3(0, 0, leftAxes.y * forceFactor));

            if (Mathf.Abs(leftHead.position.x + leftAxes.x - lHeadIniPos.x) >= leftHeadLimits.x)
            {
                leftRB.linearVelocity = new Vector3(0, leftRB.linearVelocity.y, leftRB.linearVelocity.z);
            }
            if (Mathf.Abs(leftHead.position.z + leftAxes.y - lHeadIniPos.z) >= leftHeadLimits.z)
            {
                leftRB.linearVelocity = new Vector3(leftRB.linearVelocity.x, leftRB.linearVelocity.y, 0);
            }
        }
    }
}
