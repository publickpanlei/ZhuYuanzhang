using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using LitJson;

public class CellScript : MonoBehaviour {

    public int cellId=0;
    public int cellFatherId = -1;
    public int isUncle = 0;//1是叔叔 0不是
    public string cellName;
    public string cellText;
    public int cellLeftId = -1;
    public int cellRightId = -1;
    //-1未做选择 0打包 1单分支 2双分支 3结局 10打包的最后一个 3分支之后的那个----old
    //0 HS 1 VS 2 PJ 3 PC 4 LE //3 PC BS 30 PC 31 PCA 32 PCB -1未定 0-2 4
    public int cellType;
    public int isGirl;//-1未知 1带箭头 0不带箭头 2不带箭头但能移动
    private int fatherType = -1;
    private int sonId = 0;//0 left 1 right
    public float cellPosX;
    public float cellPosY;
    public string leftText;
    public string rightText;
    JsonData jsonData;


    public InputField inputName;
    public InputField inputText;
    public InputField inputLeftText;
    public InputField inputRightText;
    public RectTransform lineFather;
    public Transform onlySonPoint;
    private Transform fatherPoint;
    public Transform nephewPoint;
    public GameObject buttonRight;
    public RectTransform lineNephew;//侄子连线

    public GameObject cellObj;
    public GameObject cellBranchObj;
    private JsonEdit jsonEdit;
    private ContentEdit contentEdit;
    private CountScript countScript;
    public GameObject leftSon;
    public GameObject rightSon;
    public GameObject father;
    public GameObject buttonToLink;
    public GameObject panelCreate;
    public GameObject buttonToFindNephew;
    private float[,] cellColors = new float[3, 3] { { 0.8f, 0.8f, 0.8f }, { 0.6f, 1, 0.7f }, { 0.6f, 0.7f, 1 } };
    private CellScript[] uncles=new CellScript[5];
    private int[] unclesId={-1,-1,-1,-1,-1};

