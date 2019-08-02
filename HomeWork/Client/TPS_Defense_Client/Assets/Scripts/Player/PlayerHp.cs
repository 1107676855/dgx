using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : MonoBehaviour
{
    //血量
    public int Hp;
    //步枪子弹数
    public int cur_gun1;
    public int Gun1_bullet;
    //手枪子弹数
    public int cur_gun2;
    public int Gun2_bullet;
    //手雷数
    public int Genade;
    //金钱数,等于积分按照比例减去花费
    public int money;
    //花掉的钱
    public int cost_money=0;
    
    // Start is called before the first frame update
    void Start()
    {
        Hp = 100;
        Gun1_bullet = 30;
        cur_gun2=30;
        cur_gun1=30;
        Gun2_bullet = 30;
        Genade = 3;
        money = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.InputController.BuyBullet)
        {
            Buy_bullet();
        }
    }

    private void Buy_bullet()
    {
        if (money >= 30)
        {
            NetWorkManage.Instance.SendMessage("BuyBullet:{\"Gun1\":10}");
            Gun1_bullet += 10;
        }
    }
}
