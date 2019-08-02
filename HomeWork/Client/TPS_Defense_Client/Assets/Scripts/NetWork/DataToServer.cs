using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class DataToServer:MonoBehaviour
{
    PlayerData playerData = new PlayerData();
    private string valuesJson;
    private string valuesJsonTank;
    private float updateTime;
    public float intervalTime;
    TankInfo tankinfo = new TankInfo();
    public GameObject Tank;
    public Camera camera;
    private bool isFirstSend=false;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {

        
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Tank = GameObject.FindGameObjectWithTag("Tank");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time < updateTime)
            return;
        if (!isFirstSend)
        {

                CalculateValues();
                SendTankInfo();

            if (valuesJson != "")
            {
                NetWorkManage.Instance.SendMessage("playerstatus:" + valuesJson);
                isFirstSend = true;
            }

        }
        CalculateValues();
        SendTankInfo();
        if (HasUpdata())
        {

  
            NetWorkManage.Instance.SendMessage("playerstatus:" + valuesJson);
            if (camera.GetComponent<ThirdCamera>().isInTank)
            {

                NetWorkManage.Instance.SendMessage("Tankstatus:" + valuesJsonTank);

            }

            
        }
        else
        {
            NetWorkManage.Instance.SendMessage("");
        }

        updateTime = Time.time + intervalTime;
    }

    private bool HasUpdata()
    {
        if(GameManager.Instance.InputController.Horizontal<-0.01|| GameManager.Instance.InputController.Horizontal > 0.01)
        {
            return true;
        }
        if(GameManager.Instance.InputController.Vertical < -0.01 || GameManager.Instance.InputController.Vertical > 0.01)
        {
            return true;
        }
        if (GameManager.Instance.InputController.MouseInput.x < -0.01 || GameManager.Instance.InputController.MouseInput.x > 0.01)
        {
            return true;
        }
        if(GameManager.Instance.InputController.Fire1|| GameManager.Instance.InputController.Fire1_one)
        {
            return true;
        }
        if (GameManager.Instance.InputController.BuyBullet)
        {
            return true;
        }
        if(GameManager.Instance.InputController.GetInTank)
        {
            return true;
        }
        if (GameManager.Instance.InputController.Reload)
        {
            return true;
        }
        if (GameManager.Instance.InputController.Space)
        {
            return true;
        }
        return false;
    }

    private void CalculateValues()
    {
        playerData.hp = GetComponent<PlayerHp>().Hp;
        playerData.Gun1 = GetComponent<PlayerHp>().cur_gun1+ GetComponent<PlayerHp>().Gun1_bullet;
        playerData.Gun2 = GetComponent<PlayerHp>().cur_gun2+ GetComponent<PlayerHp>().Gun2_bullet;
        playerData.Genade = GetComponent<PlayerHp>().Genade; ;
        playerData.money = GetComponent<PlayerHp>().money;
        playerData.px = transform.position.x;
        playerData.py = transform.position.y;
        playerData.pz = transform.position.z;
        playerData.rw = transform.rotation.w;
        playerData.rx = transform.rotation.x;
        playerData.ry = transform.rotation.y;
        playerData.rz = transform.rotation.z;
        playerData.h = GameManager.Instance.InputController.Horizontal;
        playerData.v = GameManager.Instance.InputController.Vertical;
        playerData.tag =this.tag;
        if (camera.GetComponent<ThirdCamera>().isInTank)
            playerData.f = 100;
        else
            playerData.f = 200;
        playerData.reweapon = GameManager.Instance.InputController.InputWeapon;
        valuesJson = JsonConvert.SerializeObject(playerData);
    }
    
    private void SendTankInfo()
    {
        if (camera.GetComponent<ThirdCamera>().isInTank)
            tankinfo.playername = this.tag;
        else
            tankinfo.playername = "NoBody";

        tankinfo.px = Tank.transform.position.x;
        tankinfo.py = Tank.transform.position.y;
        tankinfo.pz = Tank.transform.position.z;
        tankinfo.rw = Tank.transform.rotation.w;
        tankinfo.rx = Tank.transform.rotation.x;
        tankinfo.ry = Tank.transform.rotation.y;
        tankinfo.rz = Tank.transform.rotation.z;
        tankinfo.childRw = Tank.GetComponentInChildren<Transform>().GetComponentsInChildren<Transform>()[5].rotation.w;
        tankinfo.childRx = Tank.GetComponentInChildren<Transform>().GetComponentsInChildren<Transform>()[5].rotation.x;
        tankinfo.childRy = Tank.GetComponentInChildren<Transform>().GetComponentsInChildren<Transform>()[5].rotation.y;
        tankinfo.childRz = Tank.GetComponentInChildren<Transform>().GetComponentsInChildren<Transform>()[5].rotation.z;
        tankinfo.isShoot = GameManager.Instance.InputController.Fire1?1:0;
        valuesJsonTank = JsonConvert.SerializeObject(tankinfo);

    }
}
