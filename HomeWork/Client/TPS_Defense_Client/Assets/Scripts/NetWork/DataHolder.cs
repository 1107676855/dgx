using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;


public class DataHolder
{
    public int playerHp;
    public int Gun1;
    public int Gun2;
    public int Genade;
    public int money;
    public float h;
    public float v;
    public int reweapon;
    public int ammoVal;
    public string preMessage;
    public event System.Action<PlayerInfo> OnUpdataPlayer;
    public event System.Action<RedayInfo> OnCreatePlayer;
    public event System.Action<TankInfo> OnUpdateTank;
    public event System.Action<enemyInfo> OnCreateEnemy;
    public event System.Action<PlayerUpdata> OnPlayerUpdata;
    public event System.Action<MoneyUpdata> OnMoneyUpdata;
    public event System.Action<EnemyDie> OnEnemyDie;
    public event System.Action<EnemyDie> OnEnemyBoom;
    public event System.Action<NavMeshInfo> OnWayPoint;
    public event System.Action<string> OnLoginResult;
    public event System.Action<string> OnStartResult;
    public event System.Action<string> OnRegisterResult;
    public event System.Action<string> OnGameCount;
    private Queue<string> holder;
 

    private DataHolder()
    {
        holder = new Queue<string>();
    }
    private static DataHolder m_dataholder;
    public static DataHolder Instance
    {
        get
        {
            if (m_dataholder == null)
                m_dataholder = new DataHolder();
            return m_dataholder;
        }
    }
    public void AddMessage(string message)
    {
        preMessage = message + preMessage;
        while (preMessage.IndexOf(">") != -1)
        {
            if (preMessage.IndexOf(">") == preMessage.Length - 1)
            {
                holder.Enqueue(preMessage);
                preMessage = "";
            }
            else
            {
                int i = preMessage.IndexOf(">");
                holder.Enqueue(preMessage.Substring(0, i + 1));
                preMessage = preMessage.Substring(i + 1, preMessage.Length - i - 1);
            }
        }
    }
    public void GetMessage()/////放在update里更新
    {
        if (holder.Count == 0)
            return;
        string message = holder.Dequeue();
        HandleMessage(message.Substring(1, message.Length - 2));
    }
    public void HandleMessage(string message)
    {
        Debug.Log("HandleMessage:" + message);
        int target = message.IndexOf(":");
        string tag = message.Substring(0, target);
        string content = message.Substring(target + 1);
   
        switch (tag)
        {
            case "register":
                if (OnRegisterResult != null)
                    OnRegisterResult(content);
                break;
            case "login":
  
                if (OnLoginResult != null)
                    OnLoginResult(content);
 
                break;
            case "createPlayer":
              
                RedayInfo redayinfo = GetData2<RedayInfo>(content);

                if (OnCreatePlayer != null)
                {
                    Debug.Log(content);
                    OnCreatePlayer(redayinfo);
                }

                break;
            case "playerstatus":
                PlayerInfo player = GetData2<PlayerInfo>(content);
                if (OnUpdataPlayer != null)
                    OnUpdataPlayer(player);
               
                break;
            case "reday":
                if (content=="go!"&&OnStartResult != null)
                    OnStartResult(content);
                break;
            case "Tankstatus":
                Debug.Log(content);
                TankInfo tankinfo = GetData2<TankInfo>(content);
                if (OnUpdateTank != null)
                    OnUpdateTank(tankinfo);
                break;
            case "createEnemy":
                Debug.Log("收到创建");
                enemyInfo enemyInfo = GetData2<enemyInfo>(content);
                if (OnCreateEnemy != null)
                    OnCreateEnemy(enemyInfo);
                break;
            case "GamePass":
                if (content == "first")
                {
                    if (OnGameCount != null)
                    {
                        OnGameCount(content);
                    }
                    NetWorkManage.Instance.SendMessage("CreateEnemy:{\"enemy\":2}");
                }
                else if (content == "second")
                {
                    if (OnGameCount != null)
                    {
                        OnGameCount(content);
                    }
                    NetWorkManage.Instance.SendMessage("CreateEnemy:{\"enemy\":3}");
                }
                else
                {
                    SceneManager.LoadScene("Scenes/win");
                }
                break;
            case "EnemyDie":
                EnemyDie enemydie = GetData2<EnemyDie>(content);
                if (OnEnemyDie != null)
                {
                    OnEnemyDie(enemydie);
                }
                break;
            case "EnemyBoom":
                EnemyDie enemyboom = GetData2<EnemyDie>(content);
                if (OnEnemyBoom != null)
                {
                    OnEnemyBoom(enemyboom);

                }
                break;
            case "PlayerUpdata":
                PlayerUpdata playerupdata = GetData2<PlayerUpdata>(content);
                if (OnPlayerUpdata != null)
                {
                    OnPlayerUpdata(playerupdata);
                }
                break;
            case "Enemytarget":
                NavMeshInfo navMeshInfo = GetData2<NavMeshInfo>(content);
                if (OnWayPoint != null)
                {
                    OnWayPoint(navMeshInfo);
                }
                break;
            case "MoneyUpdata":
                MoneyUpdata money = GetData2<MoneyUpdata>(content);
                if (OnMoneyUpdata != null)
                {
                    OnMoneyUpdata(money);
                }
                break;
            case "GameOver":
                SceneManager.LoadScene("Scenes/GameOver");
                break;
            default:
                break;
        }
    }
    public static T GetData2<T>(string text) where T : class
    {

        T t = JsonConvert.DeserializeObject<T>(text);
       
        return t;
    }
}
