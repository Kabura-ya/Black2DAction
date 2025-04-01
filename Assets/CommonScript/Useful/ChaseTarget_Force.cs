using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTarget_Force : MonoBehaviour
{
    //targetNameの名前のオブジェクトを
    public string targetName = "Player"; 
    protected GameObject target;//追跡対象
    protected Transform targetTrans;//追跡対象の位置
    protected Rigidbody2D thisRb2d;//自身のrigidbody2D
    public float chaseForce = 5;//追跡対象の方向に自身が加える力
    public float speedLimit = 10;//速度上限
    private Transform thisTrans;
    // Start is called before the first frame update
    void Start()
    {
        thisRb2d = GetComponent<Rigidbody2D>();//自身のRigidbodyを変数に入れる
        thisTrans = thisRb2d.transform;
        target = GameObject.Find(targetName);
        targetTrans = target.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //追跡対象の方向に力を加える
        Vector2 chaseVector = (targetTrans.position - thisTrans.position).normalized;
        thisRb2d.AddForce(chaseVector * chaseForce, ForceMode2D.Force);

        //speedLimit以上の速度が出ないようにする
        if (thisRb2d.velocity.magnitude > speedLimit)
        {
            thisRb2d.velocity = thisRb2d.velocity.normalized * speedLimit;
        }
    }
}
