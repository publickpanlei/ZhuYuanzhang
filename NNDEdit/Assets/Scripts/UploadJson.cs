using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using System;
using UnityEngine.Networking;

public class UploadJson : MonoBehaviour {

    private string FTPHost = "ftp://43.226.153.17/Web/res/raw-assets/resources/save/";
    private string FTPUserName = "ftp6262703";
    private string FTPPassword = "Panlei123";
    private string fileName = "2";
    private string fileName2 = "2.json";
    private string jsonDirectory;
    private string jsonFullName;
    bool canUp = true;

	// Use this for initialization
	void Start () {
        string dpath = Application.dataPath;
        int num = dpath.LastIndexOf("/");
        dpath = dpath.Substring(0, num);
        jsonDirectory = dpath + "/Save";
        if (!Directory.Exists(jsonDirectory))
        {
            Debug.Log("路径不存在");
        }
        else
        {
            jsonFullName = jsonDirectory + "/" + fileName + ".json";
        }
        
	}


    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField(jsonDirectory, fileName2);

        UnityWebRequest www = UnityWebRequest.Post("http://43.226.153.17/Web/res/raw-assets/resources/save/", form);
        
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        
    }
    public void UploadFile()
    {

        StartCoroutine(Upload());


            Debug.Log("开始上传 " + FTPHost);

//             WebClient client = new System.Net.WebClient();
//             Debug.Log("jsonFullName " + jsonFullName);
//             Uri uri = new Uri(FTPHost + fileName + ".json");
//             client.UploadProgressChanged += new UploadProgressChangedEventHandler(OnFileUploadProgressChanged);
//             client.UploadFileCompleted += new UploadFileCompletedEventHandler(OnFileUploadCompleted);
//             client.Credentials = new System.Net.NetworkCredential(FTPUserName, FTPPassword);
//             client.UploadFileAsync(uri, "STOR", jsonFullName);


    }

//     void OnFileUploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
//     {
//         Debug.Log("Uploading Progreess: " + e.ProgressPercentage);
//     }
// 
//     void OnFileUploadCompleted(object sender, UploadFileCompletedEventArgs e)
//     {
//         Debug.Log("文件 " + fileName+".json 上传完毕！");
//         canUp = true;
//     }

	// Update is called once per frame
	void Update () {
		
	}
}
