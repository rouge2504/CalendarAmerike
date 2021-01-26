using System.Collections;
using System.Collections.Generic;
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

    public void SetClient(Data data)
    {
        inputFieldClient.text = data.clientName;
    }
}
