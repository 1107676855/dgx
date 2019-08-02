using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwichControler : MonoBehaviour
{

    public bool WhatControl=false;
    Camera tpscam;
    public GameObject Tank;
    public Transform player;
    public bool isCanInTank=true;
    // Start is called before the first frame update
    void Start()
    {
        player = this.transform;
        tpscam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        if ((Tank.transform.position - player.position).magnitude < 5&&GameManager.Instance.InputController.GetInTank&&isCanInTank)
        {
            WhatControl = !WhatControl;
        }
        swapCamera(WhatControl);
    }

    private void swapCamera(bool w)
    { 
        //tank
        if (w)
        {
            tpscam.GetComponent<ThirdCamera>().isInTank = true;

        }
        else
        {
            tpscam.GetComponent<ThirdCamera>().isInTank = false;
        }
    }


}
