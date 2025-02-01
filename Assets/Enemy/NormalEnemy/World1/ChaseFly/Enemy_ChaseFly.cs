using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ChaseFly : EnemyBase1
{
    public SightEnemy sightEnemy;//ここにプレイヤーが入ると追跡開始
    private void Start()
    {
        base.Start();
        StartCoroutine(Idle());
    }

    private void Update()
    {
        base.Update();
    }

    private IEnumerator Idle()//待機状態
    {
        while (true)
        {
            if (sightEnemy.IsPlayerinSight())
            {
                StartCoroutine(Chase());
                yield break;
            }
            yield return null;//1フレームだけ待つ
        }
    }
    private IEnumerator Chase()//追跡状態、待機中に視界のコライダーにプレイヤーが入るとこの状態に遷移
    {
        while (true)
        {
            FlipToPlayer();
            float dist = Vector3.Distance(transform.position, player.transform.position);
            Vector3 chaseVector = (player.transform.position - transform.position) / dist;
            rigidbody2d.AddForce(chaseVector * speed);
            yield return null;
        }
    }
}