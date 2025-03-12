using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//参考元URL
public class ProceduralChain : MonoBehaviour
{
    public Rigidbody2D firstChainRb;
    public Transform firstChain;//鎖の先頭の位置
    public Transform[] chain = new Transform[200];//各鎖の座標を格納
    public Vector3[] headPos = new Vector3[10];//各鎖の右側の座標
    public Vector3[] rightPos = new Vector3[200];//各鎖の右側の座標
    public Vector3[] leftPos = new Vector3[200];//各鎖の左側の座標
    public Vector3 tailPos;
    public float[] bodyWidth = new float[200];//各鎖が表す胴体の横幅
    public Vector3 headVector;
    public float headLength = 2.0f;
    public float headWidth1 = 1.0f;
    public float headWidth2 = 1.5f;
    public float firstWidth = 1.2f;
    public float endWidth = 1.0f;
    public int chainNum = 10;//鎖の点の数
    public float chainLimit = 10;//各鎖の間の長さ
    public float chainAngleConstraint = 120;//各鎖はこの角度以上曲がらない（単位は度）
    // Start is called before the first frame update
    int lineRenNum = 0;
    public LineRenderer lineRenderer;

    void Start()
    {
        // 頂点の数を設定
        lineRenderer.positionCount = 2 * chainNum + 8;
        initializeBodyWidth();
        // 各点の座標を設定
        /*
        for (int i = 0; i <= chainNum; i++)
        {
            chain[i].position = this.transform.position;
        }
        */
    }

