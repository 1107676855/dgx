using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcHp : MonoBehaviour
{
    //主摄像机对象
    private Camera camera;


    //NPC模型高度
    float npcHeight;
    //红色血条贴图
    public Texture2D blood_red;

    //默认NPC血值
    public int HP = 100;

    void Start()
    {

        //得到摄像机对象
        camera = Camera.main;


        float scal_y = transform.localScale.y;
        //它们的乘积就是高度
        npcHeight = (2.5f * scal_y);

    }

    void Update()
    {

        if (HP <= 0)
        {
            this.GetComponent<enemy>().isDeath = true;
            
        }
    }

    void OnGUI()
    {
        //得到NPC头顶在3D世界中的坐标
        //默认NPC坐标点在脚底下，所以这里加上npcHeight它模型的高度即可
        Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + npcHeight, transform.position.z);
        //根据NPC头顶的3D坐标换算成它在2D屏幕中的坐标
        Vector2 position = camera.WorldToScreenPoint(worldPosition);
        //得到真实NPC头顶的2D坐标
        position = new Vector2(position.x, Screen.height - position.y);
        //注解2
        //计算出血条的宽高
        Vector2 bloodSize = GUI.skin.label.CalcSize(new GUIContent(blood_red)) / 5;

        //通过血值计算红色血条显示区域
        int blood_width = blood_red.width * HP / 100;

        //在绘制红色血条
        GUI.DrawTexture(new Rect(position.x - (bloodSize.x / 2), position.y - bloodSize.y, blood_width / 5, bloodSize.y), blood_red);

    }




}
