using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DayChecker : MonoBehaviour
{

    public ScrollRect scrollRect;
    public RectTransform contentPanel;
    public GameObject hourToClone;

    private List<Hour> hoursList;

    public Color stayHourColor;

    public float repeatTimeOnMinutesToUpdate;

    //public RectTransform target;


    void Start()
    {
        SetHours();
        InvokeRepeating("UpdateList", 0, repeatTimeOnMinutesToUpdate * 60);
        //SnapTo(target);
    }

    private void SetHours()
    {
        var hours = Enumerable.Range(00, 24).Select(i => (DateTime.MinValue.AddHours(i)).ToString("hh.mm tt")).ToArray();

        hoursList = new List<Hour>();

        for (int i = 0; i < hours.Count(); i++)
        {
            GameObject clone = Instantiate(hourToClone, contentPanel.GetComponent<Transform>());
            clone.GetComponent<Hour>().SetContent(hours[i], "", hours[i]);
            hoursList.Add(clone.GetComponent<Hour>());
        }




    }

    private void UpdateList()
    {
        DateTime date = DateTime.Now;

        for (int i = 0; i < hoursList.Count(); i++)
        {

            string validateMeridian = date.ToString("tt");
            string validateHour = date.ToString("%h");
            if (hoursList[i].nameHour.Contains(validateMeridian) && hoursList[i].nameHour.Contains(validateHour))
            {
                SnapTo(hoursList[i].GetComponent<RectTransform>());
                SetColor(hoursList[i].GetComponent<Image>());
            }
        }

    }

    public void SetColor(Image target)
    {
        target.color = stayHourColor;
    }

    public void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        contentPanel.anchoredPosition =
            (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position)
            - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
    }
}
