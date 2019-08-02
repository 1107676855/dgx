using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//#if UNITY_EDITOR
//using UnityEditor;

public class ObjectManager : MonoBehaviour
{

    public GameObject userObject;
    public GameObject playerDie;
    public Transform target;
    public Transform []enemy_pos;
    public Transform [] player_pos;
    public GameObject player;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject Tank;
    private  List<string> nameList;
    private bool Hasplayer = false;
    public GameObject d;
    public Dictionary<string, string> dic_name;
    private List<GameObject> OtherPlayer;
    public Camera maincamera;
    private Dictionary<string,int> isCanInTank;
    private Dictionary<int,GameObject> EnemyManager;

    public Text GameCount;

    private void Start()
    {
        isCanInTank = new Dictionary<string, int>();
        OtherPlayer = new List<GameObject>();
        EnemyManager = new Dictionary<int, GameObject>();
        dic_name = new Dictionary<string, string>();
        DataHolder.Instance.OnCreatePlayer += Instance_OnCreatePlayer;
        nameList = new List<string>();
        DataHolder.Instance.OnCreateEnemy += Instance_OnCreateEnemy;
        DataHolder.Instance.OnEnemyDie += Instance_OnEnemyDie;
        DataHolder.Instance.OnUpdataPlayer += Instance_OnUpdataPlayer;
        DataHolder.Instance.OnEnemyBoom += Instance_OnEnemyBoom;
        DataHolder.Instance.OnPlayerUpdata += Instance_OnPlayerUpdata;
        DataHolder.Instance.OnWayPoint += Instance_OnWayPoint;
        DataHolder.Instance.OnMoneyUpdata += Instance_OnMoneyUpdata;
        DataHolder.Instance.OnGameCount += Instance_OnGameCount;
    }
    private void Awake()
    {
        NetWorkManage.Instance.SendMessage("CreatePlayer:{\"create\":0}");
       
    }
    private void Update()
    {
        if(OtherPlayer.Count>0)
            this.GetComponent<SwichControler>().isCanInTank = TankGetIn();

        DataHolder.Instance.GetMessage();
    }

    private void Instance_OnWayPoint(NavMeshInfo navMeshInfo)
    {
        if (GameObject.FindGameObjectWithTag(navMeshInfo.playertag) != null)
        {
            GameObject temp = GameObject.FindGameObjectWithTag(navMeshInfo.playertag);
            if (EnemyManager[navMeshInfo.enemy].GetComponent<enemy>().navtarget != temp.transform)
            {
                EnemyManager[navMeshInfo.enemy].GetComponent<enemy>().navtarget = temp.transform;
                EnemyManager[navMeshInfo.enemy].transform.position = new Vector3(navMeshInfo.px, navMeshInfo.py, navMeshInfo.pz);
            }
        }
            
        
        
    }
    private void Instance_OnGameCount(string str)
    {
        if (str == "first")
        {
            GameCount.text = "第二波";
        }
        if (str == "second")
        {
            GameCount.text = "第二波";
        }
    }

    private void Instance_OnMoneyUpdata(MoneyUpdata money)
    {
        d.GetComponent<PlayerHp>().money = money.money;
    }

    private void Instance_OnPlayerUpdata(PlayerUpdata playerupdata)
    {
        if (playerupdata.HpUpdata > 0)
        {
            d.GetComponent<PlayerHp>().Hp = playerupdata.HpUpdata;
        }
        else
        {
            d.GetComponent<PlayerHp>().Hp = 0;
            NetWorkManage.Instance.SendMessage("playerDie:{\"playerDie\":0}");
            d.GetComponent<PlayerAnimation>().isDie = true;
            d.GetComponent<DataToServer>().enabled = false;
            d.GetComponent<Player>().enabled = false;
            Invoke("PlayerDie", 1.0f);
        }

    }
    private void PlayerDie()
    {
        
        Destroy(d.gameObject);
        d= Instantiate(playerDie, new Vector3(0,0,0),new Quaternion(0,0,0,0));
    }
    

    private void Instance_OnEnemyDie(EnemyDie enemydie)
    {
        if (EnemyManager.ContainsKey(enemydie.enemy))
        {
            if (EnemyManager[enemydie.enemy] != null)
            {
                
                Destroy(EnemyManager[enemydie.enemy].gameObject);
            }
        }
    }

    private void Instance_OnEnemyBoom(EnemyDie enemyboom)
    {
        if (EnemyManager.ContainsKey(enemyboom.enemy))
        {
            if (EnemyManager[enemyboom.enemy] != null)
            {

                EnemyManager[enemyboom.enemy].GetComponent<BoomSoilder>().BoomToDie(); ;
            }
        }
    }



    private bool TankGetIn()
    {
        int num = 0;
        foreach(var x in isCanInTank)
        {
            num += x.Value;
        }
        Debug.Log(num);
        if (num / isCanInTank.Count == 200)
        {
            return true;
        }
        else
            return false;
    }

    public void Instance_OnCreateEnemy(enemyInfo enemydata)
    {
        EnemyCreateManager(enemydata.num, enemydata.enemyClass, enemydata.pos);

    }

