using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DownLoad : MonoBehaviour {

    public string url = "http://localhost:8080/Video/Total.mp4";
    // Use this for initialization
    string path = "";
    HttpDownload download = new HttpDownload();
    DownLoaderEnum down;
    private void Awake()
    {
        path = Application.streamingAssetsPath;
        //DirectoryInfo directory = new DirectoryInfo(path);
        //if (directory.Exists)
        //    directory.Create();
        //Debug.LogError(path);
    }
    // Use this for initialization
    void Start()
    {
        ThreadPool.SetMaxThreads(5, 5);
        down = new DownLoaderEnum(url, path);//请求url，存放地址，存放文件名
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ThreadPool.QueueUserWorkItem(download.HttpDownloader, down);
        }
    }
}
