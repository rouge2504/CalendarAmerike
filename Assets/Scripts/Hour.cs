using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hour : MonoBehaviour
{
    public Text dayNumber;
    public Text contentText;
    public Button openDayDataDay;
    [HideInInspector] public DateTime dateHour;
    [HideInInspector] public string nameHour;
    

    public void SetContent(string dayNumber, string contentText, string  nameHour, DateTime dateHour)
    {
        this.dayNumber.text = dayNumber;
        this.contentText.text = contentText;
        this.nameHour = nameHour;
        this.dateHour = dateHour;
    }

    public void SetContentText(string nameMedic)
    {
        string textToSend = null;
        if (DataManager.instance.tempProfile == DataManager.Profile.CLIENT)
        {
            textToSend = "OCUPADO";
        }
        else
        {
            textToSend = nameMedic;
        }

        contentText.text = textToSend;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
