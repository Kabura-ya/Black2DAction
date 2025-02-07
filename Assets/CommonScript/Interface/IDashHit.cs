using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IDashHit//プレイヤーがダッシュでぶつかると何か起きるものにこのインターフェースをつける（ダッシュによるエナジー吸収はIDrainableインターフェース）
{
    //プレイヤーが通常ダッシュで敵の弾にぶつかると弾を反射するとか、チャージダッシュで一部の攻撃パターン中の敵にぶつかると敵をスタンさせられるとかのために使用する
    public void NormalDashHit();//プレイヤーは通常ダッシュでぶつかった相手にこの関数を実行する。
    public void SuperDashHit();//プレイヤーはスーパーダッシュでぶつかった相手にこの関数を実行する。
}