    public void Instance_OnCreatePlayer(RedayInfo redayinfo)
    {
        
        Vector3 pos = new Vector3(player_pos[redayinfo.Num].position.x, player_pos[redayinfo.Num ].position.y, player_pos[redayinfo.Num ].position.z);
        Debug.Log(pos);
        d=Instantiate(userObject, pos,player_pos[0].transform.rotation);
        d.tag = "player" + (redayinfo.Num+1).ToString();
        Debug.Log(d.transform.position);
        dic_name.Add(redayinfo.username, d.tag);
        maincamera.GetComponent<ThirdCamera>().player = d.transform;
        this.GetComponent<SwichControler>().player = d.transform;
        Hasplayer = true;
        NetWorkManage.Instance.SendMessage("CreateEnemy:{\"enemy\":1}");

    }


    private void Instance_OnUpdataPlayer(PlayerInfo playerinfo)
    {
        
        if (!dic_name.ContainsKey(playerinfo.name))
        {
            Vector3 pos = new Vector3(playerinfo.px, 1.0f, playerinfo.pz);
            Quaternion rot = new Quaternion(-playerinfo.rw, -playerinfo.rx, -playerinfo.ry, -playerinfo.rz);
            var other=Instantiate(player, pos, rot);
            other.tag = playerinfo.tag;
            other.transform.Rotate(new Vector3(0, 180, 0));
            other.transform.localEulerAngles = new Vector3(0, -other.transform.localEulerAngles.y, 0);
            dic_name.Add(playerinfo.name, other.tag);
            OtherPlayer.Add(other);
            //AddTag(playerinfo.name, player);
            isCanInTank.Add(playerinfo.tag, playerinfo.f);
            //player.GetComponentInChildren<Rigidbody>().useGravity = false;
        }
        else if(playerinfo.tag!=d.tag)
        {
            for(int i = 0; i < OtherPlayer.Count; i++)
            {
                
                if (playerinfo.tag==OtherPlayer[i].tag)
                {
                    if (playerinfo.hp <= 0)
                    {
                        OtherPlayer[i].GetComponent<PlayerAnimation>().isDie = true;
                        Destroy(OtherPlayer[i].gameObject);
                    }
                    Vector3 pos = new Vector3(playerinfo.px, playerinfo.py, playerinfo.pz);
                    Quaternion rot = new Quaternion(-playerinfo.rw, -playerinfo.rx, -playerinfo.ry, -playerinfo.rz);
                    //Debug.Log(rot);

                    OtherPlayer[i].transform.position =Vector3.Lerp(OtherPlayer[i].transform.position,pos,0.1f);
                    OtherPlayer[i].transform.rotation = rot;
                    OtherPlayer[i].transform.Rotate(new Vector3(0, 180, 0));
                    OtherPlayer[i].transform.localEulerAngles=new Vector3(0, -OtherPlayer[i].transform.localEulerAngles.y, 0);
                    OtherPlayer[i].GetComponent<PlayerAnimation>().SetAnimation(playerinfo.h, playerinfo.v, 0);
                    OtherPlayer[i].GetComponent<ReWeapon>().ReWeaponClass(playerinfo.reweapon);
                    if (playerinfo.f==100)
                    {
                        OtherPlayer[i].transform.localScale = Vector3.zero;
                    }
                    else
                    {
                        OtherPlayer[i].transform.localScale = Vector3.one;
                    }
                    isCanInTank[playerinfo.tag] = playerinfo.f;
                }
            }
   
        }


    }

    private GameObject Createplayer(Vector3 pos,Quaternion rot)
    {
        return Instantiate(player, pos, rot);
    }


    private void CreateTank(Vector3 pos,Quaternion rot)
    {
        Instantiate(Tank, pos, rot);
    }

    private void ControllerManager()
    {
        for(int i = 0; i < 3; i++)
        {
            Vector3 temp_pos = target.position + new Vector3(Random.Range(0f, 3f), 0, Random.Range(0f, 3f));

            GameObject player_temp = Createplayer(temp_pos, Quaternion.LookRotation(enemy_pos[1].position - target.position));
            player_temp.tag = "";
            
        } 

    }

    public  void EnemyCreateManager(int num,int EnemyClass,int pos)
    {

        GameObject enemy;
        int enemy_class = EnemyClass-1;
        int posNum = pos;
        if (enemy_class == 0)
        {
            Vector3 temp_pos = enemy_pos[posNum-1].position ;
            enemy = CreateEnemy1(temp_pos, Quaternion.LookRotation(target.position - temp_pos));
        }
        else
        {
            Vector3 temp_pos = enemy_pos[posNum-1].position ;
            enemy = CreateEnemy2(temp_pos, Quaternion.LookRotation(target.position - temp_pos));
        }
        enemy.GetComponent<enemy>().num = num;
        EnemyManager.Add(num, enemy);

    }

    private GameObject CreateEnemy1(Vector3 pos, Quaternion rot)
    {
        return Instantiate(enemy1, pos,rot);
    }
    private GameObject CreateEnemy2(Vector3 pos, Quaternion rot)
    {
        return Instantiate(enemy2, pos, rot);
    }

    

}
