using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class timer : MonoBehaviour
{
    private float TimeLeft = 60;
   

    public TextMeshProUGUI TimerTxt;

   

    // Update is called once per frame
    void Update()
    {
        TimeLeft -= Time.deltaTime;
        TimerTxt.text = ((int)TimeLeft).ToString();
    }
}
