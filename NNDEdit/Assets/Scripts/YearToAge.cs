using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YearToAge : MonoBehaviour {

    public Text textAge;
    private InputField thisInputField;
    int age = 0;
    int year = 0;

    public void toAge()
    {
        age = 0;
        year = 0;
        if (int.TryParse(thisInputField.text, out year))
        {
            age = year - 1328;
        }
        age = Mathf.Clamp(age, 0, 100);
        textAge.text = age + "";
    }
    public int getYear()
    {
        return year;
    }
    public int getAge()
    {
        return age;
    }
    void Awake ()
    {
        thisInputField = this.GetComponent<InputField>();
    }
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
