using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg : MonoBehaviour
{
    // Start is called before the first frame update
    public LineRenderer rightLine, leftLine;
    public Vector3 rightRootPos = Vector3.zero, rightTipPos = Vector3.zero, leftRootPos = Vector3.zero, leftTipPos = Vector3.zero;//rootが足の付け根、tipが先端の方
    public Transform body, frontBody;//bodyの横に足を生やす。胴体の向きを計算するため、前の胴体の位置も使う
    public float rootLength = 0.5f;
    public float tipLength = 1.5f;
    public float tipLimit = 1.5f;//これだけ進んだら足先の位置を更新する
    public float chainLimit = 1.0f;
    public Transform[] rightChain = new Transform[3];//右足の各鎖の座標を格納、0番が足の付け根で2番が先
    public Transform[] leftChain = new Transform[3];//左足の各鎖の座標を格納、0番が足の付け根で2番が先
    public int legLength = 3; //足の鎖の個数（現状3のみの予定）
    public int pullTimes = 10;//足を引っ張る回数
    public float rightFirstForward = 1.5f;//足が交互に動くように、右足の先端の初期位置をこの分だけ前に動かす
    void Start()
    {
        InitLine();
        SetFirstTipPos();
    }

    void InitLine()//足の線を引くLineRendererを初期化
    {
        rightLine.positionCount = legLength;
        leftLine.positionCount = legLength;
    }
    // Update is called once per frame
    void Update()
    {
        SetRootPos();
        SetTipPos();
        PullBoth(pullTimes);
        LegLine();
    }
    void SetRootPos()//足の付け根の座標を計算する
    {
        Vector3 VectToFront = (frontBody.position - body.position).normalized;
        Vector3 rightVector = new Vector2(VectToFront.y, -1 * VectToFront.x);
        Vector3 leftVector = new Vector2(-1 * VectToFront.y, VectToFront.x);
        rightRootPos = body.position + rightVector * rootLength;
        leftRootPos = body.position + leftVector * rootLength;
        //Gizmos.DrawSphere(rightPos[n], 1);
        //Gizmos.DrawSphere(leftPos[n], 1);
        return;
    }

    void SetTipPos()//足の先端の座標を計算する、（本体が一定以上進んだら各TipPosを更新する）
    {
        Vector3 VectToFront = (frontBody.position - body.position).normalized;
        Vector3 rightVector = new Vector2(VectToFront.y, -1 * VectToFront.x);
        Vector3 leftVector = new Vector2(-1 * VectToFront.y, VectToFront.x);

        Vector3 tempRightTipPos = body.position + rightVector * tipLength;
        Vector3 tempLeftTipPos = body.position + leftVector * tipLength;

        //前の足先の位置から、胴体が一定以上進んだら足先の位置を更新
        if (Vector3.Distance(rightTipPos, tempRightTipPos) > tipLimit){
            rightTipPos = tempRightTipPos;

        }
        if (Vector3.Distance(leftTipPos, tempLeftTipPos) > tipLimit)
        {
            leftTipPos = tempLeftTipPos;

        }
        //Gizmos.DrawSphere(rightPos[n], 1);
        //Gizmos.DrawSphere(leftPos[n], 1);
        return;
    }

    void SetFirstTipPos()//足の先端の座標を計算する、（本体が一定以上進んだら各TipPosを更新する）
    {
        Vector3 VectToFront = (frontBody.position - body.position).normalized;
        Vector3 rightVector = new Vector2(VectToFront.y, -1 * VectToFront.x);
        Vector3 leftVector = new Vector2(-1 * VectToFront.y, VectToFront.x);

        rightTipPos = body.position + rightVector * tipLength + VectToFront * rightFirstForward;
        leftTipPos = body.position + leftVector * tipLength;

        //前の足先の位置から、胴体が一定以上進んだら足先の位置を更新

        //Gizmos.DrawSphere(rightPos[n], 1);
        //Gizmos.DrawSphere(leftPos[n], 1);
        return;
    }

    void PullToRoot()//付け根の座標に足を引っ張る
    {
        //右足を引っ張る
        rightChain[0].position = rightRootPos;//先頭の鎖の位置を決定する
        for (int i = 1; i < legLength; i++)
        {
            float chainDist = Vector2.Distance(rightChain[i - 1].position, rightChain[i].position);
            Debug.Log(chainDist);
            if (chainDist > chainLimit)
            {
                //i番目の鎖を、前の鎖からchainLimitの距離に移動させる
                rightChain[i].position = rightChain[i - 1].position + (rightChain[i].position - rightChain[i - 1].position).normalized * chainLimit;
            }
        }

        //左足を引っ張る
        leftChain[0].position = leftRootPos;
        for (int i = 1; i < legLength; i++)
        {
            float chainDist = Vector2.Distance(leftChain[i - 1].position, leftChain[i].position);
            Debug.Log(chainDist);
            if (chainDist > chainLimit)
            {
                //i番目の鎖を、前の鎖からchainLimitの距離に移動させる
                leftChain[i].position = leftChain[i - 1].position + (leftChain[i].position - leftChain[i - 1].position).normalized * chainLimit;
            }
        }
    }
    void PullToTip()//足の先端の座標に足を引っ張る
    {
        //右足を引っ張る
        rightChain[legLength - 1].position = rightTipPos;//先頭の鎖の位置を決定する
        for (int i = legLength-2; i >= 0; i--)
        {
            float chainDist = Vector2.Distance(rightChain[i + 1].position, rightChain[i].position);
            Debug.Log(chainDist);
            if (chainDist > chainLimit)
            {
                //i番目の鎖を、前の鎖からchainLimitの距離に移動させる
                rightChain[i].position = rightChain[i + 1].position + (rightChain[i].position - rightChain[i + 1].position).normalized * chainLimit;
            }
        }

        //左足を引っ張る
        leftChain[legLength - 1].position = leftTipPos;//先頭の鎖の位置を決定する
        for (int i = legLength - 2; i >= 0; i--)
        {
            float chainDist = Vector2.Distance(leftChain[i + 1].position, leftChain[i].position);
            Debug.Log(chainDist);
            if (chainDist > chainLimit)
            {
                //i番目の鎖を、前の鎖からchainLimitの距離に移動させる
                leftChain[i].position = leftChain[i + 1].position + (leftChain[i].position - leftChain[i + 1].position).normalized * chainLimit;
            }
        }
    }

    void PullBoth(int n)
    {
        for (int i = 1; i <=n; i++)
        {
            PullToTip();
            PullToRoot();
        }
    }

    void LegLine()//足の線を引く
    {
        for (int i = 0; i < legLength; i++)//右の方の線を引く
        {
            rightLine.SetPosition(i, rightChain[i].position);
        }
        for (int i = 0; i < legLength; i++)//左の方の線を引く
        {
            leftLine.SetPosition(i, leftChain[i].position);
        }
    }
}
