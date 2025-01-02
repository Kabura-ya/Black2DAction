using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase1 : MonoBehaviour
{
    //�G�̊�{�I�ȋ@�\�A�p��������O��ŁA�p����ŐڐG����֌W�̐F�X�Ȋ֐����I�[�o�[���C�h���Ďg�p����z��

    public bool enableHit = true;//���ꂪtrue�̎������_���[�W���󂯂���^����
    public int attack = 1;
    public int hp = 3;
    public float speed = 10;
    protected GameObject player;//�v���C���[�̏����g����悤�ɂ��Ă���
    protected Transform playerTrans;
    Player playerScript;//�v���C���[�̃X�N���v�g�̊֐��𗘗p�ł���悤�ɂ���
    protected Rigidbody2D rigidbody2d;

    public GameObject damageEffect;
    public float invincibleTime = 0.2f;  // ���G���ԁi�_�Ŏ��ԁj
    public float blinkInterval = 0.1f;  // �_�ł̊Ԋu
    private SpriteRenderer spriteRenderer;
    private Animator anim;//�A�j���[�^�[
    // Start is called before the first frame update
    protected virtual void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();//���g��Rigidbody��ϐ��ɓ����
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<Player>();//�v���C���[�̃X�N���v�g�ɑ΂��đ���ł���悤�ɂ���
        playerTrans = player.transform;
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame

    protected virtual void Update()
    {

    }
    protected void FlipToPlayer()//Player�̕�������
    {
        if (playerTrans.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public virtual void BodyStay(Collider2D collision)//Body�����̎q�I�u�W�F�N�g��OnTriggerStay�ŌĂ΂��
    {
        Debug.Log("EnemyBase1_BodyStay");
        var damageTarget = collision.gameObject.GetComponent<IDamageable>();
        if (damageTarget != null)
        {
            damageTarget.Damage(attack);
        }
        if (collision.gameObject.tag == "Player" && enableHit)
        {
            Debug.Log("EnemyOntrrigerEnter_Player");
            //var damageTarget = collision.gameObject.GetComponent<IDamageable>();
            if (damageTarget != null)
            {
                damageTarget.Damage(attack);
            }
        }
    }

    //�_���[�W���󂯂镔���A�^���I�Ƀf�t�H���g�������Č����邽�߂�3�ϐ��̐��̈ႤDamage�֐���p�ӂ��Ă���
    public void Damage(int value) { Damage(value, Vector2.zero); }
    public void Damage(int value, Vector2 vector) { Damage(value, vector, 0); }
    public void Damage(int value, Vector2 vector, int type)
    {
        Debug.Log("EnemyBase_Damage");
        if (enableHit)
        {
            hp -= value;
            Instantiate(damageEffect, transform.position, transform.rotation);

            rigidbody2d.velocity = vector;

            if (hp <= 0)
            {
                Death();
            }
            else
            {
                StartCoroutine(BlinkCoroutine(invincibleTime));
            }
        }
    }

    IEnumerator BlinkCoroutine(float duration)//�_���[�W���󂯂����Ɉ�u�_�ł���
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // �X�v���C�g�̉�����؂�ւ���
            spriteRenderer.enabled = !spriteRenderer.enabled;

            // �_�ł̊Ԋu�����҂�
            yield return new WaitForSeconds(blinkInterval);

            // �o�ߎ��Ԃ��X�V
            elapsedTime += blinkInterval;
        }
        // �_�ŏI����ɃX�v���C�g��\������
        spriteRenderer.enabled = true;
    }
    public void Death()
    {
        Destroy(this.gameObject);
    }
    public bool Drain()//Body�̎q�I�u�W�F�N�g����Ă΂��
    {
        return enableHit;//�_���[�W����Ƃ���L���ɂ��Ă���Ԃ̂݃h���C���\
    }

    public bool SuperDrain()//Body�̎q�I�u�W�F�N�g����Ă΂��
    {
        return Drain();
    }
}
