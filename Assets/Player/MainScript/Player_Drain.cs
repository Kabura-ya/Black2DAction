using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Drain : MonoBehaviour
{
    // Start is called before the first frame update
    //�v���C���[�̃h���C���p�R���C�_�[�ɂ���X�N���v�g
    //�h���C���p�R���C�_�[�̓_�b�V�����ɂ̂ݗL���������
    //�G�ꂽ���̂����̂܂܃v���C���[�ɑ��邾��

    private void OnTriggerEnter2D(Collider2D collision)//Body�����ɐG�ꂽ���̂̏����A���̂܂ܐe�I�u�W�F�N�g��Player�X�N���v�g�ɑ��邾��
    {
        Debug.Log("Player_Drain_OnTrigger");
        transform.root.gameObject.GetComponent<Player>().DrainEnter(collision);
    }
}
