using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightEnemy : MonoBehaviour
{
    private bool isPlayerInCollider = false;//�v���C���[���R���C�_�[�̒��ɂ����true
    // Start is called before the first frame update
    public Collider2D sight;//���E��\���R���C�_�[

    void OnTriggerEnter2D(Collider2D collision)
    {
        //�R���C�_�[�Ƀv���C���[�������������L�^
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInCollider = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        //�R���C�_�[����v���C���[���o�������L�^
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInCollider = false;
        }
    }
    public bool IsPlayerinSight()
    {
        return isPlayerInCollider;
    }
}
