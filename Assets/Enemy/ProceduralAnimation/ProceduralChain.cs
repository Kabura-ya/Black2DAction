using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProceduralChain : MonoBehaviour
{
    public Transform firstChain;//���̐擪�̈ʒu
    public Transform[] chain = new Transform[200];//�e���̍��W���i�[
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
}