    public void SetCell(JsonData _jsonData)
    {
        jsonData = _jsonData;
        cellId = GetJsonData("id",-1);
        cellFatherId = GetJsonData("fatherId", -1);
        cellType = GetJsonData("type", -1);
        isGirl = GetJsonData("isGirl", -1);
        isUncle = GetJsonData("isUncle", 0);
        cellName = GetJsonData("cellName");
        cellText = GetJsonData("cellText");
        cellLeftId = GetJsonData("leftId", -1);
        cellRightId = GetJsonData("rightId", -1);
        inputName.text = cellName;
        inputText.text = cellText;
        leftText = GetJsonData("leftText");
        rightText = GetJsonData("rightText");
        if(cellType==30)
        {
            inputLeftText.text = leftText;
            inputRightText.text = rightText;
        }
        cellPosX = GetJsonData("cellPosX", 60.0f);
        cellPosY = GetJsonData("cellPosY", 60.0f);
        this.transform.position = new Vector2(cellPosX, cellPosY);
        if (isGirl == 0)
        {
            isADaughter(true);
        }
        else if (isGirl == 2) 
        {
            beLeave();
        }
        if (cellId == 0)
        {
            isADaughter(true); 
        }
        if(cellType<=2)
        {
            this.GetComponent<CellScript>().inputName.GetComponent<Image>().color = new Color(cellColors[cellType, 0], cellColors[cellType, 1], cellColors[cellType, 2]);
            this.GetComponent<CellScript>().inputText.GetComponent<Image>().color = new Color(cellColors[cellType, 0], cellColors[cellType, 1], cellColors[cellType, 2]);
        }
        if (cellLeftId != -1 && cellType <= 2 || cellType == 4 || isUncle == 1)
        {
            this.GetComponent<CellScript>().buttonRight.SetActive(false);
        }
        if (isUncle == 1)
        {
            this.GetComponent<CellScript>().lineNephew.gameObject.SetActive(true);
        }
        countScript = GameObject.Find("PanelCount").GetComponent<CountScript>();
        countScript.addCellCount(cellType, 1);
        if (JsonEdit.cellLoadNum>=0)
        {
            JsonEdit.cellLoadNum++;
        }        
    }
    public void SeeWeight()
    {
        StartCoroutine(SeeWeight(0.02f, this.transform.position.x));
    }
    public IEnumerator SeeWeight(float f,float x)
    {
        yield return new WaitForSeconds(f);
        contentEdit.SeeWeight(x);
        if (cellLeftId != -1 && isUncle == 0)
        {
            leftSon.GetComponent<CellScript>().SeeWeight();
        }
        if (cellRightId != -1)
        {
            rightSon.GetComponent<CellScript>().SeeWeight();
        }
    }
    public void SeeHeight()
    {
        StartCoroutine(SeeHeight(0.02f, this.transform.position.y));
    }
    public IEnumerator SeeHeight(float f, float y)
    {
        yield return new WaitForSeconds(f);
        contentEdit.SeeHeight(y);
        if(cellLeftId!=-1 && isUncle==0)
        {
            leftSon.GetComponent<CellScript>().SeeHeight();
        }
        if (cellRightId != -1)
        {
            rightSon.GetComponent<CellScript>().SeeHeight();
        }
    }
    public void WriteIn(string str)
    {
        if(str.Equals("name"))
        {
            cellName = inputName.text;
        }
        else if (str.Equals("text"))
        {
            cellText = inputText.text;
        }
        else if (str.Equals("left"))
        {
            leftText = inputLeftText.text;
        }
        else if (str.Equals("right"))
        {
            rightText = inputRightText.text;
        }
        if (str.Equals("namepc"))
        {
            cellName = inputName.text;
            father.GetComponent<CellScript>().cellName = inputName.text+"BS";
            father.GetComponent<CellScript>().inputName.text = inputName.text + "BS";
            leftSon.GetComponent<CellScript>().cellName = inputName.text + "B";
            leftSon.GetComponent<CellScript>().inputName.text = inputName.text + "B";
            rightSon.GetComponent<CellScript>().cellName = inputName.text + "A";
            rightSon.GetComponent<CellScript>().inputName.text = inputName.text + "A";
        }
    }
	// Use this for initialization
	void Start () {
        jsonEdit = GameObject.Find("PanelCellWorld").GetComponent<JsonEdit>();
//        xmlEdit = GameObject.Find("PanelCellWorld").GetComponent<XmlEdit>();
        contentEdit = GameObject.Find("PanelCellWorld").GetComponent<ContentEdit>();       
        cellObj = jsonEdit.cellObj;
        cellBranchObj = jsonEdit.cellBranchObj;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void LoadSonWait()
    {
        StartCoroutine(LoadSon(0.02f));
    //    LoadSon(cellLeftId);
    }
    public IEnumerator LoadSon(float f)
    {
        yield return new WaitForSeconds(f);
        LoadSon(cellLeftId);
    }
    void LoadSon(int sonCellId)
    {
    //    Debug.Log("JsonEdit.cellJsonCount " + JsonEdit.cellJsonCount);
    //    Debug.Log("JsonEdit.cellLoadNum " + JsonEdit.cellLoadNum);
        if (JsonEdit.cellLoadNum >= JsonEdit.cellJsonCount)
        {
        //    Debug.Log("JsonEdit.cellLoadNum " + JsonEdit.cellLoadNum);
            JsonEdit.cellLoadNum = -1;
            jsonEdit.LoadNephew();
        }
        if(sonCellId==-1 || isUncle==1)
        {
            return;
        }
        for (int i = 0; i < JsonEdit.jsonCells.Count; i++)
        {
            if (JsonEdit.jsonCells[i]["id"] + "" == sonCellId + "")
            {
                string s = JsonEdit.jsonCells[i]["type"] + "";
                int j = -1;
                int.TryParse(s, out j);
                if(j<=2||j==4)
                {
                    GameObject obj = Instantiate(cellObj, this.transform);
                    obj.GetComponent<CellScript>().SetCell(JsonEdit.jsonCells[i]);
                    JsonEdit.obj_cells.Add(obj);
                    obj.GetComponent<CellScript>().LoadSonWait();
                    obj.GetComponent<CellScript>().SetFather(this.gameObject);
                    obj.transform.parent = this.transform;
                    leftSon = obj;
                }
                else if(j==3)
                {
                    GameObject obj = Instantiate(cellBranchObj, this.transform);
                    obj.GetComponent<CellScript>().SetCell(JsonEdit.jsonCells[i]);
                    JsonEdit.obj_cells.Add(obj);
                    obj.GetComponent<CellScript>().SetFather(this.gameObject);
                    obj.transform.parent = this.transform;
                    leftSon = obj;
                    obj.GetComponent<CellScript>().LoadBranchSon(obj.GetComponent<CellScript>().cellLeftId,i+1);
                }
                break;
            }
        }
    }
    public void LoadBranchSon(int sonCellId,int i)
    {
        string s = JsonEdit.jsonCells[i]["type"] + "";
        int j = -1;
        int.TryParse(s, out j);
        if (j == 30)
        {
            leftSon.GetComponent<CellScript>().SetCell(JsonEdit.jsonCells[i]);
            JsonEdit.obj_cells.Add(leftSon);
            leftSon.GetComponent<CellScript>().LoadBranchSon(leftSon.GetComponent<CellScript>().cellLeftId, i + 1);
            leftSon.GetComponent<CellScript>().LoadBranchSon(leftSon.GetComponent<CellScript>().cellRightId, i + 2);
        }
        else if (j == 31)
        {
            leftSon.GetComponent<CellScript>().SetCell(JsonEdit.jsonCells[i]);
            JsonEdit.obj_cells.Add(leftSon);
            leftSon.GetComponent<CellScript>().LoadSonWait();
        }
        else if (j == 32)
        {
            rightSon.GetComponent<CellScript>().SetCell(JsonEdit.jsonCells[i]);
            JsonEdit.obj_cells.Add(rightSon);
            rightSon.GetComponent<CellScript>().LoadSonWait();
        }
    }
    public void viewFatherLine()
    {
        isADaughter(false);
        transform.localPosition = new Vector2(400, 0);
        fatherPoint = father.GetComponent<CellScript>().onlySonPoint;
        setFatherLine();
        SeeWeight();
    }
    public void SetFather(GameObject obj)
    {
        father = obj;
        fatherType = father.GetComponent<CellScript>().cellType;
        if (fatherType == 31 || fatherType==32)
        {
            father.GetComponent<CellScript>().buttonRight.SetActive(false);
        }
        if (isGirl == 1)
        {
            fatherPoint = father.GetComponent<CellScript>().onlySonPoint;
            setFatherLine();
        }
    }
    public void setFatherLine()
    {
        if (isGirl==1)
        {            
            Vector3 targetDir = transform.position - fatherPoint.position; // 目标坐标与当前坐标差的向量
        //    Debug.Log("targetDir.position " + targetDir.x + " " + targetDir.y);
            float fad = Mathf.Atan(targetDir.y / targetDir.x);
            float ang = fad * 180 / Mathf.PI;
            if (targetDir.x < 0)
            {
                ang += 180;
            }
        //    Debug.Log("ang " + ang); 
            lineFather.eulerAngles = new Vector3(0, 0, ang);
            lineFather.sizeDelta = new Vector2(Mathf.Abs(targetDir.x / Mathf.Cos(fad)) * JsonEdit.lineLengthX / ContentEdit.distance, 10);
        }
   //     Debug.Log("JsonEdit.cellLoadNum " + JsonEdit.cellLoadNum); 
        if (JsonEdit.cellLoadNum==-1)
        {
            setUnclesLine();
            for (int i = 0; i < 5; i++)
            {
                if (unclesId[i] != -1)
                {
                    uncles[i].setUnclesLine();
                }
            }
            setSonUnclesLine();
        }
    }
    public void setSonUnclesLine()
    {
        setUnclesLine();
        for (int i = 0; i < 5; i++)
        {
            if (unclesId[i] != -1)
            {
                uncles[i].setUnclesLine();
            }
        }
        if(isUncle==0)
        {
            if (cellLeftId != -1)
            {
                leftSon.GetComponent<CellScript>().setSonUnclesLine();
            }
            if (cellRightId != -1)
            {
                rightSon.GetComponent<CellScript>().setSonUnclesLine();
            }
        }        
    }
    public void setUnclesLine()
    {
        if (isUncle == 1 && nephewPoint != null)
        {
            Vector3 targetDir = nephewPoint.position - onlySonPoint.position; // 目标坐标与当前坐标差的向量
            float fad = Mathf.Atan(targetDir.y / targetDir.x);
            float ang = fad * 180 / Mathf.PI;
            if (targetDir.x < 0)
            {
                ang += 180;
            }
            //    Debug.Log("ang " + ang); 
            lineNephew.eulerAngles = new Vector3(0, 0, ang);
            lineNephew.sizeDelta = new Vector2(Mathf.Abs(targetDir.x / Mathf.Cos(fad)) * JsonEdit.lineLengthX / ContentEdit.distance, 10);
        }
    }
    public void CreateSon(int i)
    {
        if (jsonEdit.wantFindNephewId !=-1)//如果有连接侄子请求
        {
            jsonEdit.wantFindNephew.buttonToFindNephew.GetComponent<Animator>().Play("Stop");
            jsonEdit.wantFindNephew = null;
            jsonEdit.wantFindNephewId = -1;
        }
        if (cellLeftId == -1)
        {
            haveCreated();
            cellLeftId = JsonEdit.cellIdNew;
            GameObject obj = null;
            if (i <= 2)
            {
                obj = Instantiate(cellObj, this.transform);
                if (cellType <= 2)
                {
                    obj.transform.localPosition = new Vector2(0, -60);
                }
                else
                {
                    obj.transform.localPosition = new Vector2(400, 0);
                    obj.GetComponent<CellScript>().isGirl = 1;
                }
                buttonRight.SetActive(false);
            }
            else if (i == 4)
            {
                obj = Instantiate(cellObj, this.transform);
                obj.transform.localPosition = new Vector2(400, 0);
                buttonRight.SetActive(false);
                obj.GetComponent<CellScript>().isGirl = 1;
            }
            else if (i == 3)
            {
                obj = Instantiate(cellBranchObj, this.transform);
                obj.transform.localPosition = new Vector2(400, 0);
                buttonRight.SetActive(false);
            }
            JsonData newJson = new JsonData();
            newJson["id"] = cellLeftId;
            newJson["cellName"] = getName(i);
            newJson["cellText"] = "";
            newJson["leftText"] = "";
            newJson["rightText"] = "";
            newJson["leftId"] = -1;
            newJson["rightId"] = -1;
            newJson["fatherId"] = cellId;
            newJson["type"] = i;
            newJson["isGirl"] = obj.GetComponent<CellScript>().isGirl;
            newJson["isUncle"] = obj.GetComponent<CellScript>().isUncle;
            newJson["cellPosX"] = obj.transform.position.x;
            newJson["cellPosY"] = obj.transform.position.y;
            obj.GetComponent<CellScript>().SetCell(newJson);
            JsonEdit.obj_cells.Add(obj);
            JsonEdit.cellIdNew++;
            obj.GetComponent<CellScript>().SeeHeight();
            obj.GetComponent<CellScript>().SeeWeight();
            obj.GetComponent<CellScript>().SetFather(this.gameObject);
            obj.transform.parent = this.transform;
            leftSon = obj;
            if (i == 3)
            {
                obj.GetComponent<CellScript>().setSon(30);
            }
        }
    }
    void setSon(int i)//30 31 32
    {
        if (i == 30)
        {
            cellLeftId = JsonEdit.cellIdNew;            
            JsonData newJson = new JsonData();
            newJson["id"] = cellLeftId;
            newJson["cellName"] = getName(i);
            newJson["cellText"] = "";
            newJson["leftText"] = "";
            newJson["rightText"] = "";
            newJson["leftId"] = -1;
            newJson["rightId"] = -1;
            newJson["fatherId"] = cellId;
            newJson["type"] = i;
            newJson["isGirl"] = 0;
            newJson["isUncle"] = 0;
            newJson["cellPosX"] = leftSon.transform.position.x;
            newJson["cellPosY"] = leftSon.transform.position.y;
            leftSon.GetComponent<CellScript>().SetCell(newJson);
            JsonEdit.obj_cells.Add(leftSon);
            JsonEdit.cellIdNew++;
            leftSon.GetComponent<CellScript>().setSon(31);
            leftSon.GetComponent<CellScript>().setSon(32);
        }
        else if( i == 31)
        {
            cellLeftId = JsonEdit.cellIdNew;
            JsonData newJson = new JsonData();
            newJson["id"] = cellLeftId;
            newJson["cellName"] = getName(i);
            newJson["cellText"] = "";
            newJson["leftText"] = "";
            newJson["rightText"] = "";
            newJson["leftId"] = -1;
            newJson["rightId"] = -1;
            newJson["fatherId"] = cellId;
            newJson["type"] = i;
            newJson["isGirl"] = 0;
            newJson["isUncle"] = 0;
            newJson["cellPosX"] = leftSon.transform.position.x;
            newJson["cellPosY"] = leftSon.transform.position.y;
            leftSon.GetComponent<CellScript>().SetCell(newJson);
            JsonEdit.obj_cells.Add(leftSon);
            JsonEdit.cellIdNew++;
        }
        else if (i == 32)
        {
            cellRightId = JsonEdit.cellIdNew;
            JsonData newJson = new JsonData();
            newJson["id"] = cellRightId;
            newJson["cellName"] = getName(i);
            newJson["cellText"] = "";
            newJson["leftText"] = "";
            newJson["rightText"] = "";
            newJson["leftId"] = -1;
            newJson["rightId"] = -1;
            newJson["fatherId"] = cellId;
            newJson["type"] = i;
            newJson["isGirl"] = 0;
            newJson["isUncle"] = 0;
            newJson["cellPosX"] = rightSon.transform.position.x;
            newJson["cellPosY"] = rightSon.transform.position.y;
            rightSon.GetComponent<CellScript>().SetCell(newJson);
            JsonEdit.obj_cells.Add(rightSon);
            JsonEdit.cellIdNew++;
            rightSon.GetComponent<CellScript>().SeeWeight();
        }
    }
    public void SonDelete()
    {
        cellLeftId = -1;
        leftSon = null;
        buttonRight.SetActive(true);
    }
    public void DelLink()
    {
        if (cellFatherId!=-1)
        {
            father.GetComponent<CellScript>().SonDelete();
            fatherPoint = null;
            father = null;
            cellFatherId = -1;
            fatherType = -1;
            sonId = -1;
            this.transform.parent = jsonEdit.cellRoot;
            beLeave();
        }
    }
    public void DelMyselfBegin()
    {
        if(isGirl==2)
        {

        }
        else
        {
            father.GetComponent<CellScript>().SonDelete();
        }        
        DelMyself();
    }
    public void DelMyself()
    {
        if(cellLeftId!=-1)
        {
            if(isUncle==0)
            {
                cellLeftId = -1;
                leftSon.GetComponent<CellScript>().DelMyself();  
            }
            else if(isUncle==1)
            {
                cellLeftId = -1;
                leftSon.GetComponent<CellScript>().DelUncle(cellId);
            }
        }
        if(cellRightId!=-1)
        {
            cellRightId = -1;
            rightSon.GetComponent<CellScript>().DelMyself();
        }
        for (int i = 0; i < 5; i++)
        {
            if (unclesId[i] != -1)
            {
                uncles[i].DelNephew();
            }
        }
        this.gameObject.SetActive(false);
        if(jsonEdit.wantCreateSonId == cellId)
        {
            jsonEdit.wantCreateSon = null;
            jsonEdit.wantCreateSonId = -1;
        }
        if (jsonEdit.wantLinkSonId == cellId)
        {
            jsonEdit.wantLinkSon = null;
            jsonEdit.wantLinkSonId = -1;
        }
        JsonEdit.obj_cells.Remove(this.gameObject);
        countScript.addCellCount(cellType, -1);
        Destroy(this.gameObject, 1);
    }
    public void DelUncle(int j)
    {
        for (int i = 0; i < 5; i++)
        {
            if (unclesId[i] == j)
            {
                unclesId[i] = -1;
                uncles[i] = null;
                break;
            }
        }
    }
    public void DelNephew()
    {
        if(isUncle==1)
        {
            isUncle = 0;
            lineNephew.gameObject.SetActive(false);
            nephewPoint = null;
            buttonRight.SetActive(true);
            leftSon = null;
            cellLeftId = -1;
        }
    }
    private string GetJsonData(string s_name)
    {
    //    Debug.Log("GetJsonData " + s_name + " " + jsonData[s_name]);
        return jsonData[s_name] + "";
    }
    private int GetJsonData(string s_name, int s_default)
    {
        string s = jsonData[s_name] + "";
        int i = 0;
        if (int.TryParse(s, out i))
        {
            return i;
        }
        return s_default;
    }
    private float GetJsonData(string s_name, float s_default)
    {
        string s = jsonData[s_name] + "";
        float i = 0;
        if (float.TryParse(s, out i))
        {
            return i;
        }
        return s_default;
    }
    public void isADaughter(bool b)
    {
        if(b)
        {
            if(cellType<=4)
            {
                lineFather.gameObject.SetActive(false);
            }            
            isGirl = 0;
            this.GetComponent<MyDrag>().SetCanDrag(false);//girl节点不能移动
        }
        else
        {
            lineFather.gameObject.SetActive(true);
            isGirl = 1;
            this.GetComponent<MyDrag>().SetCanDrag(true);//节点能移动
        }
    }
    public void beLeave()
    {
        lineFather.gameObject.SetActive(false);
        isGirl = 2;
        this.GetComponent<MyDrag>().SetCanDrag(true);//节点能移动
        buttonToLink.gameObject.SetActive(true);
    }
    public void wantToLink()
    {
        if (jsonEdit.wantLinkSonId == -1)//新点
        {
            buttonToLink.GetComponent<Animator>().Play("CellWantToLink");
            jsonEdit.wantLinkSon = this;
            jsonEdit.wantLinkSonId = cellId;
        }
        else if (jsonEdit.wantLinkSonId == cellId)//点自己的，取消
        {
            buttonToLink.GetComponent<Animator>().Play("Stop");
            jsonEdit.wantLinkSon = null;
            jsonEdit.wantLinkSonId = -1;
        }
        else//点其他的，替代
        {
            jsonEdit.wantLinkSon.buttonToLink.GetComponent<Animator>().Play("Stop");
            this.buttonToLink.GetComponent<Animator>().Play("CellWantToLink");
            jsonEdit.wantLinkSon = this;
            jsonEdit.wantLinkSonId = cellId;
        }
    }

    public void wantToCreate()
    {
        if (jsonEdit.wantLinkSonId == -1)
        {
            if (jsonEdit.wantCreateSonId == -1)//新点
            {
                panelCreate.SetActive(true);
                jsonEdit.wantCreateSon = this;
                jsonEdit.wantCreateSonId = cellId;
            }
            else if (jsonEdit.wantCreateSonId == cellId)//点自己的，取消
            {
                panelCreate.SetActive(false);
                jsonEdit.wantCreateSon = null;
                jsonEdit.wantCreateSonId = -1;
                if (jsonEdit.wantFindNephewId == cellId)//如果本身还是连接侄子请求
                {
                    buttonToFindNephew.GetComponent<Animator>().Play("Stop");
                    jsonEdit.wantFindNephew = null;
                    jsonEdit.wantFindNephewId = -1;
                }
            }
            else//点其他的，替代
            {
                jsonEdit.wantCreateSon.panelCreate.SetActive(false);
                jsonEdit.wantCreateSon = this;
                jsonEdit.wantCreateSonId = cellId;
                panelCreate.SetActive(true);
                if (jsonEdit.wantFindNephewId != -1)//如果有连接侄子请求
                {
                    jsonEdit.wantFindNephew.buttonToFindNephew.GetComponent<Animator>().Play("Stop");
                    jsonEdit.wantFindNephew = null;
                    jsonEdit.wantFindNephewId = -1;
                }
            }
        }
        else
        {
            if (jsonEdit.wantCreateSonId == cellId)
            {
                panelCreate.SetActive(false);
                jsonEdit.wantCreateSon = null;
                jsonEdit.wantCreateSonId = -1;
            }
            toLink();
        }
    }
    void haveCreated()
    {
        panelCreate.SetActive(false);
        jsonEdit.wantCreateSon = null;
        jsonEdit.wantCreateSonId = -1;
    }
    bool canLink(int i)
    {
        if (cellFatherId != -1)
        {
            if (cellFatherId == i)
            {
                return false;
            }
            else
            {
                if (!father.GetComponent<CellScript>().canLink(i))
                {
                    return false;
                }
            }
        }
        return true;
    }
    void toLink()
    {
        if (jsonEdit.wantLinkSon.cellId == cellId)
        {
            Debug.Log("不可以和它是同一个节点");
        //    TaostScript.SetTaost("不可以和它是同一个节点");
            return;
        }
        if (!canLink(jsonEdit.wantLinkSon.cellId))
        {
            Debug.Log("它不可以是我的祖先");
         //   TaostScript.SetTaost("它不可以是我的祖先");
            return;
        }
        int wantSonType = jsonEdit.wantLinkSon.cellType;
        if (cellType <= 2 && wantSonType <= 2)
        {
            haveLink(new Vector2(0, -60),true);
        }
        else
        {     
            haveLink(new Vector2(400,0),false);
        }
    }
    void haveLink(Vector2 v2,bool isDau)
    {
        jsonEdit.wantLinkSon.transform.parent = this.transform;
        jsonEdit.wantLinkSon.father = this.gameObject;
        jsonEdit.wantLinkSon.cellFatherId = cellId;
        if(isDau==false)
        {
            jsonEdit.wantLinkSon.fatherPoint = onlySonPoint;  
        }
        jsonEdit.wantLinkSon.sonId = 0;
        leftSon = jsonEdit.wantLinkSon.gameObject;
        cellLeftId = jsonEdit.wantLinkSon.cellId;
        jsonEdit.wantLinkSon.fatherType = cellType;  
        jsonEdit.wantLinkSon.isADaughter(isDau);
        jsonEdit.wantLinkSon.transform.localPosition = v2;
        jsonEdit.wantLinkSon.setFatherLine();
        jsonEdit.wantLinkSon.buttonToLink.GetComponent<Animator>().Play("Stop");
        jsonEdit.wantLinkSon.buttonToLink.gameObject.SetActive(false);
        jsonEdit.wantLinkSon.SeeHeight();
        jsonEdit.wantLinkSon.SeeWeight();
        jsonEdit.wantLinkSon = null;
        jsonEdit.wantLinkSonId = -1;
        buttonRight.SetActive(false);
    }
    public void wantToFindNephew()
    {
        if (jsonEdit.wantFindNephewId == -1)//新点
        {
            buttonToFindNephew.GetComponent<Animator>().Play("CellWantToFind");
            jsonEdit.wantFindNephew = this;
            jsonEdit.wantFindNephewId = cellId;
        }
        else if (jsonEdit.wantFindNephewId == cellId)//点自己的，取消
        {
            buttonToFindNephew.GetComponent<Animator>().Play("Stop");
            jsonEdit.wantFindNephew = null;
            jsonEdit.wantFindNephewId = -1;
        }
    }
    public void findNephew()
    {
        if (jsonEdit.wantFindNephewId != -1)//
        {
            if(cellType<=4)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (unclesId[i] == -1)
                    {
                        Debug.Log("侄儿");
                        jsonEdit.wantFindNephew.buttonToFindNephew.GetComponent<Animator>().Play("Stop");
                        jsonEdit.wantFindNephew.isUncle = 1;
                        jsonEdit.wantFindNephew.lineNephew.gameObject.SetActive(true);
                        jsonEdit.wantFindNephew.cellLeftId = this.cellId;
                        jsonEdit.wantFindNephew.leftSon = this.gameObject;
                        jsonEdit.wantFindNephew.nephewPoint = this.transform;
                        jsonEdit.wantFindNephew.setUnclesLine();
                        setUncle(jsonEdit.wantFindNephew, jsonEdit.wantFindNephew.cellId);
                        jsonEdit.wantFindNephew = null;
                        jsonEdit.wantFindNephewId = -1;
                        jsonEdit.wantCreateSon.panelCreate.SetActive(false);
                        jsonEdit.wantCreateSon.buttonRight.SetActive(false);
                        jsonEdit.wantCreateSon = null;
                        jsonEdit.wantCreateSonId = -1;
                        break;
                    }
                }
            }
        }
    }
    public void setUncle(CellScript obj, int j)
    {
        for (int i = 0; i < 5; i++) 
        {
            if(unclesId[i]==-1)
            {
                unclesId[i] = j;
                uncles[i] = obj;
                break;
            }
        }
    }
    string getName(int i)
    {
        string s = "";
        if (i == 0)
        {
            s = "HS" + getNum3(JsonEdit.cellCharacterId[0]);
            JsonEdit.cellCharacterId[0]++;
        }
        else if (i == 1)
        {
            s = "VS" + getNum3(JsonEdit.cellCharacterId[1]);
            JsonEdit.cellCharacterId[1]++;
        }
        else if (i == 2)
        {
            s = "PJ" + getNum3(JsonEdit.cellCharacterId[2]);
            JsonEdit.cellCharacterId[2]++;
        }
        else if (i == 3)
        {
            s = "PC" + getNum3(JsonEdit.cellCharacterId[3])+"BS";
        }
        else if (i == 4)
        {
            s = "LE" + getNum3(JsonEdit.cellCharacterId[4]);
            JsonEdit.cellCharacterId[4]++;
        }
        else if (i == 30)
        {
            s = "PC" + getNum3(JsonEdit.cellCharacterId[3]);
        }
        else if (i == 31)
        {
            s = "PC" + getNum3(JsonEdit.cellCharacterId[3]) + "B";
        }
        else if (i == 32)
        {
            s = "PC" + getNum3(JsonEdit.cellCharacterId[3]) + "A";
            JsonEdit.cellCharacterId[3]++;
        }
        return s;
    }
    string getNum3(int i)
    {
        if (i < 10)
            return "00" + i;
        else if (i < 100)
            return "0" + i;
        else
            return "" + i;
    }
}
