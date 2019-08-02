using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{


    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Manager").GetComponent<ObjectManager>().d;
        Invoke("destroyBullet", 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == player.tag)
        {
            NetWorkManage.Instance.SendMessage("EnemyBullet:{\"Hp\":50}");

        }
        destroyBullet();
    }


    private void destroyBullet()
    {
        Destroy(this.gameObject);
    }



}
