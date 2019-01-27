using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using UnityEngine.UI;

public class XmlEdit : MonoBehaviour {


    public static List<GameObject> obj_cell = new List<GameObject>();
    public static int cellIdNew = 0;
    public static int layerIdMax = 0;
    public static float layerHalfWeight = 0;
    public GameObject cellObj;
    public RectTransform content;
    public Transform cellRoot;

    string xmlDirectory;
    public static string xmlDirectoryAndName;
    public InputField inputFileName;

   
	// Use this for initialization
	void Start () {
        string dpath = Application.dataPath;
        Debug.Log("dpath " + dpath);
        int num = dpath.LastIndexOf("/");
        Debug.Log("num " + num);
        dpath = dpath.Substring(0, num);
        Debug.Log("dpath " + dpath);
        string directory = dpath + "/Save";
        if(!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
   //     xmlDirectory = directory + "/abc.xml";
        xmlDirectory = directory;
   //     CreateXml();

	}
    public void CreateRootCell()
    {
//         GameObject obj = Instantiate(cellObj, cellRoot);
//         obj.GetComponent<CellScript>().SetCell(0, -1, "", "", 0, -1, -1,60,60);
//         obj.transform.localPosition = new Vector2(0, 0);
//         obj_cell.Add(obj);
//         obj.GetComponent<MyDrag>().enabled = false;//根节点不能移动
//         XmlEdit.cellIdNew++;
    }
    public void LoadRootCell(int _cellId, int _cellMotherId, string _cellName, string _cellText, int _cellImageId, int _cellLeftId, int _cellRightId)
    {
//         GameObject obj = Instantiate(cellObj, cellRoot);
//         obj.GetComponent<CellScript>().SetCell(_cellId, _cellMotherId, _cellName, _cellText, _cellImageId, _cellLeftId, _cellRightId,
//         _lineLengthLeft,_lineLengthRight);
//         obj.transform.localPosition = new Vector2(0, 0);
//         obj_cell.Add(obj);
//         obj.GetComponent<CellScript>().LoadSonWait();
//         obj.GetComponent<MyDrag>().enabled = false;//根节点不能移动
    }
    public void CreateXml()
    {
        if (inputFileName.text=="")
        {
            Debug.Log("请输入存档名字");
            return;
        }
        XmlEdit.xmlDirectoryAndName = xmlDirectory + "/" + inputFileName.text + ".xml";
        if (!File.Exists(XmlEdit.xmlDirectoryAndName))
        {
            // 创建xml文档实例
            XmlDocument xmlDoc = new XmlDocument();
            // 创建根节点
            XmlElement root = xmlDoc.CreateElement("xml");
            // 创建第1个子节点allCell
            XmlElement allCell = xmlDoc.CreateElement("allCell");
            // 设置节点属性
            allCell.SetAttribute("cellIdNew", "1");
            // 创建allCell的子节点
            XmlElement cell = xmlDoc.CreateElement("cell");
            // 设置节点属性
            cell.SetAttribute("id", "0");
            cell.SetAttribute("motherId", "-1");
            cell.SetAttribute("cellName", "");
            cell.SetAttribute("cellText", "");
            cell.SetAttribute("cellImageId", "0");
            cell.SetAttribute("cellLeftId", "-1");
            cell.SetAttribute("cellRightId", "-1");
            // 排序
            allCell.AppendChild(cell);
            root.AppendChild(allCell);
            xmlDoc.AppendChild(root);

            xmlDoc.Save(XmlEdit.xmlDirectoryAndName);
            Debug.Log("存档文件创建完毕");

            //创建初始cell
            foreach (GameObject old in XmlEdit.obj_cell)
            {
                old.SetActive(false);
                Destroy(old, 1);
            }
            obj_cell.Clear();
            XmlEdit.cellIdNew = 0;
            XmlEdit.layerIdMax = 0;
            XmlEdit.layerHalfWeight = 0;
            content.sizeDelta = new Vector2(1580, 830); 
            CreateRootCell();
        }
        else
        {
            Debug.Log("存档文件已经存在");
        }
    }
    public void SaveXml()
    {
        if (inputFileName.text == "")
        {
            Debug.Log("请输入存档名字");
            return;
        }
        XmlEdit.xmlDirectoryAndName = xmlDirectory + "/" + inputFileName.text + ".xml";
        if (File.Exists(XmlEdit.xmlDirectoryAndName))
        {
            Debug.Log("存档文件存在，开始保存");
            CleanXml();//先清空
        }
        else
        {
            Debug.Log("存档文件不存在，开始新建并保存");
        }
        // 创建xml文档实例
        XmlDocument xmlDoc = new XmlDocument();
        // 创建根节点
        XmlElement root = xmlDoc.CreateElement("xml");
        // 创建第1个子节点allCell
        XmlElement allCell = xmlDoc.CreateElement("allCell");
        // 设置节点属性
        allCell.SetAttribute("cellIdNew", XmlEdit.cellIdNew + "");

        foreach (GameObject obj in XmlEdit.obj_cell)
        {
            CellScript cellScript = obj.GetComponent<CellScript>();

            // 创建allCell的子节点
            XmlElement cell = xmlDoc.CreateElement("cell");
            // 设置节点属性
            cell.SetAttribute("id", cellScript.cellId+"");
            cell.SetAttribute("motherId", cellScript.cellFatherId + "");
            cell.SetAttribute("cellName", cellScript.cellName);
            cell.SetAttribute("cellText", cellScript.cellText);
            cell.SetAttribute("cellLeftId", cellScript.cellLeftId + "");
            cell.SetAttribute("cellRightId", cellScript.cellRightId + "");
            // 排序
            allCell.AppendChild(cell);
        }

        root.AppendChild(allCell);
        xmlDoc.AppendChild(root);

        xmlDoc.Save(XmlEdit.xmlDirectoryAndName);
        Debug.Log("存档文件保存完毕");

    }
    public void LoadCell()
    {
        if (inputFileName.text == "")
        {
            Debug.Log("请输入存档名字");
            return;
        }
        XmlEdit.xmlDirectoryAndName = xmlDirectory + "/" + inputFileName.text + ".xml";
        if (File.Exists(XmlEdit.xmlDirectoryAndName))
        {
            Debug.Log("存档文件存在，开始读取");
            foreach (GameObject old in XmlEdit.obj_cell)//先清空
            {
                old.SetActive(false);
                Destroy(old, 1);
            }
            obj_cell.Clear();
            XmlEdit.layerIdMax = 0;
            XmlEdit.layerHalfWeight = 0;
            content.sizeDelta = new Vector2(1580, 830); 

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(XmlEdit.xmlDirectoryAndName);
            XmlNodeList nodes = xmlDoc.SelectSingleNode("xml").ChildNodes;
            foreach (XmlElement allCell in nodes)
            {
                XmlEdit.cellIdNew = GetXmlData(allCell, "cellIdNew", 1);
                foreach (XmlElement cell in allCell)
                {
                    if(cell.GetAttribute("id")=="0")
                    {
                        LoadRootCell(0,
                                    -1,
                                    GetXmlData(cell,"cellName",""),
                                    GetXmlData(cell, "cellText", ""),
                                    GetXmlData(cell, "cellImageId", 0),
                                    GetXmlData(cell, "cellLeftId", -1),
                                    GetXmlData(cell, "cellRightId", -1)
                                    );
                        break;
                    }
                }
            }
            Debug.Log("读取完毕 cellIdNew " + cellIdNew);
        }
        else
        {
            Debug.Log("存档文件不存在");
        }
    }
    public void CleanXml()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(XmlEdit.xmlDirectoryAndName);
        XmlNodeList nodeList = xmlDoc.SelectSingleNode("xml").ChildNodes;

        foreach (XmlElement allCell in nodeList)
        {
            allCell.RemoveAll();
        }
        xmlDoc.Save(XmlEdit.xmlDirectoryAndName);
        Debug.Log("清空存档文件");
    }
    public void Write(string s)
    {

    }
	// Update is called once per frame
	void Update () {

	}

    private string GetXmlData(XmlElement obj,string s_name,string s_default)
    {
        string s = obj.GetAttribute(s_name);
        if(s!="")
        {
            return s;
        }        
        return s_default;
    }
    private int GetXmlData(XmlElement obj, string s_name, int s_default)
    {
        string s = obj.GetAttribute(s_name);
        if (s != "")
        {
            return int.Parse(s);
        } 
        return s_default;
    }
}
