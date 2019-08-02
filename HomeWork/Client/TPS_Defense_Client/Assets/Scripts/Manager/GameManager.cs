using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    
    private GameObject gameObject;
    private static GameManager m_Instance;
    public static GameManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new GameManager
                {
                    gameObject = new GameObject("_gameManager")
                    
                };
                m_Instance.gameObject.AddComponent<InputController>();


            }
           
            return m_Instance;
        }
    }
    private InputController m_inputController;
    public InputController InputController
    {
        get
        {
            if (m_inputController == null)
                m_inputController = gameObject.GetComponent<InputController>();
            return m_inputController;
        }
    }

  
    
    //private UserInfo m_userinfo;
    //public UserInfo UserInfo
    //{
    //    get
    //    {
    //        if (m_userinfo == null)
    //            m_userinfo = new UserInfo();
    //        return m_userinfo;
    //    }
    //}


}
