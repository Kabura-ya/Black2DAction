using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDrainable
{
    //敵につけておいて、プレイヤーがエナジードレイン可能かを示す関数
    // Start is called before the first frame updat
    public bool Drain();//この関数を実行してtrueの時はエナジー吸収できる
}
