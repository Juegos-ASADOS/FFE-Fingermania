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

    private bool leftBloqued, rightBloqued, collisionFallen;
    private float timerLeft, timerRight;
    private float leftSpeed = 0, rightSpeed = 0;

    [SerializeField]
    float hitForce;

    private float colCooldown = 0;
    [SerializeField]
    float timeBetweenColisions;

    bool managingCollision;

    Vector3 leftPreviousPos = Vector3.zero, rightPreviousPos = Vector3.zero;

    [SerializeField]
    StaminaPlayer leftStamina, rightStamina;

    [SerializeField]
    float winDamage, loseDamage, tieDamage, timeDamage, timeToDamage;

    float timeLeftMoving = 0, timeRightMoving = 0;

    private void Start()
    {
        lHeadIniPos = leftHead.position;
        rHeadIniPos = rightHead.position;
        leftStamina.SetFingerControler(this);
        rightStamina.SetFingerControler(this);
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
            if (leftSpeed > 0) timeLeftMoving += Time.deltaTime;
            else timeLeftMoving = 0;

            if(timeLeftMoving > timeToDamage)
            {
                timeLeftMoving = 0;
                leftStamina.loseStamina(timeDamage);
            }
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
            if (rightSpeed > 0) timeRightMoving += Time.deltaTime;
            else timeRightMoving = 0;

            if (timeRightMoving > timeToDamage)
            {
                timeRightMoving = 0;
                rightStamina.loseStamina(timeDamage);
            }
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
    public void SwitchMovement(StaminaPlayer stamin, bool block)
    {
        if (stamin == leftStamina)
        {
            leftBloqued = block;
            timerLeft = block ? 1000f : 0f;
        }
        else
        {
            rightBloqued = block;
            timerRight = block ? 1000f : 0f;
        }
        collisionFallen = block;
        if (!collisionFallen)
        {
            leftStamina.SetDifficultyTembleke(1);
            rightStamina.SetDifficultyTembleke(1);
        }
    }

    public float GetFingerSpeed(bool isLeft)
    {
        return isLeft ? leftSpeed : rightSpeed;
    }


    public void ManageFingerColision(Vector3 leftPos, Vector3 rightPos)
    {
        if (collisionFallen)
        {
            leftStamina.SetDifficultyTembleke(2);
            rightStamina.SetDifficultyTembleke(2);
            return;
        }

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
        float leftDamage, rightDamage;

        managingCollision = true;
        if (leftSpeed > rightSpeed)
        {
            leftTime = winnerCooldown;
            rightTime = looserCooldown;
            rightForce = hitForce;
            leftDamage = winDamage;
            rightDamage = loseDamage;
        }
        else if (rightSpeed > leftSpeed)
        {
            leftTime = looserCooldown;
            rightTime = winnerCooldown;
            leftForce = hitForce;
            leftDamage = loseDamage;
            rightDamage = winDamage;
        }
        else
        {
            leftTime = rightTime = tieCooldown;
            leftForce = rightForce = hitForce;
            leftDamage = tieDamage;
            rightDamage = tieDamage;
        }
        if(!leftBloqued) DeactiveFinger(true, leftTime);
        if(!rightBloqued) DeactiveFinger(false, rightTime);
        rightRb.AddForce(rightForce * rightDir.normalized, ForceMode.Impulse);
        leftRb.AddForce(leftForce * leftDir.normalized, ForceMode.Impulse);
        leftStamina.loseStamina(leftDamage);
        rightStamina.loseStamina(rightDamage);
    }

    public void ManageFingerColisionExit()
    {
        if (collisionFallen)
        {
            leftStamina.SetDifficultyTembleke(1);
            rightStamina.SetDifficultyTembleke(1);
        }
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
