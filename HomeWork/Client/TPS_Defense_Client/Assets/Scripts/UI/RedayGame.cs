using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class RedayGame : MonoBehaviour
{
    

    private void Start()
    {
        DataHolder.Instance.OnStartResult += Instance_OnStartResult;
        NetWorkManage.Instance.SendMessage("reday:{ \"reday\":2}");

    }
    private void Instance_OnStartResult(string message)
    {

        if (message != "failed")
        {

            print("进入游戏");

            SceneManager.LoadScene("Scenes/SampleScene");


        }
        else
        {
            print("联网失败");
           
        }

    }

    private void Update()
    {
        //SNetWorkManage.Instance.SendMessage("reday:{ \"reday\":2}");
        DataHolder.Instance.GetMessage();
    }

}
