using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public enum Profile { MEDIC, RECEPCIONIST, CLIENT }

    public Profile tempProfile;

    public FormDayData formDayData;

    public string fileName;

    private string path;

    [HideInInspector] public Data dataTemp;

    [HideInInspector] public List<Data> dataList = new List<Data>();
    // Start is called before the first frame update
    void Start()
    {
        path = Path.Combine(Application.streamingAssetsPath, fileName);

        instance = this;
        dataTemp = new Data();
        RecoveryData();
    }

    void RecoveryData()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Data[] data = JsonHelper.FromJson<Data>(json);
            dataList = data.OfType<Data>().ToList();
        }else
        {
            File.WriteAllText(path, "");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            string json = File.ReadAllText(path);
            print(json);
            Data[] data = JsonHelper.FromJson<Data>(json);
            print(data[0].profile);
        }
    }

    public void FillDataOnForm()
    {
        
        dataTemp.profile = tempProfile;
        dataTemp.SetDate();
        dataList.Add(dataTemp);
        string json = JsonHelper.ToJson(dataList.ToArray(), true);
        File.WriteAllText(path, json);
    }
   
    public void DateOccupied()
    {

    }



}
[Serializable]
public class Data
{
    public DateTime dateTime;
    public int day;
    public int month;
    public int year;
    public int hour;
    public string meridian;
    public string clientName;
    public string medicName;
    public DataManager.Profile profile;

    public void SetDate()
    {
        year = dateTime.Year;
        month = dateTime.Month;
        day = dateTime.Day;
    }

    public DateTime GetDate()
    {
        return new DateTime(year,month,day);
    }

    public DateTime GetTime()
    {

        int year = 1900;
        int month = 1;
        int day = 1;
        int minutes = 00;
        String format = meridian;
        DateTime date = new DateTime(year,
                                 month,
                                 day,
                                 (format.ToUpperInvariant() == "PM" && hour < 12) ?
                                     hour + 12 : hour,
                                 minutes,
                                 00);
        return date; 
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }



}
