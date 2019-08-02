using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    //手雷
    public bool isGenade = false;
    public GameObject genade;
    //子弹
    public GameObject bullet;
    // 设置枪击带来的伤害值
    public int gunDamage = 1;
    
    // 设置两次枪击的间隔时间
    public float fireRate = 0.25f;

    // 设置玩家可以射击的Unity单位
    public float weaponRange = 50f;

    // 设置枪击为物体带来的冲击力
    public float hitForce = 100f;

    // GunEnd游戏对象
    public Transform gunEnd;

    // 相机
    private Camera tpsCam;


    // 玩家上次射击后的间隔时间
    private float nextFire;


    void Start()
    {
      

        // 获取Camera组件
        tpsCam =GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

    }

 

    public void ShootControl(bool f)
    {
        // 检测是否按下射击键以及射击间隔时间是否足够
        if (f && Time.time > nextFire)
        {
            if (ReWeapon.weaponNO == 3&& GetComponent<PlayerHp>().Genade>0)
            {
                nextFire = Time.time + fireRate + 2.0f;
                Invoke("CreateGenade", 0.8f);
                GetComponent<PlayerHp>().Genade -= 1;
            }
            else
            {
                if (ReWeapon.weaponNO == 1)
                {
                    if(GetComponent<PlayerHp>().cur_gun1>0)
                    {
                        nextFire = Time.time + fireRate;
                        GetComponent<PlayerHp>().cur_gun1 -= 1;
                        Instantiate(bullet, this.transform.position + this.transform.forward * 1.2f + this.transform.up * 1.8f, this.transform.rotation);
                    }
                }
                else if(ReWeapon.weaponNO == 2)
                {
                    if (GetComponent<PlayerHp>().cur_gun2 > 0)
                    {
                        nextFire = Time.time + fireRate;
                        GetComponent<PlayerHp>().cur_gun2 -= 1;
                        Instantiate(bullet, this.transform.position + this.transform.forward * 1.2f + this.transform.up * 1.8f, this.transform.rotation);
                    }
                }
                // 射击之后更新间隔时间
              

            }
        }
    }

    private void Update()
    {
        if (GameManager.Instance.InputController.Reload) {
            if (ReWeapon.weaponNO == 1)
            {
                if (GetComponent<PlayerHp>().Gun1_bullet > 30- GetComponent<PlayerHp>().cur_gun1)
                {
                    GetComponent<PlayerHp>().cur_gun1 = 30;
                    GetComponent<PlayerHp>().Gun1_bullet = GetComponent<PlayerHp>().Gun1_bullet - (30 - GetComponent<PlayerHp>().cur_gun1);
                }
                else
                {
                    GetComponent<PlayerHp>().cur_gun1 = GetComponent<PlayerHp>().cur_gun1 + GetComponent<PlayerHp>().Gun1_bullet;
                    GetComponent<PlayerHp>().Gun1_bullet = 0;
                }
            }
            else if (ReWeapon.weaponNO == 2)
            {
                if (GetComponent<PlayerHp>().Gun2_bullet > 30 - GetComponent<PlayerHp>().cur_gun2)
                {
                    GetComponent<PlayerHp>().cur_gun2 = 30;
                    GetComponent<PlayerHp>().Gun2_bullet = GetComponent<PlayerHp>().Gun2_bullet - (30 - GetComponent<PlayerHp>().cur_gun2);
                }
                else
                {
                    GetComponent<PlayerHp>().cur_gun2 = GetComponent<PlayerHp>().cur_gun2 + GetComponent<PlayerHp>().Gun2_bullet;
                    GetComponent<PlayerHp>().Gun2_bullet = 0;
                }
            }
        }
    }
    private void CreateGenade()
    {
        Instantiate(genade, this.transform.position + this.transform.forward * 1.2f + this.transform.up * 2.5f, this.transform.rotation);

    }

}
