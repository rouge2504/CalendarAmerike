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

    [HideInInspector] public List<Data> medicList = new List<Data>();
    [HideInInspector] public List<string> medicName;
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
            Data[] data = JsonHelperItem.FromJson<Data>(json);
            dataList = data.OfType<Data>().ToList();
            FillListMedic(dataList);
        }else
        {
            File.WriteAllText(path, "");
        }
    }

    void FillListMedic(List<Data> dataList)
    {
        medicList = dataList.Distinct(new ItemEqualityComparer()).ToList();

        medicName = new List<string>();
        medicName.Add("");
        foreach (var item in medicList)
        {
            print($"DISTINCT ITEM = {item.medicName} ");
            medicName.Add(item.medicName);
        }

        formDayData.dropdownMedic.ClearOptions();
        formDayData.dropdownMedic.AddOptions(medicName);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            string json = File.ReadAllText(path);
            print(json);
            Data[] data = JsonHelperItem.FromJson<Data>(json);
            print(data[0].profile);
        }
    }

    public void FillDataOnForm()
    {
        print("Guardando");
        dataTemp.profile = tempProfile;
        dataTemp.SetDate();

        dataList.Add(new Data(dataTemp));
        string json = JsonHelperItem.ToJson(dataList.ToArray(), true);

        File.WriteAllText(path, json);
    }
   
    public void DateOccupied()
    {

    }


    class ItemEqualityComparer : IEqualityComparer<Data>
    {

        public bool Equals(Data x, Data y)
        {
            // Two items are equal if their keys are equal.
            return x.medicName == y.medicName;
        }

        public int GetHashCode(Data obj)
        {
            return obj.medicName.GetHashCode();
        }
    }

    class HourEqualityComparer : IEqualityComparer<Data>
    {

        public bool Equals(Data x, Data y)
        {
            // Two items are equal if their keys are equal.
            return x.hour == y.hour;
        }

        public int GetHashCode(Data obj)
        {
            return obj.medicName.GetHashCode();
        }
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

    public Data()
    {

    }

    public Data (int day, int month, int year, int hour, string meridian, string clientName, string medicName, DataManager.Profile profile)
    {
        this.day = day;
        this.month = month;
        this.year = year;
        this.hour = hour;
        this.meridian = meridian;
        this.clientName = clientName;
        this.medicName = medicName;
        this.profile = profile;
    }

    public Data (Data item)
    {
        this.day = item.day;
        this.month = item.month;
        this.year = item.year;
        this.hour = item.hour;
        this.meridian = item.meridian;
        this.clientName = item.clientName;
        this.medicName = item.medicName;
        this.profile = item.profile;
    }

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

public static class JsonHelperItem
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
