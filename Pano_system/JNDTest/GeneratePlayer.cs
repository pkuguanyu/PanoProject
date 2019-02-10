using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.UI;

using UnityEngine.Video;
public class GeneratePlayer : MonoBehaviour {

	public GameObject mPrefab;


	private int CenterTillingNumber=0;
	private Vector3[] TillPosition; 
	private float gap_Width,gap_Height;
	private GameObject []Till;
	private Ray ray;
	private RaycastHit rayinfo; 
	private GameObject mainCamera;
	private Camera theCamera;
	private Vector3 eular;
	private GameObject baselayer;
	private GameObject centertill;
	private float U;
	private float V;
	private Vector3[] crossPoint;
	private Vector3 InitialVector = new Vector3 (0.5f, 0f, 0f);
	private Vector3[] viewPort;
	private int []Qp_Tile;
	private Vector2[] viewPoint;
	private Vector3 viewPoint3D;
	private int width=6;
	private int height=12;
	private int T = 0;
    public VideoClip v;
    public VideoClip v1;
    public VideoClip v2;
    //先使用方


    //距离摄像机8.5米 用黄色表示
    public float upperDistance = 8.5f;
	//距离摄像机12米 用红色表示
	public float lowerDistance = 1f;
	private Transform tx;


	Vector3 [] GenerateRect(int width,int height)
	{
		int i, j, k;
		Vector3 []temp = new Vector3[width*height];
		for (i = 0; i <height; i++) {
			for (j = 0; j < width; j++) {
				temp [i*width + j] = new Vector3((i * gap_Height+(i+1)* gap_Height)/2-(height/2)* gap_Height, (j * gap_Width+(j+1)*gap_Width)/2-(width/2)*gap_Width,-0.1f);
			}
		}
		return temp;
	}

	// Use this for initialization
	void Start () {
		int i, j, k;
		if ( !theCamera )
		{
			theCamera = Camera.main;
		}
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        tx = mainCamera.transform;
		crossPoint = new Vector3[4];
		viewPort = new Vector3[4];
	
		baselayer = GameObject.FindGameObjectWithTag ("BaseLayer");
		//centertill = GameObject.FindGameObjectWithTag ("CenterTill");
		baselayer.transform.localPosition = new Vector3 (0f, 0f, 0f);
		baselayer.transform.localScale = new Vector3 (12f, 6f, 1f);

		gap_Width = 1;
		gap_Height = 1;
		CenterTillingNumber = width*height;


        string Set = GameObject.FindGameObjectWithTag("data").GetComponent<Pdata>().set;
        string videoid = GameObject.FindGameObjectWithTag("data").GetComponent<Pdata>().videoid;
        string userid = GameObject.FindGameObjectWithTag("data").GetComponent<Pdata>().userid;
        
        v = Resources.Load<VideoClip>("Ada_Real_System/" + Set +"/"+videoid+"/"+userid+".mp4");
        v1 = Resources.Load<VideoClip>("Ada_Real_System_WithoutVR/" + Set + "/" + videoid + "/" + userid + ".mp4");
        v2 = Resources.Load<VideoClip>("QuanHigh_Real_System/" +Set+ "/" + videoid + "/" + userid + ".mp4");
        /*Till = new GameObject[width*height];
		Qp_Tile = new int[CenterTillingNumber];
		TillPosition = GenerateRect (width,height);
		for (i = 0; i <  height; i++) {
			for (j = 0; j < width; j++) {
				GameObject objPrefab = (MonoBehaviour.Instantiate(mPrefab, Vector3.zero, Quaternion.identity) as GameObject);
				objPrefab.transform.localPosition = TillPosition [i*width + j];
				objPrefab.transform.parent = centertill.transform;
				Till [i*width + j] = objPrefab;
			}
		}
        */
        ReadviewPoint(Set + "/" + videoid + "/" + userid);
		StartCoroutine (TimeEndToEnd ());

	}

	// Update is called once per frame
	void Update () {
		//ray = new Ray (mainCamera.transform.position+mainCamera.transform.forward*10, -mainCamera.transform.forward*10);
		//Debug.DrawRay (mainCamera.transform.position+mainCamera.transform.forward*10, -mainCamera.transform.forward*10, Color.red);
		//centertill.transform.localPosition = new Vector3 (-1f * U, 1f * V, 0f);
		//FindLowerCorners();//找到camera四个角
		//FindCenterTill ();//找到中心tile
		//UpdateTill ();   //更新tileQp
		EndToEndTest();
	}

