using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalData : MonoBehaviour
{

    #region 单例
    private static GlobalData _instance;
    public static GlobalData Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }

            return null;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        // Load dic；
    }
    #endregion

    public string path ; // 载入数据路径



    //账号密码数据
    private Dictionary<int, string> pwdDic;
    private Dictionary<int, UserData> userDic;

    // 注册函数
    // 注册失败返回 -1
    // 成功返回 1
    public int Register(string name, string pwd)
    {
        if (pwdDic.Count <= 8999)
        {
            int newID = Random.Range(1000, 9999);
            while (pwdDic.ContainsKey(newID) == true)
            {
                newID = newID + 1;
                if (newID > 9999)
                {
                    newID = 1000;
                }
            }

            UserData newUser = new UserData(newID, name);

            pwdDic.Add(newID, pwd);
            userDic.Add(newID, newUser);
            return newID;
        }
        else
        {
            return -1;
        }
    }

    // 登录判断
    public bool Login(int id, string pwd)
    {
        if (pwdDic.ContainsKey(id))
        {
            if(pwd == pwdDic[id])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    // 更改密码用户密码流程
    // 在忘记密码，但记得用户ID的情况下，验证昵称更改密码。
    public bool ChangePwd(int id, string name, string newPwd)
    {
        if (userDic.ContainsKey(id) == true)
        {
            // 验证名字
            if (userDic[id].name == name)
            {
                pwdDic[id] = newPwd;
                return true;   
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    void OnDestroy()
    {
        // 存储现有数据到JSON
    }
}


[SerializeField]
public class UserData
{
    public int id;
    public string name;

    public UserData(int id, string name)
    {
        this.id = id;
        this.name = name;
    }
}
