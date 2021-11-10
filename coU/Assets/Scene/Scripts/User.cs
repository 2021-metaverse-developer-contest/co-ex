using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string id;
    public string pw;
    public string storeName;
    public int grade;
    //public int isValid;

    public User()
    {
    }

    public User(ref User other)
    {
        this.id = other.id;
        this.pw = other.pw;
        this.storeName = other.storeName;
        this.grade = other.grade;
    }

    public User(string id, string pw, string storeName, int grade)
    {
        this.id = id;
        this.pw = pw;
        this.storeName = storeName;
        this.grade = grade;
        //this.isValid = isValid;
    }
}
