using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    //怪物波次
    public int count = 3;
    //每波生成怪的个数
    public int enemyNum;
    //场上剩余的敌人数量
    public int enemyRes=0;
    //死亡敌人数
    public int score = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyRes == 0 && count > 0)
        {
            //GetComponent<ObjectManager>().EnemyCreateManager(4 - count);
            enemyRes = 4 - count;
            count--;
        }
    }
}
