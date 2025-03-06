using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProceduralChain : MonoBehaviour
{
    public Transform firstChain;//鎖の先頭の位置
    public Transform[] chain = new Transform[200];//各鎖の座標を格納
    public float[] bodyWidth;//各鎖が表す胴体の横幅
    public float headWidth = 1.2f;
    public float firstWidth = 1;
    public float endWidth = 0.2f;
    public int chainNum = 10;//鎖の点の数
    public float chainLimit = 10;//各鎖の間の長さ
    // Start is called before the first frame update
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        // 頂点の数を設定
        lineRenderer.positionCount = chainNum;

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
        for (int i = 2; i < chainNum; i++)
        {
            bodyWidth[i] = firstWidth - dif*(i-2);
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
            Debug.Log(i);
            Debug.Log(chain[i].position);
            Debug.DrawLine(chain[i-1].position, chain[i].position, Color.red);
        }
        
        for (int i = 0; i < chainNum; i++)
        {
            lineRenderer.SetPosition(i, chain[i].position);
        }
        
        Debug.DrawLine(new Vector2(0, 0), new Vector2(100, 100), Color.red);
    }

    Vector2 RightPos(Vector2 frontChain/*n-1番目の鎖*/, Vector2 thisChain/*n番目の鎖*/, int n)//鎖の位置と、前の鎖の位置から、鎖の右側の座標を返す
    {
        Vector2 VectToFront = (frontChain - thisChain).normalized;
        Vector2 rightVector = new Vector2(VectToFront.y, VectToFront.x) * bodyWidth[n];

        return rightPos;
    }
}
