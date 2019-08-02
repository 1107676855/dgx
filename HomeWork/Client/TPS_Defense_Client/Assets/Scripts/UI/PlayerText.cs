using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerText : MonoBehaviour
{
    //血量
    public int Hp;
    //步枪子弹数
    public int cur_Gun1;
    public int Gun1_bullet;
    //手枪子弹数
    public int cur_Gun2;
    public int Gun2_bullet;
    //金钱数,等于积分按照比例减去花费
    public int money;
    public int genadenum;

    public Text HpText;
    public Text Gun1Text;
    public Text Gun2Text;
    public Text genade;
    public Text moneyText;

    public Camera tpsCam;

    // Start is called before the first frame update
    void Start()
    {
        Hp = 100;
        Gun1_bullet = 30;
        Gun2_bullet = 30;
        cur_Gun1 = 22;
        cur_Gun2 = 12;
        genadenum = 3;
        money = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (tpsCam.GetComponent<ThirdCamera>().player != null)
        {
            
            HpText.text = "Hp:" + tpsCam.GetComponent<ThirdCamera>().player.GetComponent<PlayerHp>().Hp.ToString();
            Gun1Text.text = "步枪：" + tpsCam.GetComponent<ThirdCamera>().player.GetComponent<PlayerHp>().cur_gun1.ToString() + "/" + tpsCam.GetComponent<ThirdCamera>().player.GetComponent<PlayerHp>().Gun1_bullet.ToString();
            Gun2Text.text = "手枪:" + tpsCam.GetComponent<ThirdCamera>().player.GetComponent<PlayerHp>().cur_gun2.ToString() + "/" + tpsCam.GetComponent<ThirdCamera>().player.GetComponent<PlayerHp>().Gun2_bullet.ToString();
            moneyText.text = "money:" + tpsCam.GetComponent<ThirdCamera>().player.GetComponent<PlayerHp>().money.ToString();
            genade.text = "手雷：" + tpsCam.GetComponent<ThirdCamera>().player.GetComponent<PlayerHp>().Genade.ToString();
        }
        else
        {
            HpText.text = "Hp:" + Hp.ToString();
            Gun1Text.text = "步枪：" + cur_Gun1.ToString() + "/" + Gun1_bullet.ToString();
            Gun2Text.text = "手枪:" + cur_Gun2.ToString() + "/" + Gun2_bullet.ToString();
            moneyText.text = "money:" + money.ToString();
            genade.text = "手雷：" + genadenum.ToString();
        }

    }

}
