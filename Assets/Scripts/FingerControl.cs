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
    float looserCooldown, winnerCooldown, tieCooldown, minForce;

    private bool leftBloqued, rightBloqued;
    private float timerLeft, timerRight;
    private float leftSpeed = 0, rightSpeed = 0;
    private Vector3 leftPreviousPos = Vector3.zero, rightPreviousPos = Vector3.zero;

    private float colCooldown = 0;
    [SerializeField]
    float timeBetweenColisions;

    bool managingCollision;

    [SerializeField]
    TextMeshProUGUI text;

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
            leftHead.position = new Vector3(lHeadIniPos.x - (leftAxes.y * leftHeadLimits.x), lHeadIniPos.y - (leftHeadLimits.y * leftAttack), lHeadIniPos.z + (leftAxes.x * leftHeadLimits.z) + (leftHeadLimits.y * leftAttack));
        }
        else
        {
            timerLeft -= Time.deltaTime;
            if (timerLeft <= 0f)
                leftBloqued = false;
        }

        if (!rightBloqued)
        {
            rightHead.position = new Vector3(rHeadIniPos.x - (rightAxes.y * rightHeadLimits.x), rHeadIniPos.y - (rightHeadLimits.y * rightAttack), rHeadIniPos.z + (rightAxes.x * rightHeadLimits.z) - (rightHeadLimits.y * rightAttack));
        }
        else
        {
            timerRight -= Time.deltaTime;
            if (timerRight <= 0f)
                rightBloqued = false;
        }
    }

    private void FixedUpdate()
    {
        Vector3 posDiff = leftHead.position - leftPreviousPos;
        leftSpeed = Mathf.Abs(posDiff.magnitude);
        leftPreviousPos = leftHead.position;
        text.text = leftSpeed.ToString();

        posDiff = rightHead.position - rightPreviousPos;
        rightSpeed = Mathf.Abs(posDiff.magnitude);
        rightPreviousPos = rightHead.position;
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
            leftForce = Mathf.Max(minForce / 10f, rightSpeed / 10f);
            rightForce = Mathf.Max(minForce, leftSpeed);
            leftTime = winnerCooldown;
            rightTime = looserCooldown;
        }
        else if (rightSpeed > leftSpeed)
        {
            leftForce = Mathf.Max(minForce, rightSpeed);
            rightForce = Mathf.Max(minForce / 10f, leftSpeed / 10f);
            leftTime = looserCooldown;
            rightTime = winnerCooldown;
        }
        else
        {
            leftForce = Mathf.Max(minForce / 5f, rightSpeed / 5f);
            rightForce = Mathf.Max(minForce / 5f, leftSpeed / 5f);
            leftTime = rightTime = tieCooldown;
        }
        DeactiveFinger(true, leftTime);
        DeactiveFinger(false, rightTime);
        rightRb.AddForce(rightForce * rightDir.normalized * 100f, ForceMode.Impulse);
        leftRb.AddForce(leftForce * leftDir.normalized * 100f, ForceMode.Impulse);
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