    public void inT()
    {
        int i, j, k;
        T = 0;
        float X = viewPoint[T].x;
        float Y = viewPoint[T].y;
        X = X / 240;
        Y = Y / 240;
        X = 9 - X;
        Y = 3 - Y;//normalization

        if (X >= 0 && X < 6)
        {
            X = X / 6 * 180;
        }
        else if (X > 6 && X < 9)
        {
            X = (X - 12) / 6 * 180;
        }
        else if (X < 0 && X > -3)
        {
            X = (X / 6) * 180;
        }
        if (Y >= 0 && Y <= 3)
        {
            Y = Y / 6 * 180 * -1;
        }
        else if (Y < 0 && Y > -3)
        {
            Y = Y / 6 * 180 * -1;
        }
        viewPoint3D.x = Y;
        viewPoint3D.y = X;
        viewPoint3D.z = 0;  //当前应该旋转到的点
        Debug.Log(viewPoint3D.ToString());
        T = T + 1;
    }

    IEnumerator TimeEndToEnd()
	{
        T = 0;
        inT();
        GameObject.FindGameObjectWithTag("ShowTag").GetComponent<TextMesh>().text = "接下来您将看到三个\n不同质量的视频片段\n请您给出打分(1-5分)";  
        yield return new WaitForSecondsRealtime(10f);
        GameObject.FindGameObjectWithTag("ShowTag").GetComponent<TextMesh>().text = "";
        GameObject.FindGameObjectWithTag("BaseLayer").GetComponent<VideoPlayer>().clip = v;
        GameObject.FindGameObjectWithTag("BaseLayer").GetComponent<VideoPlayer>().Play();
        while (T < viewPoint.Length) {
			int i, j, k;
			float X = viewPoint [T].x;
			float Y = viewPoint [T].y;
			X = X / 240;
			Y = Y / 240;
			X = 9 - X;
			Y = 3 - Y;//normalization

			if (X >= 0 && X < 6) {
				X = X / 6 * 180;
			} else if (X > 6 && X < 9) {
				X = (X - 12) / 6 * 180;
			} else if (X < 0 && X > -3) {
				X = (X / 6) * 180;
			}
			if (Y >= 0 && Y <= 3) {
				Y = Y / 6 * 180 * -1;
			} else if (Y < 0 && Y > -3) {
				Y = Y / 6 * 180 * -1;
			}
			viewPoint3D .x = Y;
			viewPoint3D .y = X;
			viewPoint3D .z = 0;  //当前应该旋转到的点
			Debug.Log(viewPoint3D.ToString());
			yield return new WaitForSecondsRealtime (1f/3);
			T = T + 1;
		}
        GameObject.FindGameObjectWithTag("BaseLayer").GetComponent<VideoPlayer>().clip = v1;
        GameObject.FindGameObjectWithTag("BaseLayer").GetComponent<VideoPlayer>().Play();
        T = 0;
        while (T < viewPoint.Length)
        {
            int i, j, k;
            float X = viewPoint[T].x;
            float Y = viewPoint[T].y;
            X = X / 240;
            Y = Y / 240;
            X = 9 - X;
            Y = 3 - Y;//normalization

            if (X >= 0 && X < 6)
            {
                X = X / 6 * 180;
            }
            else if (X > 6 && X < 9)
            {
                X = (X - 12) / 6 * 180;
            }
            else if (X < 0 && X > -3)
            {
                X = (X / 6) * 180;
            }
            if (Y >= 0 && Y <= 3)
            {
                Y = Y / 6 * 180 * -1;
            }
            else if (Y < 0 && Y > -3)
            {
                Y = Y / 6 * 180 * -1;
            }
            viewPoint3D.x = Y;
            viewPoint3D.y = X;
            viewPoint3D.z = 0;  //当前应该旋转到的点
            Debug.Log(viewPoint3D.ToString());
            yield return new WaitForSecondsRealtime(1f / 3);
            T = T + 1;
        }
        GameObject.FindGameObjectWithTag("BaseLayer").GetComponent<VideoPlayer>().clip = v2;
        GameObject.FindGameObjectWithTag("BaseLayer").GetComponent<VideoPlayer>().Play();
        T = 0;
        while (T < viewPoint.Length)
        {
            int i, j, k;
            float X = viewPoint[T].x;
            float Y = viewPoint[T].y;
            X = X / 240;
            Y = Y / 240;
            X = 9 - X;
            Y = 3 - Y;//normalization

            if (X >= 0 && X < 6)
            {
                X = X / 6 * 180;
            }
            else if (X > 6 && X < 9)
            {
                X = (X - 12) / 6 * 180;
            }
            else if (X < 0 && X > -3)
            {
                X = (X / 6) * 180;
            }
            if (Y >= 0 && Y <= 3)
            {
                Y = Y / 6 * 180 * -1;
            }
            else if (Y < 0 && Y > -3)
            {
                Y = Y / 6 * 180 * -1;
            }
            viewPoint3D.x = Y;
            viewPoint3D.y = X;
            viewPoint3D.z = 0;  //当前应该旋转到的点
            Debug.Log(viewPoint3D.ToString());
            yield return new WaitForSecondsRealtime(1f / 3);
            T = T + 1;
          
        }
        GameObject.FindGameObjectWithTag("ShowTag").GetComponent<TextMesh>().text = "请您对3个片段给出打分（1-5分）";
    }
	void EndToEndTest()
	{
		//tx.localEulerAngles = viewPoint3D;
		//tx.localRotation = Quaternion.Euler (viewPoint3D);
		tx.localRotation = Quaternion.Slerp(tx.localRotation, Quaternion.Euler (viewPoint3D) ,1f / 3 * Time.deltaTime);  //平滑rotation
	}

