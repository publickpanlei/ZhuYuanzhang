using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TaostScript : MonoBehaviour {

	public static Text myText ;
    static int t = 0;
	// Use this for initialization
	void Start () {
		myText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(t>0)
        {
            t--;
            if(t==0)
            {
                SetTaost("");
                this.gameObject.SetActive(false);
            }
        }
	}
	public void SetTaost(string s)
	{
		myText.text = s;
        t = 100;
        this.gameObject.SetActive(true);
	}
}
