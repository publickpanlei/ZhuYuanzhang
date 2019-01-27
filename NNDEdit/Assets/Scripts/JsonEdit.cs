using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using UnityEngine.UI;



public class JsonEdit : MonoBehaviour
{

    public static List<GameObject> obj_cells = new List<GameObject>();
    string jsonDirectory;
    string jsonFullName;
    JsonData jsonData;
    public static JsonData jsonCells;
    public GameObject cellObj;
    public GameObject cellBranchObj;
    public RectTransform canvas;
    public RectTransform content;
    public Transform cellRoot;
    public InputField inputFileName;

    public static int cellIdNew = 0;
    public static int[] cellCharacterId = new int[]{2,1,1,1,1};
    public static float layerHeight = 0;
    public static float layerWeight = 0;
    public CellScript wantLinkSon;
    public int wantLinkSonId = -1;//将要连线的cell编号
    public CellScript wantCreateSon;
    public int wantCreateSonId = -1;//将要新建子节点的cell的编号
    public CellScript wantFindNephew;
    public int wantFindNephewId = -1;//将要找侄子的cell的编号
    public CountScript countScript;
    public static int cellLoadNum = 0;//读取的总数
    public static int cellJsonCount = 0;//json内cell的数量
    private TaostScript taostScript;
    public static float lineLengthX = 0.75f;//线长度系数