    void initializeBodyWidth()
    {
        bodyWidth[0] = headWidth2;
        float dif = (firstWidth - endWidth) / (chainNum - 2);
        for (int i = 1; i <= chainNum; i++)
        {
            bodyWidth[i] = (firstWidth * (chainNum - 1 - i) + endWidth * i) / (chainNum - 1);
            Debug.Log(bodyWidth[i]);
        }
        for (int i = 1; i <= chainNum; i++)
        {
            chain[i].localScale = new Vector3(1, 1, 1) * bodyWidth[i] * 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        chain[0] = firstChain;
        this.transform.position = chain[0].position;
        Debug.Log(chain[0]);
        SetHeadVector();
        for (int i = 1; i <= chainNum; i++)
        {
            float chainDist = Vector2.Distance(chain[i-1].position, chain[i].position);
            Debug.Log(chainDist);
            if (chainDist > chainLimit )
            {
                //i番目の鎖を、前の鎖からchainLimitの距離に移動させる
                chain[i].position = chain[i-1].position + (chain[i].position - chain[i-1].position) * (chainLimit / chainDist);
            }
            SetAngleLimit(i);
            SetSidePos(i);
            Debug.Log(i);
            Debug.Log(chain[i].position);
            Debug.DrawLine(chain[i-1].position, chain[i].position, Color.red);
        }
        SetTailPos();
        /*
        for (int i = 0; i < chainNum; i++)
        {
            lineRenderer.SetPosition(i, chain[i].position);
        }
        */

        lineRenNum = 0;
        LineHead();
        for (int i = 1; i <= chainNum; i++)//右の方の線を引く
        {
            lineRenderer.SetPosition(lineRenNum, rightPos[i]);
            lineRenNum++;
        }
        lineRenderer.SetPosition(lineRenNum, tailPos);
        lineRenNum++;
        for (int i = chainNum; i >= 1; i--)//左の方の線を引く
        {
            lineRenderer.SetPosition(lineRenNum, leftPos[i]);
            lineRenNum++;
        }
        Debug.Log("lineRenNum");
        Debug.Log(lineRenNum);

        //Debug.DrawLine(new Vector2(0, 0), new Vector2(100, 100), Color.red);
    }


    void SetAngleLimit(int n)//n番目の鎖の位置が、制限よりも角度が狭い位置なら制限角度の位置に動かす
    {
        if (n <= 0) return;
        //cosθを求める
        Vector3 firstVector;
        Vector3 secondVector;
        if (n == 1) {
            firstVector = headVector.normalized;
        }
        else
        {
            firstVector = (chain[n - 2].position - chain[n - 1].position).normalized;
        }
        secondVector = (chain[n].position - chain[n - 1].position).normalized;
        float cos = Vector3.Dot(firstVector, secondVector);//内積を用いてcosθを計算
        // しきい値の cos 値を求める
        float cosThreshold = Mathf.Cos(chainAngleConstraint * Mathf.Deg2Rad);

        if (cos < cosThreshold) return;//角度が十分大きければ何もしない

        Vector3 normal = Vector3.Cross(firstVector, secondVector).normalized;//回転軸とするため外積を求める

        // A を +C° 回転
        Vector3 A1 = Quaternion.AngleAxis(chainAngleConstraint, normal) * firstVector;
        // A を -C° 回転
        Vector3 A2 = Quaternion.AngleAxis(chainAngleConstraint, normal) * firstVector;

        // B との角度を計算
        float angle1 = Vector3.Angle(A1, secondVector);
        float angle2 = Vector3.Angle(A2, secondVector);

        // 角度が小さい方を選ぶ
        Vector3 newSecondVector = (angle1 < angle2) ? A1.normalized : A2.normalized;//角度制限を繁栄したベクトル
        chain[n].position = newSecondVector * chainLimit + chain[n-1].position;
    }
    void SetHeadVector()//頭の方向を計算する
    {
        if (firstChainRb.velocity == Vector2.zero) return;
        headVector = firstChainRb.velocity.normalized;
    }
    void LineHead()//頭の線を引く
    {
        lineRenderer.SetPosition(lineRenNum++, leftPos[1]);
        Vector3 headLeft = new Vector2(-1 * headVector.y, headVector.x);
        Vector3 headRight = new Vector2(headVector.y, -1 * headVector.x);
        Vector3 headLeft2 = headLeft * headWidth1 + headVector * headLength;
        Vector3 headRight2 = headRight * headWidth1 + headVector * headLength;
        lineRenderer.SetPosition(lineRenNum, leftPos[1]);
        lineRenNum++;
        lineRenderer.SetPosition(lineRenNum, chain[0].position + headLeft * headWidth2);
        lineRenNum++;
        lineRenderer.SetPosition(lineRenNum, chain[0].position + headLeft2);
        lineRenNum++;
        lineRenderer.SetPosition(lineRenNum, chain[0].position + headRight2);
        lineRenNum++;
        lineRenderer.SetPosition(lineRenNum, chain[0].position + headRight * headWidth2);
        lineRenNum++;
        lineRenderer.SetPosition(lineRenNum, rightPos[1]);
        lineRenNum++;
    }

    void SetSidePos(int n)//n番目の鎖の左右の座標を計算する
    {
        Vector3 VectToFront = (chain[n-1].position - chain[n].position).normalized;
        Vector3 rightVector = new Vector2(VectToFront.y, - 1 * VectToFront.x) * bodyWidth[n];
        Vector3 leftVector  = new Vector2(-1 * VectToFront.y, VectToFront.x) * bodyWidth[n];
        rightPos[n] = chain[n].position + rightVector;
        leftPos[n]  = chain[n].position + leftVector;
        Debug.Log("SetSidePos");
        Debug.Log("chain[n].position");
        Debug.Log(chain[n].position);
        Debug.Log("rightPos[n]");
        Debug.Log(rightPos[n]);
        Debug.Log("leftPos[n]");
        Debug.Log(leftPos[n]);
        //Gizmos.DrawSphere(rightPos[n], 1);
        //Gizmos.DrawSphere(leftPos[n], 1);
        return;
    }

    void SetTailPos()
    {
        Vector3 VectToFront = (chain[chainNum - 1].position - chain[chainNum].position).normalized;
        tailPos = chain[chainNum].position - VectToFront * bodyWidth[chainNum];
        return;
    }
}
public class DrawGizmosPoint : MonoBehaviour
{
    public Vector3 point = new Vector3(0, 0, 0);
    public float size = 0.1f;
    public Color color = Color.red;

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(point, size);
    }
}
