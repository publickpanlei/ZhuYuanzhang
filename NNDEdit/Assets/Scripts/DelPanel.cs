using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DelPanel : MonoBehaviour {

    public JsonEdit jsonEdit;
    public CellScript nowCell;
    public Button[] button;

    public void SetDelPanel(CellScript _nowCell, Vector2 pos)
    {
        this.gameObject.SetActive(true);
        nowCell = _nowCell;
        this.transform.position = pos;
        button[0].interactable = true;
        button[1].interactable = true;
        button[2].interactable = true;
    }
    public void SetDelPanelTwo(CellScript _nowCell, Vector2 pos)
    {
        this.gameObject.SetActive(true);
        nowCell = _nowCell;
        this.transform.position = pos;
        button[0].interactable = false;
        button[1].interactable = false;
        button[2].interactable = false;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void DelCell()
    {
        nowCell.DelMyselfBegin();
        this.gameObject.SetActive(false);
        nowCell = null;
    }
    public void DelLink()
    {
        nowCell.DelLink();
        this.gameObject.SetActive(false);
        nowCell = null;
    }
    public void viewLink()
    {
        nowCell.viewFatherLine();
        this.gameObject.SetActive(false);
        nowCell = null;
    }
    public void DelLinkNephew()
    {
        nowCell.DelNephew();
        this.gameObject.SetActive(false);
        nowCell = null;
    }
}
