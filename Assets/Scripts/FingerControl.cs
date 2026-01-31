using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class FingerControl : MonoBehaviour
{
    Vector2 leftAxes = new Vector2(), rightAxes = new Vector2();

    Vector3 lHeadIniPos, rHeadIniPos;

    [SerializeField]
    Vector3 leftHeadLimits, rightHeadLimits;

    float leftAttack = 0, rightAttack = 0;

    [SerializeField]
    Transform leftHead, rightHead;

    [SerializeField]
    Rigidbody leftRb, rightRb;

    [SerializeField]
    float looserCooldown, winnerCooldown, tieCooldown;

    private bool leftBloqued, rightBloqued;
    private float timerLeft, timerRight;
    private float leftSpeed = 0, rightSpeed = 0;

    private float colCooldown = 0;
    [SerializeField]
    float timeBetweenColisions;

    bool managingCollision;

    private void Start()
    {
        lHeadIniPos = leftHead.position;
        rHeadIniPos = rightHead.position;
    }

    public void OnLeftFingerMove(CallbackContext context)
    {
        leftAxes = context.ReadValue<Vector2>();
    }

    public void OnRightFingerMove(CallbackContext context)
    {
        rightAxes = context.ReadValue<Vector2>();
    }

    public void OnLeftFingerAttack(CallbackContext context)
    {
        leftAttack = context.ReadValue<float>();
    }

    public void OnRightFingerAttack(CallbackContext context)
    {
        rightAttack = context.ReadValue<float>();
    }

    public void Update()
    {
        if(colCooldown > 0)
            colCooldown -= Time.deltaTime; 

        if (!leftBloqued)
        {
            Vector3 lPosPre = leftHead.position;
            leftHead.position = new Vector3(lHeadIniPos.x + (leftAxes.x * leftHeadLimits.x), lHeadIniPos.y - (leftHeadLimits.y * leftAttack), lHeadIniPos.z + (leftAxes.y * leftHeadLimits.z) + (leftHeadLimits.y * leftAttack));
            Vector3 posDiff = leftHead.position - lPosPre;
            leftSpeed = Mathf.Abs(posDiff.magnitude);
        }
        else
        {
            timerLeft -= Time.deltaTime;
            if (timerLeft <= 0f)
                leftBloqued = false;
        }

        if (!rightBloqued)
        {
            Vector3 rPosPre = rightHead.position;
            rightHead.position = new Vector3(rHeadIniPos.x - (rightAxes.x * rightHeadLimits.x), rHeadIniPos.y - (rightHeadLimits.y * rightAttack), rHeadIniPos.z - (rightAxes.y * rightHeadLimits.z) - (rightHeadLimits.y * rightAttack));
            Vector3 posDiff = rightHead.position - rPosPre;
            rightSpeed = Mathf.Abs(posDiff.magnitude);
        }
        else
        {
            timerRight -= Time.deltaTime;
            if (timerRight <= 0f)
                rightBloqued = false;
        }
    }

    public void DeactiveFinger(bool leftFinger, float time)
    {
        if (leftFinger)
        {
            leftBloqued = true;
            timerLeft = time;
        }
        else
        {
            rightBloqued = true;
            timerRight = time;
        }
    }

    public float GetFingerSpeed(bool isLeft)
    {
        return isLeft ? leftSpeed : rightSpeed;
    }

    public void ManageFingerColision(Vector3 colPoint)
    {
        if (colCooldown > 0) return;

        if (managingCollision)
        {
            colCooldown = timeBetweenColisions;
            managingCollision = false;
            return;
        }

        Vector3 leftDir = leftHead.position - colPoint, rightDir = rightHead.position - colPoint;
        leftDir.y = rightDir.y = 0;
        float leftForce, rightForce, leftTime, rightTime;

        managingCollision = true;
        if (leftSpeed > rightSpeed)
        {
            leftForce = rightSpeed / 10f;
            rightForce = leftSpeed;
            leftTime = winnerCooldown;
            rightTime = looserCooldown;
        }
        else if (rightSpeed > leftSpeed)
        {
            leftForce = rightSpeed;
            rightForce = leftSpeed / 10f;
            leftTime = looserCooldown;
            rightTime = winnerCooldown;
        }
        else
        {
            leftForce = rightSpeed / 5f;
            rightForce = leftSpeed / 5f;
            leftTime = rightTime = tieCooldown;
        }

        DeactiveFinger(true, leftTime);
        DeactiveFinger(false, rightTime);
        rightRb.AddForce(rightForce * rightDir.normalized * 1000f, ForceMode.Impulse);
        leftRb.AddForce(leftForce * leftDir.normalized * 1000f, ForceMode.Impulse);
    }

    //private void OnDrawGizmos()
    //{
    //    //Gizmos.DrawCube(lHeadIniPos, leftHeadLimits);
    //    Gizmos.color = Color.white;
    //    Gizmos.DrawWireCube(lHeadIniPos, leftHeadLimits * 2);
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(new Vector3(lHeadIniPos.x, lHeadIniPos.y, lHeadIniPos.z + leftAttack * 2), leftHeadLimits * 2);
    //}
}
