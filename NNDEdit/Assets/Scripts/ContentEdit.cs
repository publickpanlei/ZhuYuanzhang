using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentEdit : MonoBehaviour {

    public RectTransform content;
    int timeUpdate = 0;
    public static float distance = 1;
    float mSpeed = 0.5f;
    public DelPanel delPanel;
    public float GetScale()
    {
        return content.localScale.x;
    }
    public void SeeHeight(float y)
    {
        float length = Mathf.Abs(content.position.y - y) / content.localScale.y;
        if (length > JsonEdit.layerHeight)
        {
            Debug.Log("SeeHeight " + JsonEdit.layerHeight);
            if (length > 415-100)
            {
                JsonEdit.layerHeight = length;
                content.sizeDelta = new Vector2(content.rect.width, JsonEdit.layerHeight * 2 + 200);
            }
        }
    }
    public void SeeWeight(float x)
    {
        float length = Mathf.Abs(content.position.x - x) / content.localScale.x;
        if (length > JsonEdit.layerWeight)
        {
            Debug.Log("SeeWeight " + JsonEdit.layerWeight);
            if (length > 1280)
            {
                JsonEdit.layerWeight = length;
                content.sizeDelta = new Vector2(JsonEdit.layerWeight + 400, content.rect.height);
            }
        }
    }
    public void contentScale(int i)//0小 1大
    {
        if (i == 0)
        {
            if (content.localScale.x > 0.3f)
            {
                content.localScale = new Vector2(content.localScale.x - 0.2f, content.localScale.x - 0.2f);
            }
        }
        else if (i == 1)
        {
            if (content.localScale.x < 1.1f)
            {
                content.localScale = new Vector2(content.localScale.x + 0.2f, content.localScale.x + 0.2f);
            }
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timeUpdate++;
        if (timeUpdate % 500 == 0)
        {
            //    Debug.Log("XmlEdit.cellIdNew " + XmlEdit.cellIdNew);
        }
        //鼠标滚轮 放大缩小
        distance += Input.GetAxis("Mouse ScrollWheel") * mSpeed * distance;
        distance = Mathf.Clamp(distance, 0.1f, 1.5f);
        content.localScale = new Vector2(distance, distance);
	}
    public void RightButtonClick(CellScript cell, Vector2 pos)
    {

    }
    public void RightButtonClick(CellScript cell, Vector2 pos, int thisType)
    {
        if (thisType <= 4)
        {
            delPanel.SetDelPanel(cell, pos);
        }
        else if (thisType == 31 || thisType == 32)
        {
            delPanel.SetDelPanelTwo(cell, pos);
        }
        
    }
}
