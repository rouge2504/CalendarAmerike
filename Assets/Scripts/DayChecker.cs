﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DayChecker : MonoBehaviour
{
    public static DayChecker instance;

    public ScrollRect scrollRect;
    public RectTransform contentPanel;
    public GameObject hourToClone;

    public GameObject dataDayForm;

    private List<Hour> hoursList;

    public Color stayHourColor;
    public Color ocupadedHourColor;

    public float repeatTimeOnMinutesToUpdate;

    //public RectTransform target;


    void Start()
    {


        instance = this;
        SetHours();
        InvokeRepeating("UpdateList", 0, repeatTimeOnMinutesToUpdate * 60);
        //SnapTo(target);
    }

    void Enable()
    {
        instance = this;
    }

    public void Init()
    {
        UpdateList();

    }

    private void SetHours()
    {
        var hours = Enumerable.Range(00, 24).Select(i => (DateTime.MinValue.AddHours(i)).ToString("hh:mm tt")).ToArray();

        hoursList = new List<Hour>();

        DateTime[] saveHour = new DateTime[hours.Count()];

        for (int i = 0; i < hours.Count(); i++)
        {
            GameObject clone = Instantiate(hourToClone, contentPanel.GetComponent<Transform>());
            string hour = (hours[i].Contains("a. m.")) ? hours[i].Replace("a. m.", "AM") : hours[i].Replace("p. m.", "PM");

            saveHour[i] = DateTime.ParseExact(hour, "hh:mm tt",  CultureInfo.InvariantCulture);

            Hour cloneHour = clone.GetComponent<Hour>();
            cloneHour.SetContent(hour, "", hour, saveHour[i]);
            DateTime dateTimeTemp = saveHour[i];
            cloneHour.openDayDataDay.onClick.AddListener(delegate { OpenDataDayForm(dateTimeTemp); });
            hoursList.Add(cloneHour);
        }
    }


    public void OpenDataDayForm(DateTime hour)
    {
        dataDayForm.SetActive(true);
        DataManager.instance.dataTemp.hour = hour.Hour;
        DataManager.instance.dataTemp.meridian = hour.ToString("tt", CultureInfo.InvariantCulture);
        FillFormDayData();
        //DataManager.instance.FillDataOnForm();

    }

    public void FillFormDayData()
    {
        int hour = DataManager.instance.dataTemp.hour;
            DateTime date = new DateTime(1900,
                         1,
                         1,
                         (DataManager.instance.dataTemp.meridian.ToUpperInvariant() == "PM" && hour < 12) ?
                             hour + 12 : hour,
                         0,
                         00);
        for (int j = 0; j < DataManager.instance.dataList.Count; j++)
        {
            Data data = DataManager.instance.dataList[j];
            DateTime dateTimeTemp = data.GetTime();
            if (DataManager.instance.dataTemp.dateTime.Day == data.GetDate().Day &&
                    DataManager.instance.dataTemp.dateTime.Month == data.GetDate().Month &&
                        DataManager.instance.dataTemp.dateTime.Year == data.GetDate().Year &&
                            date == dateTimeTemp)
            {
                DataManager.instance.formDayData.SetForm(data.medicName);
                DataManager.instance.formDayData.SetClient(data);
                DataManager.instance.formDayData.SetHours(date);
                DataManager.instance.formDayData.SetPatient(data.clientName);
                print("Find");
                return;
            }

        }

        DataManager.instance.formDayData.SetForm("");
        DataManager.instance.formDayData.SetHours(date);
        DataManager.instance.formDayData.SetPatient("");

        print("Empty");
    }

    private void UpdateList()
    {
        DateTime date = DateTime.Now;

        for (int i = 0; i < hoursList.Count(); i++)
        {

            string validateMeridian = date.ToString("tt");
            string validateHour = date.ToString("hh");
            //print(hoursList[i].nameHour);
            validateMeridian = (validateMeridian.Contains("a. m.")) ? validateMeridian.Replace("a. m.", "AM") : validateMeridian.Replace("p. m.", "PM");
            if (hoursList[i].nameHour.Contains(validateMeridian) && hoursList[i].nameHour.Contains(validateHour))
            {
                SnapTo(hoursList[i].GetComponent<RectTransform>());
                SetColor(hoursList[i].GetComponent<Image>(), stayHourColor);
            }

            hoursList[i].contentText.text ="DISPONIBLE";

            for (int j = 0; j < DataManager.instance.dataList.Count; j++)
            {
                Data data = DataManager.instance.dataList[j];
                DateTime dateTimeTemp = data.GetTime();
                if (DataManager.instance.dataTemp.dateTime.Day == data.GetDate().Day &&
                        DataManager.instance.dataTemp.dateTime.Month == data.GetDate().Month &&
                            DataManager.instance.dataTemp.dateTime.Year == data.GetDate().Year &&
                                dateTimeTemp.Hour == hoursList[i].dateHour.Hour)
                {
                    validateMeridian = dateTimeTemp.ToString("tt");
                    validateHour = dateTimeTemp.ToString("%h");
                    string hour = ChangeMeridian(hoursList[i].nameHour, false);
                    if (hour.Contains(validateMeridian) && hour.Contains(validateHour))
                    {
                        SnapTo(hoursList[i].GetComponent<RectTransform>());
                        SetColor(hoursList[i].GetComponent<Image>(), ocupadedHourColor);
                        hoursList[i].openDayDataDay.gameObject.SetActive((DataManager.instance.tempProfile == DataManager.Profile.CLIENT) ? false : true);
                        hoursList[i].SetContentText(data.medicName);

                    }
                }

            }
        }

    }

    public void CleanHours()
    {
        for (int i = 0; i < hoursList.Count(); i++)
        {
            SetColor(hoursList[i].GetComponent<Image>(), Color.white);
            hoursList[i].contentText.text = "";
        }

    }

    public void SetColor(Image target, Color color)
    {
        target.color = color;
    }

    public void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        contentPanel.anchoredPosition =
            (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position)
            - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
    }

    public string ChangeMeridian(string meridian, bool Upper)
    {
       
        if (Upper)
        {
            meridian = (meridian.Contains("a. m.")) ? meridian.Replace("a. m.", "AM") : meridian.Replace("p. m.", "PM");
        }
        else
        {
            meridian = (meridian.Contains("AM")) ? meridian.Replace("AM", "a. m.") : meridian.Replace("PM","p. m." );
        }
        return meridian;
    }
}
