using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserData
{
    public string Id;
    public string name;

    public UserData(string Id, string name)
    {
        this.Id = Id;
        this.name = name;
    }
}
