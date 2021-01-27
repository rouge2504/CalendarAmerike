using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;

public class LoginManager : MonoBehaviour
{
    public InputField user;
    public InputField password;
    public string fileName;
    private string path;
    [HideInInspector] public List<Data> dataList = new List<Data>();
    // Start is called before the first frame update
    void Start()
    {
        path = Path.Combine(Application.streamingAssetsPath, fileName);
        RecoveryData();
    }

    void RecoveryData()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Data[] data = JsonHelperItem.FromJson<Data>(json);

            User[] users = new User[1];
            users[0] = new User("Macaco", "r@a.com", "nel", DataManager.Profile.MEDIC);

            SaveJSON saveJSON = new SaveJSON(data, users);
            json = JsonHelperItem.ToJson<Data>(data, true);
            string json2 = JsonHelperUser.ToJson<User>(users, true);
            FixJSONSave(json, json2, true);
            //print(json2);

            //dataList = data.OfType<Data>().ToList();
            //FillListMedic(dataList);
        }
        else
        {
            File.WriteAllText(path, "");
        }
    }

    public string FixJSONSave(string json1, string json2, bool pretty = false)
    {
        if (pretty)
        {
            json1 = json1.Replace("]\n}", "],");
            json2 = json2.Replace(json2[0].ToString(), "");

        }
        else
        {
            json1 = json1.Replace("]}", "],");
            json2 = json2.Replace(json2[0].ToString(), "\"");
        }
        print(json1);
        print(json2);
        string temp = json1 + json2;
        print(temp);
        return temp;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

   
}
[Serializable]
public class SaveJSON
{
    Data[] data;
    User[] users;

    public SaveJSON(Data[] data, User[] users)
    {
        this.data = data;
        this.users = users;
    }
}

[Serializable]
public class User
{
    public string nameUser;
    public string mail;
    public string password;
    public DataManager.Profile profile;

    public User(string nameUser, string mail, string password, DataManager.Profile profile)
    {
        this.nameUser = nameUser;
        this.mail = mail;
        this.password = password;
        this.profile = profile;
    }
}


public static class JsonHelperUser
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.User;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.User = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.User = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] User;
    }

}