	float CalAngle_Y(float x)  //initial vector (0f,-90f,0f)
	{
		//get x
		if (x >= 270f) {
			return (-x + 360f) / 180f;
		}else
			return -x/180f;
	}

	float CalAngle_X(float y)  //initial vector (0f,-90f,0f)
	{
		//get x
		if (y >= -90f && y <= 90.0f) {
			return (y - 90f)/360f;
		} 
		if (y >= 90f && y <= 180f) {
			return (y - 90f)/360f;
		}
		if (y >= -180f && y <= -90f) {
			return (y + 270f)/360f; 
		}
		return 0f;

	}

	void UpdateTill()
	{
		int i, j, k;
		for (i = 0; i < CenterTillingNumber; i++) {
			if (Qp_Tile[i]<=27)
				Till [i].GetComponent<MeshRenderer> ().material.color = Color.red;
			else
				Till [i].GetComponent<MeshRenderer> ().material.color = Color.white;
		}
	}

	void FindCenterTill()
	{
		int i, j, k;
		for (i = 0; i < CenterTillingNumber; i++) {
					Qp_Tile [i] = 42;
		}
		for (i = 0; i < CenterTillingNumber; i++) {
			Vector3 temp = TillPosition [i];
			//当正常
			if (viewPort[3].x<=temp.x+0.5 && temp.x-0.5<=viewPort[0].x) {
				if (viewPort[3].y<=temp.y+0.5 && viewPort[0].y>=temp.y-0.5) {
					Qp_Tile [i] = 22;
				}
			}
			//左右
			if (viewPort [3].x - viewPort [0].x>= 6) {
				if (viewPort[0].x>=temp.x-0.5  || temp.x+0.5>=viewPort[3].x) {
					if (viewPort[3].y<=temp.y+0.5 && viewPort[0].y>=temp.y-0.5) {
						Qp_Tile [i] = 22;
					}
				}
			}
			if (viewPort [0].x - viewPort [3].x>= 6) {
				if (viewPort[3].x>=temp.x-0.5  || temp.x+0.5>=viewPort[0].x) {
					if (viewPort[3].y<=temp.y+0.5 && viewPort[0].y>=temp.y-0.5) {
						Qp_Tile [i] = 22;
					}
				}
			}
			//上下
			if ( Mathf.Abs(viewPort [0].y - viewPort [3].y)<= 1 && viewPort [0].y>0) {
				if (temp.y >= 0) {
					Qp_Tile [i] = 22;
				}
			}

			if ( Mathf.Abs(viewPort [0].y - viewPort [3].y)<= 1 && viewPort [0].y<0) {
				if (temp.y <= 0) {
					Qp_Tile [i] = 22;
				}
			}
			//当左右越界
		}
	}


