using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genade : MonoBehaviour
{

    public GameObject boom;

    //手雷速度
    private float bullet_force = 10.0f;
 
    //初始位置
    Vector3 bullet_pre_pos;
    // Start is called before the first frame update
    void Start()
    {
        bullet_pre_pos = this.transform.position;
        this.GetComponent<Rigidbody>().AddForce((this.transform.forward+this.transform.up) * bullet_force, ForceMode.VelocityChange);
        Invoke("Destroy_bullet", 10.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && collision.collider.tag != "Tank")
        {
            Collider[] collidedObj = Physics.OverlapSphere(this.transform.position, 10);
            foreach (var x in collidedObj)
            {
                if (x.tag == "enemy")
                {
                    x.GetComponent<NpcHp>().HP = 0;
                }
            }

            Destroy_bullet();

        }
    }

    private void Destroy_bullet()
    {
       Instantiate(boom, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
     

    }

}
