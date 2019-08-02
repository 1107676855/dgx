using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShoot : MonoBehaviour
{

    Transform ShootPos;
    public GameObject bullet;

    public  Transform TankShell;
    public float fireRate =5f;
    // 玩家上次射击后的间隔时间
    private float nextFire;
    // Start is called before the first frame update
    void Start()
    {
      
        ShootPos = GameObject.FindGameObjectWithTag("TankShoot").transform;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void ShootControl(bool f)
    {
        // 检测是否按下射击键以及射击间隔时间是否足够
        if (f && Time.time > nextFire)
        {
            
            nextFire = Time.time + fireRate;

            // Instantiate(bullet, this.transform.position + this.transform.forward * 1.2f + this.transform.up * 1.8f, this.transform.rotation);
            CreateBullet();


        }
    }


    private void CreateBullet()
    {
        Instantiate(bullet, ShootPos.position, TankShell.rotation);

    }
}
