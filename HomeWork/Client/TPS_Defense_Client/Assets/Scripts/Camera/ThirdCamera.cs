using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdCamera : MonoBehaviour
{
    public Transform player;
    public Transform TankPos;
    public string player_name;
    //坦克或者人
    public bool isInTank = false;

    //方向灵敏度  
    public float sensitivityX = 10F;
    public float sensitivityY = 0.1F;
    
    //上下最大视角(Y视角)  
    public float minimumY = -0.1F;
    public float maximumY = 0.1F;
    //摄像机相对于人的位置
    [SerializeField] float x = 2.4f;
    [SerializeField] float y = 0.6f; 
    [SerializeField] float z = 2.5f;
    float rotationY = 0F;

    
    private void Start()
    {
       
        x = 2.4f;
        y = 0.6f;
        z = 2.5f;
       

        Cursor.lockState = CursorLockMode.Locked;
        
    }
    
    void Update()
    {
        Cursor.visible = false;

        
        if (isInTank)
        {
            x = 2f;
            y = 1f;
            z = 5f;

        }
        else
        {
            x = 2.4f;
            y = 0.6f;
            z = 2.5f;

        }
        if (player.GetComponent<PlayerAnimation>().isDie)
        {
            this.transform.position = new Vector3(0, 60, 0);
            this.transform.LookAt(new Vector3(0, 0, 0));
        }
        else
        {
            if (!isInTank)
            {
                this.transform.position = player.position + player.up * x + player.right * y - player.forward * z;
                //根据鼠标移动的快慢(增量), 获得相机左右旋转的角度(处理X)  
                float rotationX = transform.localEulerAngles.y + GameManager.Instance.InputController.MouseInput.x * sensitivityX;

                //根据鼠标移动的快慢(增量), 获得相机上下旋转的角度(处理Y)  
                rotationY += GameManager.Instance.InputController.MouseInput.y * sensitivityY;
                //角度限制. rotationY小于min,返回min. 大于max,返回max. 否则返回value   
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);


                Vector3 target = player.up * x + player.right * y + player.position + new Vector3(0, rotationY / 100, 0);
                //总体设置一下相机角度  
                transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
                transform.LookAt(target);
            }
            else
            {
                Transform[] father = TankPos.GetComponentsInChildren<Transform>();
                this.transform.position = father[5].position + father[5].up * x + father[5].right * y - father[5].forward * z;
                //根据鼠标移动的快慢(增量), 获得相机左右旋转的角度(处理X)  
                float rotationX = transform.localEulerAngles.y + GameManager.Instance.InputController.MouseInput.x * sensitivityX;

                //根据鼠标移动的快慢(增量), 获得相机上下旋转的角度(处理Y)  
                rotationY += GameManager.Instance.InputController.MouseInput.y * sensitivityY;
                //角度限制. rotationY小于min,返回min. 大于max,返回max. 否则返回value   
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);


                Vector3 target = father[5].up * x + father[5].right * y + father[5].position + new Vector3(0, rotationY / 100, 0);
                //总体设置一下相机角度  
                transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
                transform.LookAt(target);
            }

        }


    }

}
