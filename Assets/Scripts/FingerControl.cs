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
        leftHead.position = new Vector3(lHeadIniPos.x + (leftAxes.x * leftHeadLimits.x), lHeadIniPos.y - (leftHeadLimits.y * leftAttack), lHeadIniPos.z + (leftAxes.y * leftHeadLimits.z) + (leftHeadLimits.y * leftAttack));
        rightHead.position = new Vector3(rHeadIniPos.x - (rightAxes.x * rightHeadLimits.x), rHeadIniPos.y - (rightHeadLimits.y * rightAttack), rHeadIniPos.z - (rightAxes.y * rightHeadLimits.z) - (rightHeadLimits.y * rightAttack));
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
