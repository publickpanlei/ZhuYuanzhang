using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountScript : MonoBehaviour {

    public Text[] countText;
    int[] countNum = { 0, 0, 0, 0, 0 };
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void addCellCount(int t,int i)
    {
        if(t>=0&&t<=5)
        {
            countNum[t] += i;
            countText[t].text = countNum[t] + "";
        }
    }
    public void clearCount()
    {
        for(int i=0;i<5;i++)
        {
            countNum[i] = 0;
            countText[i].text = countNum[i] + "";
        }
    }
    public int getCount(int i)
    {
        return countNum[i];
    }
}
