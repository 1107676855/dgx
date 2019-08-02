using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankControl : MonoBehaviour
{
    bool isRun=false;
    TankShoot tankshoot;
    // Start is called before the first frame update
    Camera tpscam;

    void Start()
    {
        DataHolder.Instance.OnUpdateTank+= Instance_OnUpdateTank;
        tpscam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        tankshoot = GetComponent<TankShoot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tpscam.GetComponent<ThirdCamera>().isInTank)
        {
            
            tankshoot.ShootControl(GameManager.Instance.InputController.Fire1_one);
            
            if (Input.GetKey(KeyCode.W))
            {
                this.transform.position += this.transform.forward * 0.1f;
                isRun = true;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                this.transform.position -= this.transform.forward * 0.1f;
                isRun = true;
            }
            else
            {
                isRun = false;
            }
            if (Input.GetKey(KeyCode.A) && isRun)
            {
                this.transform.RotateAround(this.transform.position, Vector3.up, -0.5f);
            }
            if (Input.GetKey(KeyCode.D) && isRun)
            {
                this.transform.RotateAround(this.transform.position, Vector3.up, 0.5f);
            }

            Transform[] father = this.GetComponentInChildren<Transform>().GetComponentsInChildren<Transform>();
            father[5].RotateAround(this.transform.position, Vector3.up * GameManager.Instance.InputController.MouseInput.x, 2f);
        }

    }
    private void Instance_OnUpdateTank(TankInfo tankinfo)
    {

        this.transform.position =Vector3.Lerp(this.transform.position,new Vector3(tankinfo.px, tankinfo.py, tankinfo.pz),0.1f);
        this.transform.rotation = new Quaternion(tankinfo.rw, tankinfo.rx, tankinfo.ry, tankinfo.rz);
        this.GetComponentInChildren<Transform>().GetComponentsInChildren<Transform>()[5].rotation = new Quaternion(tankinfo.childRw, tankinfo.childRx, tankinfo.childRy, tankinfo.childRz);
        this.transform.Rotate(new Vector3(0, 180, 0));
        this.transform.localEulerAngles = new Vector3(0, -this.transform.localEulerAngles.y, 0);
        if (tankinfo.isShoot==1)
        {
            tankshoot.ShootControl(true);
        }

    }


}
