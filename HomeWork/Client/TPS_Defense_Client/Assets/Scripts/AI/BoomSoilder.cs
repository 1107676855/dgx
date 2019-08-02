using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomSoilder : MonoBehaviour
{
    public GameObject Boom;

    public Transform player;

    // Start is called before the first frame update
    void Start()
    {

    }
    private void Awake()
    {
        player =GameObject.FindGameObjectWithTag("Manager").GetComponent<ObjectManager>().d.transform;
    }
    // Update is called once per frame
    void Update()
    {

        attact();
    }
    public void attact()
    {
        if (this.GetComponent<NpcHp>().HP > 0)
            if ((this.transform.position - player.position).magnitude < 5)
            {
                Instantiate(Boom, this.transform.position, new Quaternion(0, 0, 0, 0));
                Destroy(this.gameObject);
                NetWorkManage.Instance.SendMessage("EnemyBoom:{\"enemy\":" + this.GetComponent<enemy>().num + ",\"px\":"+this.transform.position.x+ ",\"py\":" + this.transform.position.y+ ",\"pz\":" + this.transform.position.z+"}");
                    
            }
    }

    public void BoomToDie()
    {
        Instantiate(Boom, this.transform.position, new Quaternion(0, 0, 0, 0));
        Destroy(this.gameObject);
    }
}
