// Opens an android date picker dialog and grabs the result using a callback.
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.Collections.Generic;

class DatePicker2 : MonoBehaviour
{
    public Toggle toggle1;
    public Toggle toggle2;
    public Toggle toggle3;
    private Dictionary<Toggle, string> toggleLabels = new Dictionary<Toggle, string>();

    public Text AgeText;
    public Slider AgeSlider;
    public TMP_InputField EspIP;
    //public TMP_Text EspIP;
    private float age;
    string B, Espip;
    private static DateTime selectedDate = DateTime.Now;

    private void Start()
    {
        toggleLabels.Add(toggle1, "a");
        toggleLabels.Add(toggle2, "b");
        toggleLabels.Add(toggle3, "c");
        B = selectedDate.ToString();
        age = PlayerPrefs.GetFloat("Age", 0);
        Espip = PlayerPrefs.GetString("EspIP");
        AgeText.text = age.ToString() + " 歲";
        EspIP.text = Espip;
        AgeSlider.value = age;
        LoadCheckedToggles();
    }

    
    class DateCallback : AndroidJavaProxy
    {
        public DateCallback() : base("android.app.DatePickerDialog$OnDateSetListener") { }
        void onDateSet(AndroidJavaObject view, int year, int monthOfYear, int dayOfMonth)
        {
            selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
        }
    }
        
    /*void OnGUI()
    {
        if (GUI.Button(new Rect(15, 15, 450, 75), string.Format("{0:yyyy-MM-dd}", selectedDate)))
        {
            var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                new AndroidJavaObject("android.app.DatePickerDialog", activity, new DateCallback(), selectedDate.Year, selectedDate.Month - 1, selectedDate.Day).Call("show");
            }));
        }
    }*/
    public void Opendate()
    {
        var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            new AndroidJavaObject("android.app.DatePickerDialog", activity, new DateCallback(), selectedDate.Year, selectedDate.Month - 1, selectedDate.Day).Call("show");
        }));
            
    }
    public void GetAge()
    {
        DateTime now = DateTime.Now;
        age = now.Year - selectedDate.Year;
        if (now.Month < selectedDate.Month || (now.Month == selectedDate.Month && now.Day < selectedDate.Day))
        {
            age--;
        }
        AgeText.text = age.ToString() + " 歲";
        AgeSlider.value = age;
    }
    public void GetAgeSlider()
    {
        age = AgeSlider.value;
        //Debug.Log(age);
        AgeText.text = AgeSlider.value + " 歲";
    }
    void LoadCheckedToggles()
    {
        if (PlayerPrefs.HasKey("CheckedLabels"))
        {
            string checkedLabelsString = PlayerPrefs.GetString("CheckedLabels");
            // 將儲存在 PlayerPrefs 中的字串轉換成 List
            List<string> checkedLabels = new List<string>(checkedLabelsString.Split(','));
            
            foreach (var pair in toggleLabels)
            {
                Toggle toggle = pair.Key;
                string label = pair.Value;

                // 檢查每個 Toggle 對應的標籤是否在 PlayerPrefs 中，若在則勾選 Toggle
                //if (checkedLabels.Contains(label))
                //{
                //    toggle.isOn = true;
                //}
                toggle.isOn = checkedLabels.Contains(label);
            }
        }
    }
    public void SaveSettings()
    {
        //Debug.Log(age);
        List<string> checkedLabels = new List<string>();

        foreach (var pair in toggleLabels)
        {
            Toggle toggle = pair.Key;
            string label = pair.Value;

            if (toggle.isOn)
            {
                checkedLabels.Add(label);
            }
        }
        Debug.Log(string.Join(",", checkedLabels));
        string checkedLabelsString = string.Join(",", checkedLabels.ToArray());

        PlayerPrefs.SetFloat("Age", age);
        PlayerPrefs.SetString("EspIP", EspIP.text);
        PlayerPrefs.SetString("CheckedLabels", checkedLabelsString);
        PlayerPrefs.Save();
    }
    private void Update()
    {
        //Debug.Log(age);
       if(B != selectedDate.ToString())
        {
            GetAge();
            B = selectedDate.ToString();
        }
        GetAgeSlider();
    }
}