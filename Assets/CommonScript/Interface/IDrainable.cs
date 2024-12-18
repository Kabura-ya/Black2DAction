using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDrainable
{
    //敵につけておいて、プレイヤーがエナジードレイン可能かを示す関数
    // Start is called before the first frame updat
    public bool Drain();//これがtrueの時はエナジー吸収できる
    public bool SuperDrain();//これがtrueのときはスーパーダッシュでドレインできる
                             //（通常のダッシュで回避できる攻撃しかしない敵ならばDrain()の結果をそのまま返せばよい）
}