    // Use this for initialization
    void Start()
    {
        Debug.Log("canvas.localScale.x " + canvas.localScale.x);


#if UNITY_STANDALONE_WIN
        Debug.Log("WINDOWS");
        JsonEdit.lineLengthX = 1 / canvas.localScale.x;
#endif  
#if UNITY_STANDALONE_OSX
        Debug.Log("OSX");
        JsonEdit.lineLengthX = 0.631579f;
#endif
        taostScript = GameObject.Find("TextToast").GetComponent<TaostScript>();
        taostScript.SetTaost("");
        string dpath = Application.dataPath;
        int num = dpath.LastIndexOf("/");
        dpath = dpath.Substring(0, num);
        jsonDirectory = dpath + "/Save";
        if (!Directory.Exists(jsonDirectory))
        {
            Directory.CreateDirectory(jsonDirectory);
        }
        taostScript.SetTaost("存档路径：" + jsonDirectory);
        //        ReadJson("save_0");
        //屏幕分辨率
//         Resolution[] resolutions = Screen.resolutions;
//         foreach (Resolution res in resolutions) {
//             print(res.width + "x" + res.height);
//         }
//         Screen.SetResolution(resolutions[0].width, resolutions[0].height, true);
        float width;
        float height;
        float leftBorder;
        float rightBorder;
        float topBorder;
        float downBorder;
        //the up right corner
        Vector3 cornerPos=Camera.main.ViewportToWorldPoint(new Vector3(1f,1f,Mathf.Abs(Camera.main.transform.position.z)));

        leftBorder=Camera.main.transform.position.x-(cornerPos.x-Camera.main.transform.position.x);
        rightBorder=cornerPos.x;
        topBorder=cornerPos.y;
        downBorder=Camera.main.transform.position.y-(cornerPos.y-Camera.main.transform.position.y);

        width=rightBorder-leftBorder;
        height=topBorder-downBorder;
        Debug.Log("width " + width);
        Debug.Log("height " + height);
    }
    public void CreateRootCell()
    {
        GameObject obj = Instantiate(cellObj, cellRoot);
        //   obj.GetComponent<CellScript>().SetCell(0, -1, "", "", "", 0, -1, -1, 60, 60);
        obj.GetComponent<CellScript>().SetCell(jsonCells[0]);
        obj.transform.localPosition = new Vector2(50, 0);
        obj_cells.Add(obj);
        obj.GetComponent<MyDrag>().SetCanDrag(false);//根节点不能移动
        cellIdNew++;
    }
    public void LoadRootCell()
    {
        JsonEdit.cellJsonCount = jsonCells.Count;
        GameObject obj = Instantiate(cellObj, cellRoot);
        obj.GetComponent<CellScript>().SetCell(jsonCells[0]);
        //     obj.transform.localPosition = new Vector2(0, 0);
        obj_cells.Add(obj);
        obj.GetComponent<CellScript>().LoadSonWait();
        obj.GetComponent<MyDrag>().SetCanDrag(false);//根节点不能移动       
    //    Debug.Log("jsonCells.Count " + jsonCells.Count);
        
        for (int i = 0; i < jsonCells.Count; i++)//独立节点
        {
            if (jsonCells[i]["isGirl"] + "" == 2 + "")
            {
                string s = JsonEdit.jsonCells[i]["type"] + "";
                int j = -1;
                int.TryParse(s, out j);
                if (j <= 2 || j == 4)
                {
                    GameObject obj2 = Instantiate(cellObj, cellRoot);
                    obj2.GetComponent<CellScript>().SetCell(jsonCells[i]);
                    obj_cells.Add(obj2);
                    obj2.GetComponent<CellScript>().LoadSonWait();
                }
                else if(j==3)
                {
                    GameObject obj2 = Instantiate(cellBranchObj, cellRoot);
                    obj2.GetComponent<CellScript>().SetCell(jsonCells[i]);
                    obj_cells.Add(obj2);
                    obj2.GetComponent<CellScript>().LoadBranchSon(obj2.GetComponent<CellScript>().cellLeftId, i + 1);
                }
            }
        }
    }
    public void LoadNephew()
    {
        foreach (GameObject obj3 in obj_cells)
        {
            CellScript cellScript3 = obj3.GetComponent<CellScript>();
            if (cellScript3.isUncle == 1)
            {
                foreach (GameObject obj4 in obj_cells)
                {
                    CellScript cellScript4 = obj4.GetComponent<CellScript>();
                    if (cellScript3.cellLeftId == cellScript4.cellId)
                    {
                        cellScript3.leftSon = obj4;
                        cellScript3.nephewPoint = cellScript4.transform;
                        cellScript3.setUnclesLine();
                        cellScript4.setUncle(cellScript3, cellScript3.cellId);
                        break;
                    }
                }
            }
        }
    }
    public void CreateJson()
    {
        if (inputFileName.text == "")
        {
            Debug.Log("请输入存档名字");
            return;
        }
        ContentEdit.distance = 1;
        jsonFullName = jsonDirectory + "/" + inputFileName.text + ".json";
        if (!File.Exists(jsonFullName))
        {
            //创建一个新的
            jsonData = new JsonData();
            jsonData["cellIdNew"] = 1;
            jsonData["contentDis"] = 1;
            jsonData["contentPosX"] = 0;
            jsonData["contentPosY"] = 0;
            jsonData["contentWidth"] = 1580;
            jsonData["contentHeight"] = 830;
            jsonData["hsId"] = 2;
            jsonData["vsId"] = 1;
            jsonData["pjId"] = 1;
            jsonData["pcId"] = 1;
            jsonData["leId"] = 1;
            jsonData["cells"] = new JsonData();
            jsonCells = jsonData["cells"];

            JsonData newJson = new JsonData();
            newJson["id"] = 0;
            newJson["cellName"] = "HS001";
            newJson["cellText"] = "";
            newJson["leftText"] = "";
            newJson["rightText"] = "";
            newJson["leftId"] = -1;
            newJson["rightId"] = -1;
            newJson["type"] = 0;
            newJson["isGirl"] = -1;
            newJson["isUncle"] = 0;
            newJson["fatherId"] = -1;
            newJson["cellPosX"] = 50;
            newJson["cellPosY"] = 0;
            jsonCells.Add(newJson);

            //找到当前路径
            FileInfo file = new FileInfo(jsonFullName);
            //判断有没有文件，有则打开文件，，没有创建后打开文件
            StreamWriter sw = file.CreateText();
            //ToJson接口将你的列表类传进去，，并自动转换为string类型
            string json = JsonMapper.ToJson(jsonData);
            //将转换好的字符串存进文件，
            sw.WriteLine(json);
            //注意释放资源
            sw.Close();
            sw.Dispose();

            wantCreateSon = null;
            wantCreateSonId = -1;
            wantLinkSon = null;
            wantLinkSonId = -1;
            wantFindNephew = null;
            wantFindNephewId = -1;
            countScript.clearCount();

            Debug.Log("存档文件创建完毕");
            taostScript.SetTaost("存档文件创建完毕");
            //创建初始cell
            foreach (GameObject old in JsonEdit.obj_cells)
            {
                old.SetActive(false);
                Destroy(old, 1);
            }
            obj_cells.Clear();
            JsonEdit.cellIdNew = 0;
            JsonEdit.layerHeight = 0;
            JsonEdit.layerWeight = 0;
            content.sizeDelta = new Vector2(1580, 830);
            JsonEdit.cellLoadNum=-1;
            CreateRootCell();
        }
        else
        {
            Debug.Log("存档文件已经存在");
            taostScript.SetTaost("存档文件已经存在");
        }
    }
    public void SaveJson()
    {
        if (inputFileName.text == "")
        {
            Debug.Log("请输入存档名字");
            taostScript.SetTaost("请输入存档名字");
            return;
        }
        jsonFullName = jsonDirectory + "/" + inputFileName.text + ".json";
        if (File.Exists(jsonFullName))
        {
            Debug.Log("存档文件存在，开始保存");
            taostScript.SetTaost("存档文件存在，开始保存");
            CleanJson();//先清空
        }
        else
        {
            Debug.Log("存档文件不存在，开始新建并保存");
            taostScript.SetTaost("存档文件不存在，开始新建并保存");
        }

        //创建一个新的       
        jsonData["cellIdNew"] = cellIdNew;
        jsonData["contentDis"] = ContentEdit.distance;
        jsonData["contentPosX"] = content.transform.position.x;
        jsonData["contentPosY"] = content.transform.position.y;
        jsonData["contentWidth"] = content.sizeDelta.x;
        jsonData["contentHeight"] = content.sizeDelta.y;
        jsonData["hsId"] = cellCharacterId[0];
        jsonData["vsId"] = cellCharacterId[1];
        jsonData["pjId"] = cellCharacterId[2];
        jsonData["pcId"] = cellCharacterId[3];
        jsonData["leId"] = cellCharacterId[4];
        jsonData["cells"] = new JsonData();
        jsonCells = jsonData["cells"];
        foreach (GameObject obj in obj_cells)
        {
            CellScript cellScript = obj.GetComponent<CellScript>();

            JsonData newJson = new JsonData();
            newJson["id"] = cellScript.cellId;
            newJson["cellName"] = cellScript.cellName;
            newJson["cellText"] = cellScript.cellText;
            newJson["leftText"] = cellScript.leftText;
            newJson["rightText"] = cellScript.rightText;
            newJson["leftId"] = cellScript.cellLeftId;
            newJson["rightId"] = cellScript.cellRightId;
            newJson["type"] = cellScript.cellType;
            Debug.Log("cellScript.isGirl " + cellScript.isGirl);
            newJson["isGirl"] = cellScript.isGirl;
            newJson["isUncle"] = cellScript.isUncle;
            newJson["fatherId"] = cellScript.cellFatherId;
            cellScript.cellPosX = cellScript.transform.position.x; // ContentEdit.distance - content.position.x);
            cellScript.cellPosY = cellScript.transform.position.y;// ContentEdit.distance - content.position.y);
            //             Debug.Log("cellScript.transform.position.y " + cellScript.transform.position.y);
            //             Debug.Log("ContentEdit.distance " + ContentEdit.distance);
            //             Debug.Log("content.position.y " + content.position.y);
            newJson["cellPosX"] = cellScript.cellPosX;// ContentEdit.distance;
            newJson["cellPosY"] = cellScript.cellPosY;// ContentEdit.distance;
            jsonCells.Add(newJson);
        }

        //找到当前路径
        FileInfo file = new FileInfo(jsonFullName);
        //判断有没有文件，有则打开文件，没有创建后打开文件
        StreamWriter sw = file.CreateText();
        //ToJson接口将你的列表类传进去，并自动转换为string类型
        string json = JsonMapper.ToJson(jsonData);
        //将转换好的字符串存进文件，
        sw.WriteLine(json);
        //注意释放资源
        sw.Close();
        sw.Dispose();

        Debug.Log("存档文件保存完毕");

    }
    public void LoadJson()
    {
        if (inputFileName.text == "")
        {
            Debug.Log("请输入存档名字");
            taostScript.SetTaost("请输入存档名字");
            return;
        }
        jsonFullName = jsonDirectory + "/" + inputFileName.text + ".json";
        if (File.Exists(jsonFullName))
        {
            Debug.Log("存档文件存在，开始读取");
            taostScript.SetTaost("存档文件存在，开始读取");
            foreach (GameObject old in obj_cells)//先清空
            {
                old.SetActive(false);
                Destroy(old, 1);
            }
            obj_cells.Clear();
            JsonEdit.layerHeight = 0;
            JsonEdit.layerWeight = 0;
            JsonEdit.cellLoadNum = 0;
            content.sizeDelta = new Vector2(1580, 830);

            StreamReader sr = new StreamReader(jsonFullName);
            string str = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();

            wantCreateSon = null;
            wantCreateSonId = -1;
            wantLinkSon = null;
            wantLinkSonId = -1;
            wantFindNephew = null;
            wantFindNephewId = -1;
            countScript.clearCount();

            jsonData = JsonMapper.ToObject(str);
            cellIdNew = int.Parse(jsonData["cellIdNew"] + "");
            ContentEdit.distance = float.Parse(jsonData["contentDis"] + "");
            content.localScale = new Vector2(ContentEdit.distance, ContentEdit.distance);
            content.position = new Vector2(float.Parse(jsonData["contentPosX"] + ""), float.Parse(jsonData["contentPosY"] + ""));
            content.sizeDelta = new Vector2(float.Parse(jsonData["contentWidth"] + ""), float.Parse(jsonData["contentHeight"] + ""));
            jsonCells = jsonData["cells"];
            LoadRootCell();
            cellCharacterId[0] = int.Parse(jsonData["hsId"] + "");
            cellCharacterId[1] = int.Parse(jsonData["vsId"] + "");
            cellCharacterId[2] = int.Parse(jsonData["pjId"] + "");
            cellCharacterId[3] = int.Parse(jsonData["pcId"] + "");
            cellCharacterId[4] = int.Parse(jsonData["leId"] + "");

            Debug.Log("读取完毕 cellIdNew " + cellIdNew);
        }
        else
        {
            Debug.Log("存档文件不存在");
            taostScript.SetTaost("存档文件不存在");
        }
    }
    public void CleanJson()
    {
        jsonData.Clear();
        Debug.Log("清空存档文件");

    }
    void Update()
    {

    }
    public void ReadJson(string jsonName)
    {

        if (File.Exists(jsonFullName))
        {
            Debug.Log("存档文件存在，开始读取");
            StreamReader sr = new StreamReader(jsonFullName);
            string str = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            JsonData jd = JsonMapper.ToObject(str);
            jsonCells = jd["cell"];
            /*
            for (int i = 0; i < jsonCells.Count; i++)
            {
                string jsonDs = jsonDs +
                    "id=" + jsonCells[i]["id"] + "\n" +
                    "type=" + jsonCells[i]["type"] + "\n" +
                    "photoId=" + jsonCells[i]["photoId"] + "\n" +
                    "cellName=" + jsonCells[i]["cellName"] + "\n" +
                    "cellTalk=" + jsonCells[i]["cellTalk"] + "\n" +
                    "cellText=" + jsonCells[i]["cellText"] + "\n" +
                    "year=" + jsonCells[i]["year"] + "\n" +
                    "age=" + jsonCells[i]["age"] + "\n" +
                    "leftText=" + jsonCells[i]["leftText"] + "\n" +
                    "rightText=" + jsonCells[i]["rightText"] + "\n" +
                    "leftId=" + jsonCells[i]["leftId"] + "\n" +
                    "rightId=" + jsonCells[i]["rightId"] + "\n" +
                    "fatherId=" + jsonCells[i]["fatherId"] + "\n" +
                    "posX=" + jsonCells[i]["posX"] + "\n" +
                    "posY=" + jsonCells[i]["posY"] + "\n";
            }
            //修改
            jsonCells[0]["type"] = "AAA";
            jsonCells[0]["cellName"] = "小乞丐";
            //增加一个节点
            JsonData newJson = new JsonData();
            newJson["id"] = "5";
            newJson["type"] = "newType";
            newJson["photoId"] = "5";
            newJson["cellName"] = "地主";
            newJson["cellTalk"] = "小乞丐快滚开！";
            jsonCells.Add(newJson);
            //找到当前路径
            FileInfo file = new FileInfo(jsonDirectory + "/" + jsonName + "2.json");
            //判断有没有文件，有则打开文件，，没有创建后打开文件
            StreamWriter sw = file.CreateText();
            //ToJson接口将你的列表类传进去，，并自动转换为string类型
            string json = JsonMapper.ToJson(jd);
            //将转换好的字符串存进文件，
            sw.WriteLine(json);
            //注意释放资源
            sw.Close();
            sw.Dispose();
            */
        }
        else
        {
            Debug.Log("存档文件不存在");
        }
    }

}
