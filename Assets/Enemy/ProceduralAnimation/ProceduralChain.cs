using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProceduralChain : MonoBehaviour
{
    public Transform firstChain;//鎖の先頭の位置
    public Transform[] chain = new Transform[200];//各鎖の座標を格納
    public Vector3[] rightPos = new Vector3[200];//各鎖の右側の座標
    public Vector3[] leftPos = new Vector3[200];//各鎖の左側の座標
    public float[] bodyWidth = new float[200];//各鎖が表す胴体の横幅
    public float headWidth = 1.0f;
    public float firstWidth = 1;
    public float endWidth = 1.0f;
    public int chainNum = 10;//鎖の点の数
    public float chainLimit = 10;//各鎖の間の長さ
    // Start is called before the first frame update
    public LineRenderer lineRenderer;

    void Start()
    {
        // 頂点の数を設定
        lineRenderer.positionCount = 2 * chainNum - 2;
        //initializeBodyWidth();
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
        float dif = (firstWidth - endWidth) / (chainNum - 2);
        for (int i = 0; i < chainNum; i++)
        {
            bodyWidth[i] = (firstWidth * (chainNum - 1 - i) + endWidth * i) / (chainNum - 1);
            Debug.Log(bodyWidth[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        chain[0] = firstChain;
        this.transform.position = chain[0].position;
        Debug.Log(chain[0]);
        for (int i = 1; i <= chainNum; i++)
        {
            float chainDist = Vector2.Distance(chain[i-1].position, chain[i].position);
            Debug.Log(chainDist);
            if (chainDist > chainLimit )
            {
                //i番目の鎖を、前の鎖からchainLimitの距離に移動させる
                chain[i].position = chain[i-1].position + (chain[i].position - chain[i-1].position) * (chainLimit / chainDist);
            }
            SetSidePos(i);
            Debug.Log(i);
            Debug.Log(chain[i].position);
            Debug.DrawLine(chain[i-1].position, chain[i].position, Color.red);
        }
        /*
        for (int i = 0; i < chainNum; i++)
        {
            lineRenderer.SetPosition(i, chain[i].position);
        }
        */
        
        int lineRenNum = 0;
        for (int i = 1; i <= chainNum - 1; i++)
        {
            lineRenderer.SetPosition(lineRenNum, rightPos[i]);
            lineRenNum++;
        }
        for (int i = chainNum - 1; i >= 1; i--)
        {
            lineRenderer.SetPosition(lineRenNum, leftPos[i]);
            lineRenNum++;
        }
        Debug.Log("lineRenNum");
        Debug.Log(lineRenNum);

        //Debug.DrawLine(new Vector2(0, 0), new Vector2(100, 100), Color.red);
    }

    void SetSidePos(int n)//n番目の鎖の左右の座標を計算する
    {
        Vector3 VectToFront = (chain[n-1].position - chain[n].position).normalized;
        Vector3 rightVector = new Vector2(VectToFront.y, - 1 * VectToFront.x) * firstWidth;
        Vector3 leftVector  = new Vector2(-1 * VectToFront.y, VectToFront.x) * firstWidth;
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
