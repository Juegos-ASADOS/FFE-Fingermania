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

    float leftAttack = 0, leftAttackCounter = 0;

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
    }

    public void OnRightFingerMove(CallbackContext context)
    {
        Debug.Log("Right " + context.ReadValue<Vector2>());
    }

    public void OnLeftFingerAttack(CallbackContext context)
    {
        leftAttackCounter += leftAttack - context.ReadValue<float>();
        leftAttack = context.ReadValue<float>();
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

        }

        //if (leftAttackCounter != 0f) {
        //    Debug.Log(leftAttackCounter);
        //    leftRB.AddForce(new Vector3(0, 0, leftAttackCounter * forceFactor));
        //    leftAttackCounter = 0;
        //}
        if (Mathf.Abs(leftHead.position.x - lHeadIniPos.x) >= leftHeadLimits.x)
        {
            leftRB.linearVelocity = new Vector3(0, leftRB.linearVelocity.y, leftRB.linearVelocity.z);
            leftHead.position = new Vector3((lHeadIniPos.x + leftHeadLimits.x) * Mathf.Sign(leftHead.position.x), leftHead.position.y, leftHead.position.z);
        }
        if (Mathf.Abs(leftHead.position.z - lHeadIniPos.z - leftAttack * 2) >= leftHeadLimits.z)
        {
            leftRB.linearVelocity = new Vector3(leftRB.linearVelocity.x, leftRB.linearVelocity.y, 0);
            leftHead.position = new Vector3(leftHead.position.x, leftHead.position.y, (lHeadIniPos.z + leftHeadLimits.z) * Mathf.Sign(leftHead.position.z) + leftAttack * 2);
        }
    }

    public void Update()
    {
        leftHead.position = new Vector3(leftHead.position.x, lHeadIniPos.y - (leftHeadLimits.y * leftAttack), leftHead.position.z);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawCube(lHeadIniPos, leftHeadLimits);
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(lHeadIniPos, leftHeadLimits * 2);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(lHeadIniPos.x, lHeadIniPos.y, lHeadIniPos.z + leftAttack * 2), leftHeadLimits * 2);
    }
}
