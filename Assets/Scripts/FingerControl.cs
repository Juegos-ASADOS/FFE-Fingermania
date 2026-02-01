using System;
using System.Threading;
using TMPro;
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

    [SerializeField]
    float hitForce;

    private float colCooldown = 0;
    [SerializeField]
    float timeBetweenColisions;

    bool managingCollision;

    Vector3 leftPreviousPos = Vector3.zero, rightPreviousPos = Vector3.zero;

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
            leftHead.position = Vector3.Lerp(leftHead.position, new Vector3(lHeadIniPos.x - (leftAxes.y * leftHeadLimits.x), lHeadIniPos.y - (leftHeadLimits.y * leftAttack), lHeadIniPos.z + (leftAxes.x * leftHeadLimits.z) + (leftHeadLimits.y * leftAttack)), 0.3f);
            Vector3 posDiff = leftHead.position - leftPreviousPos;
            leftSpeed = Mathf.Abs(posDiff.magnitude) / Time.deltaTime;
            leftPreviousPos = leftHead.position;
        }
        else
        {
            timerLeft -= Time.deltaTime;
            if (timerLeft <= 0f)
                leftBloqued = false;
        }

        if (!rightBloqued)
        {
            rightHead.position = Vector3.Lerp(rightHead.position, new Vector3(rHeadIniPos.x - (rightAxes.y * rightHeadLimits.x), rHeadIniPos.y - (rightHeadLimits.y * rightAttack), rHeadIniPos.z + (rightAxes.x * rightHeadLimits.z) - (rightHeadLimits.y * rightAttack)), 0.3f);
            Vector3 posDiff = rightHead.position - rightPreviousPos;
            rightSpeed = Mathf.Abs(posDiff.magnitude) / Time.deltaTime;
            rightPreviousPos = rightHead.position;
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

    public void ManageFingerColision(Vector3 leftPos, Vector3 rightPos)
    {
        if (colCooldown > 0) return;

        if (managingCollision)
        {
            colCooldown = timeBetweenColisions;
            managingCollision = false;
            return;
        }

        Vector3 leftDir = leftPos - rightPos, rightDir = rightPos - leftPos;
        leftDir.y = rightDir.y = 0;
        float leftTime, rightTime, leftForce = 0, rightForce = 0;

        managingCollision = true;
        if (leftSpeed > rightSpeed)
        {
            leftTime = winnerCooldown;
            rightTime = looserCooldown;
            rightForce = hitForce;
        }
        else if (rightSpeed > leftSpeed)
        {
            leftTime = looserCooldown;
            rightTime = winnerCooldown;
            leftForce = hitForce;
        }
        else
        {
            leftTime = rightTime = tieCooldown;
            leftForce = rightForce = hitForce;
        }
        DeactiveFinger(true, leftTime);
        DeactiveFinger(false, rightTime);
        rightRb.AddForce(rightForce * rightDir.normalized, ForceMode.Impulse);
        leftRb.AddForce(leftForce * leftDir.normalized, ForceMode.Impulse);
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
