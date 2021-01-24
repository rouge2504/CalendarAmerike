using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hour : MonoBehaviour
{
    public Text dayNumber;
    public Text contentText;
    [HideInInspector] public string nameHour;

    public void SetContent(string dayNumber, string contentText, string  nameHour)
    {
        this.dayNumber.text = dayNumber;
        this.contentText.text = contentText;
        this.nameHour = nameHour;
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
