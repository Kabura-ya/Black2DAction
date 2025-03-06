using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProceduralChain : MonoBehaviour
{
    public Transform firstChain;//���̐擪�̈ʒu
    public Transform[] chain = new Transform[200];//�e���̍��W���i�[
    public float[] bodyWidth;//�e�����\�����̂̉���
    public float headWidth = 1.2f;
    public float firstWidth = 1;
    public float endWidth = 0.2f;
    public int chainNum = 10;//���̓_�̐�
    public float chainLimit = 10;//�e���̊Ԃ̒���
    // Start is called before the first frame update
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        // ���_�̐���ݒ�
        lineRenderer.positionCount = chainNum;

        // �e�_�̍��W��ݒ�
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
                //i�Ԗڂ̍����A�O�̍�����chainLimit�̋����Ɉړ�������
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

    Vector2 RightPos(Vector2 frontChain/*n-1�Ԗڂ̍�*/, Vector2 thisChain/*n�Ԗڂ̍�*/, int n)//���̈ʒu�ƁA�O�̍��̈ʒu����A���̉E���̍��W��Ԃ�
    {
        Vector2 VectToFront = (frontChain - thisChain).normalized;
        Vector2 rightVector = new Vector2(VectToFront.y, VectToFront.x) * bodyWidth[n];

        return rightPos;
    }
}
