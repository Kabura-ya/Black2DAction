using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//�Q�l��URL
public class ProceduralChain : MonoBehaviour
{
    public Rigidbody2D firstChainRb;
    public Transform firstChain;//���̐擪�̈ʒu
    public Transform[] chain = new Transform[200];//�e���̍��W���i�[
    public Vector3[] headPos = new Vector3[10];//�e���̉E���̍��W
    public Vector3[] rightPos = new Vector3[200];//�e���̉E���̍��W
    public Vector3[] leftPos = new Vector3[200];//�e���̍����̍��W
    public Vector3 tailPos;
    public float[] bodyWidth = new float[200];//�e�����\�����̂̉���
    public Vector3 headVector;
    public float headLength = 2.0f;
    public float headWidth1 = 1.0f;
    public float headWidth2 = 1.5f;
    public float firstWidth = 1.2f;
    public float endWidth = 1.0f;
    public int chainNum = 10;//���̓_�̐�
    public float chainLimit = 10;//�e���̊Ԃ̒���
    public float chainAngleConstraint = 120;//�e���͂��̊p�x�ȏ�Ȃ���Ȃ��i�P�ʂ͓x�j
    // Start is called before the first frame update
    int lineRenNum = 0;
    public LineRenderer lineRenderer;

    void Start()
    {
        // ���_�̐���ݒ�
        lineRenderer.positionCount = 2 * chainNum + 8;
        initializeBodyWidth();
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
                //i�Ԗڂ̍����A�O�̍�����chainLimit�̋����Ɉړ�������
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
        for (int i = 1; i <= chainNum; i++)//�E�̕��̐�������
        {
            lineRenderer.SetPosition(lineRenNum, rightPos[i]);
            lineRenNum++;
        }
        lineRenderer.SetPosition(lineRenNum, tailPos);
        lineRenNum++;
        for (int i = chainNum; i >= 1; i--)//���̕��̐�������
        {
            lineRenderer.SetPosition(lineRenNum, leftPos[i]);
            lineRenNum++;
        }
        Debug.Log("lineRenNum");
        Debug.Log(lineRenNum);

        //Debug.DrawLine(new Vector2(0, 0), new Vector2(100, 100), Color.red);
    }


    void SetAngleLimit(int n)//n�Ԗڂ̍��̈ʒu���A���������p�x�������ʒu�Ȃ琧���p�x�̈ʒu�ɓ�����
    {
        if (n <= 0) return;
        //cos�Ƃ����߂�
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
        float cos = Vector3.Dot(firstVector, secondVector);//���ς�p����cos�Ƃ��v�Z
        // �������l�� cos �l�����߂�
        float cosThreshold = Mathf.Cos(chainAngleConstraint * Mathf.Deg2Rad);

        if (cos < cosThreshold) return;//�p�x���\���傫����Ή������Ȃ�

        Vector3 normal = Vector3.Cross(firstVector, secondVector).normalized;//��]���Ƃ��邽�ߊO�ς����߂�

        // A �� +C�� ��]
        Vector3 A1 = Quaternion.AngleAxis(chainAngleConstraint, normal) * firstVector;
        // A �� -C�� ��]
        Vector3 A2 = Quaternion.AngleAxis(chainAngleConstraint, normal) * firstVector;

        // B �Ƃ̊p�x���v�Z
        float angle1 = Vector3.Angle(A1, secondVector);
        float angle2 = Vector3.Angle(A2, secondVector);

        // �p�x������������I��
        Vector3 newSecondVector = (angle1 < angle2) ? A1.normalized : A2.normalized;//�p�x������ɉh�����x�N�g��
        chain[n].position = newSecondVector * chainLimit + chain[n-1].position;
    }
    void SetHeadVector()//���̕������v�Z����
    {
        if (firstChainRb.velocity == Vector2.zero) return;
        headVector = firstChainRb.velocity.normalized;
    }
    void LineHead()//���̐�������
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

    void SetSidePos(int n)//n�Ԗڂ̍��̍��E�̍��W���v�Z����
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
