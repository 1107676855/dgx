using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    //子弹速度

    private float bullet_force=15.0f;
    //相机
    Camera tpsCam;
    //初始位置
    Vector3 bullet_pre_pos;
    // Start is called before the first frame update
    void Start()
    {
        tpsCam= GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        bullet_pre_pos = this.transform.position;
        this.GetComponent<Rigidbody>().AddForce((computeBulletRot()-bullet_pre_pos).normalized * bullet_force,ForceMode.VelocityChange);
        Invoke("Destroy_bullet", 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private Vector3 computeBulletRot()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 50))
        {
            return hit.point;
        }
        else
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 50));
        }
    }

    private void Destroy_bullet()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "enemy")
        {
           if(collision.collider.GetComponent<NpcHp>().HP>33)
                collision.collider.GetComponent<NpcHp>().HP -= 33;
           else
                collision.collider.GetComponent<NpcHp>().HP = 0;
        }
        Destroy(this.gameObject);
    }

}
