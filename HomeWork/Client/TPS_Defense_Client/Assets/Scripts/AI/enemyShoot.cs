using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyShoot : MonoBehaviour
{
    public GameObject bullet;
    public Transform player;
    private float nextFire;
    private float fireRate = 5.0f;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Manager").GetComponent<ObjectManager>().d.transform;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((player.position - this.transform.position).magnitude < 15 && Time.time > nextFire)
        {
            anim.SetBool("shoot", true);
            nextFire = Time.time + fireRate;
            var b = Instantiate(bullet, this.transform.position + this.transform.forward * 1+new Vector3(0,1.25f,0), this.transform.rotation);
            b.GetComponent<Rigidbody>().AddForce((player.position +new Vector3(0,1.25f,0)- b.transform.position) * 100, ForceMode.Acceleration);
            b.GetComponent<Rigidbody>().useGravity = false;
            anim.SetBool("shoot", false);
        }
    }
}
