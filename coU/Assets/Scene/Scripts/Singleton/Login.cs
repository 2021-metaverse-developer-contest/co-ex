using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login
{
    private static Login _login = null;
    private bool isLogin = false;
    private string id = "";
    private string storeName = "";

    private Login() { }

    public static Login Instance {
        get {
            if (_login == null)
                _login = new Login();
            return _login;
        }
    }

    public bool GetIsLogin()
    {
        return _login.isLogin;
    }

    public void SetLoginInfo(string id, string storeName)
    {
        _login.id = id;
        _login.storeName = storeName;
        _login.isLogin = true;
    }

    public bool IsPermission(string storeName)
    {
        if (!_login.GetIsLogin() || _login.storeName != storeName)
            return (false);
        return (true);
    }

    public void Logout()
    {
        _login.id = "";
        _login.storeName = "";
        _login.isLogin = false;
    }
}