		void  FindLowerCorners (){
			Vector3[] corners = GetCorners( 0.5f );

			for (int i = 0; i < 4; i++) 
			{
				Ray ray = new Ray (corners [i], theCamera.transform.position - corners [i]);
				RaycastHit rayinfo; 
				if (Physics.Raycast (ray, out rayinfo)) {
				crossPoint [i] = rayinfo.point;
				Debug.DrawRay (crossPoint[i], (theCamera.transform.position-crossPoint[i]) ,Color.red);
				crossPoint [i].x = crossPoint [i].x + 20;
				crossPoint [i].y = crossPoint [i].y + 20;
				Vector2 temp1 = new Vector2 (crossPoint [i].x, crossPoint [i].z);
				Vector2 temp2 = new Vector2 (1,0);
				float angle_X,angle_Y;
				if (crossPoint [i].z > 0) {
					angle_X = Mathf.Acos (Vector2.Dot (temp1.normalized, temp2.normalized)) * Mathf.Rad2Deg;
				} else {
					angle_X = -Mathf.Acos (Vector2.Dot (temp1.normalized, temp2.normalized)) * Mathf.Rad2Deg;
				}

				temp1 = new Vector3 (crossPoint [i].x+20f, crossPoint [i].y+20f, crossPoint [i].z);
				temp2 = new Vector3 (0, 1,0);
				float theta = Mathf.Acos (Vector3.Dot (temp1.normalized, temp2.normalized)) * Mathf.Rad2Deg;
				if (crossPoint [i].y > 0) {
					angle_Y = -theta +90f;
				} else {
					angle_Y = theta -90f;
				}

				float U = angle_X/360;
				float V = angle_Y/180;
				Debug.DrawRay (new Vector3 (12f * U, 6f * V, 0f), -centertill.transform.forward * 10,Color.red);
				viewPort [i] = new Vector3 (12f * U, 6f * V, 0f);
				//Debug.Log (i + " " + corners [i].ToString ("f5") + " " + V + " " + crossPoint [i].ToString ("f5"));
				//Debug.Log (V);
				//Debug.Log ( angle_Y);
				}
			}
		      
			// for debugging
			Debug.DrawLine( corners[0], corners[1], Color.red );
			Debug.DrawLine( corners[1], corners[3], Color.red );
			Debug.DrawLine( corners[3], corners[2], Color.red );
			Debug.DrawLine( corners[2], corners[0], Color.red );
		}


		Vector3[] GetCorners (  float distance   ){
			Vector3[] corners = new Vector3[ 4 ];

			float halfFOV = ( theCamera.fieldOfView * 0.5f ) * Mathf.Deg2Rad;
			float aspect = theCamera.aspect;

			float height = distance * Mathf.Tan( halfFOV );
			float width = height * aspect;

			// UpperLeft
			corners[ 0 ] = tx.position - ( tx.right * width );
			corners[ 0 ] += tx.up * height;
			corners[ 0 ] += tx.forward * distance;

			// UpperRight
			corners[ 1 ] = tx.position + ( tx.right * width );
			corners[ 1 ] += tx.up * height;
			corners[ 1 ] += tx.forward * distance;

			// LowerLeft
			corners[ 2 ] = tx.position - ( tx.right * width );
			corners[ 2 ] -= tx.up * height;
			corners[ 2 ] += tx.forward * distance;

			// LowerRight
			corners[ 3 ] = tx.position + ( tx.right * width );
			corners[ 3 ] -= tx.up * height;
			corners[ 3 ] += tx.forward * distance;

			return corners;
		}

		void ReadviewPoint(string FileName) {
			TextAsset t =	Resources.Load<TextAsset> (FileName);
			string[] strs = Regex.Split (t.text, "\r\n");//读取文件的所有行，并将数据读取到定义好的字符数组strs中，一行存一个单元
			int i;
			viewPoint = new Vector2[strs.Length-1];
			viewPoint3D= new Vector3(0,90,0);
			Debug.Log (strs.Length);
		    for (i = 0; i < strs.Length-1; i++)
			{
				string[] temp= strs[i].ToString().Split(' ');//读取每一行，并连起来
				viewPoint[i]=new Vector2( float.Parse(temp[1]),float.Parse(temp[2]));
			}
		}


}
