using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FormDayData : MonoBehaviour
{
    public GameObject contentClient;

    public InputField inputFieldClient;
    public Dropdown dropdownMedic;
    public Text labelMedic;
    public Dropdown dropdownHour;
    public Text toHour;
    public Toggle disponibility;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetForm(string medicName)
    {

        print(medicName);

        //labelMedic.text = medicName;
        for (int i = 0; i < DataManager.instance.medicName.Count; i++)
        {
            if (medicName == DataManager.instance.medicName[i])
            {
                dropdownMedic.value = i;
            }
        }
        contentClient.SetActive((DataManager.instance.tempProfile == DataManager.Profile.CLIENT) ? false : true);
        dropdownMedic.interactable = (DataManager.instance.tempProfile == DataManager.Profile.CLIENT) ? false : true;
    }

    public void SetHours(DateTime time)
    {
        print(time);
        string validateMeridian = time.ToString("tt");
        string validateHour = time.ToString("hh");
        print(validateHour + " " + validateMeridian);
        dropdownHour.ClearOptions();
        var hours = Enumerable.Range(00, 24).Select(i => (DateTime.MinValue.AddHours(i)).ToString("hh:mm tt")).ToList();
        dropdownHour.AddOptions(hours);

        for (int i = 0; i <  hours.Count; i++)
        {
            if (hours[i].Contains(validateMeridian) && hours[i].Contains(validateHour))
            {
                dropdownHour.value = i;
                toHour.text = hours[i + 1];
            }
        }
    }

    public void SetPatient(string client)
    {
        inputFieldClient.text = client;
    }

    public void SetClient(Data data)
    {
        inputFieldClient.text = data.clientName;
    }

    public void ChangeOptionMedic()
    {
        for (int i = 0; i < DataManager.instance.medicName.Count; i++)
        {
            if (dropdownMedic.value == i)
            {
                DataManager.instance.dataTemp.medicName = DataManager.instance.medicName[i];
            }
        }
    }

    public void SetPatientSave()
    {
        DataManager.instance.dataTemp.clientName = inputFieldClient.text;
    }
}
