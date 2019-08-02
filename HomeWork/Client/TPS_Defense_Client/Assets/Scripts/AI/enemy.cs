using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{

    public bool isDeath = false;
    public Animator anima;
    public Transform target;
    public int num;
    public Transform player;
    public Transform navtarget;
    public bool isGameOver = false;

    private UnityEngine.AI.NavMeshAgent navMesh;




    // Start is called before the first frame update
    void Start()
    {
    
        anima = GetComponent<Animator>();
        navMesh = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("target").transform;
        player = GameObject.FindGameObjectWithTag("Manager").GetComponent<ObjectManager>().d.transform;
        navtarget = target;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if ((player.transform.position - this.transform.position).magnitude < 30 && navtarget == target)
            {
                NetWorkManage.Instance.SendMessage("EnemyTarget:{\"enemy\":" + num + ",\"px\":" + this.transform.position.x + ",\"py\":" + this.transform.position.y + ",\"pz\":" + this.transform.position.z + ",\"playertag\":\"" + player.tag + "\"}");
                navtarget = player;
            }
            else if ((player.transform.position - this.transform.position).magnitude > 30 && navtarget == player)
            {
                NetWorkManage.Instance.SendMessage("EnemyTarget:{\"enemy\":" + num + ",\"px\":" + this.transform.position.x + ",\"py\":" + this.transform.position.y + ",\"pz\":" + this.transform.position.z + ",\"playertag\":\"" + target.tag + "\"}");
                navtarget = target;
            }
        }
        else if(navtarget == player)
        {
            NetWorkManage.Instance.SendMessage("EnemyTarget:{\"enemy\":" + num + ",\"px\":" + this.transform.position.x + ",\"py\":" + this.transform.position.y + ",\"pz\":" + this.transform.position.z + ",\"playertag\":\"" + target.tag + "\"}");
            navtarget = target;
        }
        if (navtarget != target)
        {
            navMesh.SetDestination(navtarget.position);
        }
        else 
        {
            navMesh.SetDestination(target.position);
        }

        if (Mathf.Sqrt(Mathf.Pow(this.transform.position.x - target.position.x, 2) + Mathf.Pow(this.transform.position.z - target.position.z, 2)) < 0.5)
        {
            if (!isGameOver)
            {
                NetWorkManage.Instance.SendMessage("GameOver:{\"GameOver\":" + 0 + "}");
                isGameOver = true;
            }
        }
        if (isDeath)
        {
            isDeath = false;
            anima.SetBool("die", true);
            Invoke("GoToDie", 3.0f);
            this.GetComponent<NpcHp>().enabled = false;
            GameObject.FindGameObjectWithTag("Manager").GetComponent<GameControl>().enemyRes -= 1;
            GameObject.FindGameObjectWithTag("Manager").GetComponent<GameControl>().score += 1;
          

        }

    }

    private void GoToDie()
    {
        Destroy(this.gameObject);
        NetWorkManage.Instance.SendMessage("EnemyDie:{\"enemy\":" + num + "}");
    }
}
