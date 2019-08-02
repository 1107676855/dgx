using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public GameObject dontdestroy;
    public GameObject loginPlane;
    public GameObject registerPlane;
    public GameObject freePlane;
    public InputField userName;
    public InputField userPass;
    public InputField registerName;
    public InputField registerPass;
    public InputField registerConfirmPass;
    //private networkSocket manager;
    private void Start()
    {
    }
    private void Awake()
    {
        loginPlane.SetActive(true);
        registerPlane.SetActive(false);
        freePlane.SetActive(false);
        NetWorkManage.Instance.Connect("localhost", 50000);
        DataHolder.Instance.OnRegisterResult += Instance_OnRegisterResult;
        DataHolder.Instance.OnLoginResult += Instance_OnLoginResult;
    }

    private void Instance_OnRegisterResult(string message)
    {
        if (message != "failed")
        {
            print("注册成功");
            reToLogin();
            freePlane.SetActive(false);
        }
        else
        {
            print("注册失败");
            freePlane.SetActive(false);

        }
    }
    private void Instance_OnLoginResult(string message)
    {
         
        if (message != "failed")
        {

            print("登录成功");
            SceneManager.LoadScene("Scenes/Reday");
  
         
        }
        else
        {
            print("登录失败");
            freePlane.SetActive(false);
        }

    }

    public void OnClickLogin()
    {
        freePlane.SetActive(true);
        if ((userName.text == "") || (userPass.text == ""))
        {
            print("请输入正确用户名或密码");
        }
        NetWorkManage.Instance.SendMessage("login:{ \"name\":\"" + userName.text + "\",\"password\":\"" + userPass.text + "\"}");
        print("登录中");
        //SceneManager.LoadScene("Scenes/SampleScene");
         freePlane.SetActive(false);
    }
    public void OnClickRegister()
    {
        freePlane.SetActive(true);
        loginPlane.SetActive(false);
        registerPlane.SetActive(true);
        freePlane.SetActive(false);
    }
    public void OnClickRegisterSubmit()
    {
        freePlane.SetActive(true);
        if (registerName.text == "")
        {
            print("请输入用户名");
            freePlane.SetActive(false);
            return;
        }
        if (registerPass.text != registerConfirmPass.text)
        {
            print("请输入确保两次输入的密码一致");
            freePlane.SetActive(false);
            return;
        }
        ////保存用户名和密码
        string info = "register:{ \"name\":\"" + registerName.text + "\",\"password\":\"" + registerPass.text + "\"}";
        Debug.Log(info);

        NetWorkManage.Instance.SendMessage(info);

    }
    public void reToLogin()
    {
        freePlane.SetActive(true);
        loginPlane.SetActive(true);
        registerPlane.SetActive(false);
        freePlane.SetActive(false);
    }
    private void Update()
    {
        DataHolder.Instance.GetMessage();
    }
    [ContextMenu("send message")]
    private void sendMessage()
    {
        NetWorkManage.Instance.SendMessage(userName.text);
    }
}
