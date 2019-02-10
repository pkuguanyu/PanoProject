using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SecondEventTrigger : MonoBehaviour {

	float speed=0f;
	float depth=0f;
	Vector3 Original;
	Vector3 Outside=new Vector3(1000f, 1000f, 0f);

	void Start()
	{
		float Distance=GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetDistance ();
		Original = new Vector3 (0f, 0f, Distance);
		GameObject.FindGameObjectWithTag ("ImageDisplay").transform.position = Original;
		GameObject.FindGameObjectWithTag ("ImageDisplay").transform.eulerAngles = new Vector3 (0f, 0f, 0f);
		GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponent<Image> ().color = Color.clear;
		GameObject.FindGameObjectWithTag ("ImageDisplay2").GetComponent<Image> ().color = Color.clear;
		//GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponent<Image> ().GetComponent<RectTransform> ().sizeDelta = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetImageSize ();
	}













	//景深
	//------------------------------------------------------------------------------------------
	public void OnSample1_Third()
	{
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		depth = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetDepth ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		StartCoroutine (Sample_Third (depth, 22, 27));
	}

	public void OnSample2_Third()
	{
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		depth = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetDepth ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		StartCoroutine (Sample_Third (depth, 22, 22));

	}
	public void OnSample3_Third()
	{	
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		depth = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetDepth ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();

		StartCoroutine (Sample_Third (depth, 22, 22));
	}
	public void OnSample4_Third()
	{
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		depth = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetDepth ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		StartCoroutine (Sample_Third (depth, 22, 37));

	}
	public void OnSample5_Third()
	{
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		depth = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetDepth ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		StartCoroutine (Sample_Third (depth, 22, 32));

	}
	public void OnSample6_Third()
	{
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		depth = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetDepth ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		StartCoroutine (Sample_Third (depth, 22, 42));
	}

	IEnumerator Sample_Third(float depth,int qp1,int qp2)
	{
		GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponentInChildren<TextMesh> ().text = "接下来的测试持续10秒左右\n请您给出是否图片发生变化";
		yield return  new WaitForSeconds (5f);
		StartCoroutine ((GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponent<UserStudyThree> ().Show_Depth(depth,qp1,qp2)));

	}
	//------------------------------------------------------------------------------------------


	//景深混合
	//------------------------------------------------------------------------------------------
	public void OnSample1_Bright_Mix()
	{
		float b1 = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().getBrightness ();
		float b2 = 0;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		GameObject.FindGameObjectWithTag ("LeftEye").GetComponent<Camera> ().backgroundColor = new Color ((float)b1/255, (float)b1/255, (float)b1/255);
		GameObject.FindGameObjectWithTag ("RightEye").GetComponent<Camera> ().backgroundColor = new Color ((float)b1/255, (float)b1/255, (float)b1/255);
		StartCoroutine (Sample_Bright_Mix (b1,b2));
	}

	public void OnSample2_Bright_Mix()
	{
		float b1 = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().getBrightness ();
		float b2 = 75;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer ();
		GameObject.FindGameObjectWithTag ("LeftEye").GetComponent<Camera> ().backgroundColor = new Color ((float)b1/255, (float)b1/255, (float)b1/255);
		GameObject.FindGameObjectWithTag ("RightEye").GetComponent<Camera> ().backgroundColor = new Color ((float)b1/255, (float)b1/255, (float)b1/255);
		StartCoroutine (Sample_Bright_Mix (b1,b2));

	}
	public void OnSample3_Bright_Mix()
	{
		float b1 = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().getBrightness ();
		float b2 = 165;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		GameObject.FindGameObjectWithTag ("LeftEye").GetComponent<Camera> ().backgroundColor = new Color ((float)b1/255, (float)b1/255, (float)b1/255);
		GameObject.FindGameObjectWithTag ("RightEye").GetComponent<Camera> ().backgroundColor = new Color ((float)b1/255, (float)b1/255, (float)b1/255);
		StartCoroutine (Sample_Bright_Mix (b1,b2));
	}

	public void OnSample4_Bright_Mix()
	{
		float b1 = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().getBrightness ();
		float b2 = 255;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		GameObject.FindGameObjectWithTag ("LeftEye").GetComponent<Camera> ().backgroundColor = new Color ((float)b1/255, (float)b1/255, (float)b1/255);
		GameObject.FindGameObjectWithTag ("RightEye").GetComponent<Camera> ().backgroundColor = new Color ((float)b1/255, (float)b1/255, (float)b1/255);
		StartCoroutine (Sample_Bright_Mix (b1,b2));
	}


	IEnumerator Sample_Bright_Mix(float b1,float b2)
	{
		GameObject.FindGameObjectWithTag ("3Text").GetComponent<TextMesh> ().text = "请先静止观看前方20秒\n适应当前亮度";
		yield return new WaitForSeconds(20f);
		StartCoroutine ((GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponent<UsetBrightMix> ()).Go(b2,b1));
		yield return null;
	}
	//---------------











	//景深混合
	//------------------------------------------------------------------------------------------
	public void OnSample1_Third_Mix()
	{
		int bg = 120;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		float depth = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetDepth ();
		float depth2 = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetDepth2 ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		GameObject.FindGameObjectWithTag ("LeftEye").GetComponent<Camera> ().backgroundColor = new Color ((float)(bg+30)/255, (float)(bg+30)/255, (float)(bg+30)/255);
		GameObject.FindGameObjectWithTag ("RightEye").GetComponent<Camera> ().backgroundColor = new Color ((float)(bg+30)/255, (float)(bg+30)/255, (float)(bg+30)/255);
		StartCoroutine (Sample_Third_Mix (depth,depth2,bg));
	}




    //修改一个函数就行了
    public void OnSample2_Third_Mix()
	{
		int bg = 120;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		depth = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetDepth ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer ();
		GameObject.FindGameObjectWithTag ("LeftEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		GameObject.FindGameObjectWithTag ("RightEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		StartCoroutine (Sample_Third_Mix (depth,bg));

	}
	public void OnSample3_Third_Mix()
	{
		int bg = 120;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		depth = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetDepth ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		GameObject.FindGameObjectWithTag ("LeftEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		GameObject.FindGameObjectWithTag ("RightEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		StartCoroutine (Sample_Third_Mix (depth,bg));
	}
	public void OnSample4_Third_Mix()
	{
		int bg = 120;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		depth = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetDepth ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		GameObject.FindGameObjectWithTag ("LeftEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		GameObject.FindGameObjectWithTag ("RightEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		StartCoroutine (Sample_Third_Mix (depth,bg));

	}
	public void OnSample5_Third_Mix()
	{
		int bg = 120;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		depth = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetDepth ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		GameObject.FindGameObjectWithTag ("LeftEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		GameObject.FindGameObjectWithTag ("RightEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		StartCoroutine (Sample_Third_Mix (depth,bg));

	}
	public void OnSample6_Third_Mix()
	{
		int bg = 120;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		depth = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetDepth ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		GameObject.FindGameObjectWithTag ("LeftEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		GameObject.FindGameObjectWithTag ("RightEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		StartCoroutine (Sample_Third_Mix (depth,bg));
	}
	IEnumerator Sample_Third_Mix(float depth,int bg)
	{
		StartCoroutine ((GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponent<UsetStudyMix> ()).Go(depth,-1f,bg));
		yield return null;

	}
	IEnumerator Sample_Third_Mix(float depth,float depth2,int bg)
	{
		
		StartCoroutine ((GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponent<UsetStudyMix> ()).Go(depth,depth2,bg));
		yield return null;

	}
    //------------------------------------------------------------------------------------------




    //加红点的景深不跟
    //------------------------------------------------------------------------------
    public void OnSample1_Third_MixRedSpeedNF()
    {
        int bg = 120;
        GameObject.FindGameObjectWithTag("Second").transform.position = Outside;
        float depth = GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().GetDepth();
        float depth2 = 200f;
        GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().addlayer();
        GameObject.FindGameObjectWithTag("LeftEye").GetComponent<Camera>().backgroundColor = new Color((float)(bg + 30) / 255, (float)(bg + 30) / 255, (float)(bg + 30) / 255);
        GameObject.FindGameObjectWithTag("RightEye").GetComponent<Camera>().backgroundColor = new Color((float)(bg + 30) / 255, (float)(bg + 30) / 255, (float)(bg + 30) / 255);
        StartCoroutine(Sample_Third_MixRedSpeedNF(depth, depth2, bg,2));
    }
    public void OnSample2_Third_MixRedSpeedNF()
    {
        int bg = 120;
        GameObject.FindGameObjectWithTag("Second").transform.position = Outside;
        float depth = GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().GetDepth();
        float depth2 = 3f;
        GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().addlayer();
        GameObject.FindGameObjectWithTag("LeftEye").GetComponent<Camera>().backgroundColor = new Color((float)(bg + 30) / 255, (float)(bg + 30) / 255, (float)(bg + 30) / 255);
        GameObject.FindGameObjectWithTag("RightEye").GetComponent<Camera>().backgroundColor = new Color((float)(bg + 30) / 255, (float)(bg + 30) / 255, (float)(bg + 30) / 255);
        StartCoroutine(Sample_Third_MixRedSpeedNF(depth, depth2, bg,5));
    }
    public void OnSample3_Third_MixRedSpeedNF()
    {
        int bg = 120;
        GameObject.FindGameObjectWithTag("Second").transform.position = Outside;
        float depth = GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().GetDepth();
        float depth2 = 1.5f;
        GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().addlayer();
        GameObject.FindGameObjectWithTag("LeftEye").GetComponent<Camera>().backgroundColor = new Color((float)(bg + 30) / 255, (float)(bg + 30) / 255, (float)(bg + 30) / 255);
        GameObject.FindGameObjectWithTag("RightEye").GetComponent<Camera>().backgroundColor = new Color((float)(bg + 30) / 255, (float)(bg + 30) / 255, (float)(bg + 30) / 255);
        StartCoroutine(Sample_Third_MixRedSpeedNF(depth, depth2, bg,10));
    }

    public void OnSample4_Third_MixRedSpeedNF()
    {
        int bg = 120;
        GameObject.FindGameObjectWithTag("Second").transform.position = Outside;
        float depth = GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().GetDepth();
        float depth2 = 0.5f;
        GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().addlayer();
        GameObject.FindGameObjectWithTag("LeftEye").GetComponent<Camera>().backgroundColor = new Color((float)(bg + 30) / 255, (float)(bg + 30) / 255, (float)(bg + 30) / 255);
        GameObject.FindGameObjectWithTag("RightEye").GetComponent<Camera>().backgroundColor = new Color((float)(bg + 30) / 255, (float)(bg + 30) / 255, (float)(bg + 30) / 255);
        StartCoroutine(Sample_Third_MixRedSpeedNF(depth, depth2, bg,20));
    }

    IEnumerator Sample_Third_MixRedSpeedNF(float depth, float depth2, int bg,int speed)
    {
        StartCoroutine((GameObject.FindGameObjectWithTag("ImageDisplay").GetComponent<UsetStudyMix>()).Go3(depth, depth2, bg,speed));
        yield return null;
    }
    //------------------------------------------------------------------------------


    //加红点的景深跟
    //------------------------------------------------------------------------------
    public void OnSample1_Third_MixRedSpeedF()
    {
        int bg = 120;
        GameObject.FindGameObjectWithTag("Second").transform.position = Outside;
        float depth = GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().GetDepth();
        float depth2 = 200f;
        GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().addlayer();
        GameObject.FindGameObjectWithTag("LeftEye").GetComponent<Camera>().backgroundColor = new Color((float)(bg + 30) / 255, (float)(bg + 30) / 255, (float)(bg + 30) / 255);
        GameObject.FindGameObjectWithTag("RightEye").GetComponent<Camera>().backgroundColor = new Color((float)(bg + 30) / 255, (float)(bg + 30) / 255, (float)(bg + 30) / 255);
        StartCoroutine(Sample_Third_MixRedSpeedF(depth, depth2, bg, 10));
    }
    public void OnSample2_Third_MixRedSpeedF()
    {
        int bg = 120;
        GameObject.FindGameObjectWithTag("Second").transform.position = Outside;
        float depth = GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().GetDepth();
        float depth2 = 3f;
        GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().addlayer();
        GameObject.FindGameObjectWithTag("LeftEye").GetComponent<Camera>().backgroundColor = new Color((float)(bg + 30) / 255, (float)(bg + 30) / 255, (float)(bg + 30) / 255);
        GameObject.FindGameObjectWithTag("RightEye").GetComponent<Camera>().backgroundColor = new Color((float)(bg + 30) / 255, (float)(bg + 30) / 255, (float)(bg + 30) / 255);
        StartCoroutine(Sample_Third_MixRedSpeedF(depth, depth2, bg, 30));
    }
    public void OnSample3_Third_MixRedSpeedF()
    {
        int bg = 120;
        GameObject.FindGameObjectWithTag("Second").transform.position = Outside;
        float depth = GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().GetDepth();
        float depth2 = 1.5f;
        GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().addlayer();
        GameObject.FindGameObjectWithTag("LeftEye").GetComponent<Camera>().backgroundColor = new Color((float)(bg + 30) / 255, (float)(bg + 30) / 255, (float)(bg + 30) / 255);
        GameObject.FindGameObjectWithTag("RightEye").GetComponent<Camera>().backgroundColor = new Color((float)(bg + 30) / 255, (float)(bg + 30) / 255, (float)(bg + 30) / 255);
        StartCoroutine(Sample_Third_MixRedSpeedF(depth, depth2, bg, 75));
    }

    public void OnSample4_Third_MixRedSpeedF()
    {
        int bg = 120;
        GameObject.FindGameObjectWithTag("Second").transform.position = Outside;
        float depth = GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().GetDepth();
        float depth2 = 0.5f;
        GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().addlayer();
        GameObject.FindGameObjectWithTag("LeftEye").GetComponent<Camera>().backgroundColor = new Color((float)(bg + 30) / 255, (float)(bg + 30) / 255, (float)(bg + 30) / 255);
        GameObject.FindGameObjectWithTag("RightEye").GetComponent<Camera>().backgroundColor = new Color((float)(bg + 30) / 255, (float)(bg + 30) / 255, (float)(bg + 30) / 255);
        StartCoroutine(Sample_Third_MixRedSpeedF(depth, depth2, bg, 120));
    }

    IEnumerator Sample_Third_MixRedSpeedF(float depth, float depth2, int bg, int speed)
    {
        StartCoroutine((GameObject.FindGameObjectWithTag("ImageDisplay").GetComponent<UsetStudyMix>()).Go3(depth, depth2, bg,speed));
        yield return null;
    }
    //------------------------------------------------------------------------------





    //加红点的景深
    public void OnSample1_Third_MixRed()
    {
        int bg = 120;
        GameObject.FindGameObjectWithTag("Second").transform.position = Outside;
        float depth = GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().GetDepth();
        float depth2 = 200f;
        GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().addlayer();
        GameObject.FindGameObjectWithTag("LeftEye").GetComponent<Camera>().backgroundColor = new Color((float)(bg + 30) / 255, (float)(bg + 30) / 255, (float)(bg + 30) / 255);
        GameObject.FindGameObjectWithTag("RightEye").GetComponent<Camera>().backgroundColor = new Color((float)(bg + 30) / 255, (float)(bg + 30) / 255, (float)(bg + 30) / 255);
        StartCoroutine(Sample_Third_MixRed(depth, depth2, bg));
    }
    public void OnSample2_Third_MixRed()
    {
        int bg = 120;
        GameObject.FindGameObjectWithTag("Second").transform.position = Outside;
        float depth = GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().GetDepth();
        float depth2 = 3f;
        GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().addlayer();
        GameObject.FindGameObjectWithTag("LeftEye").GetComponent<Camera>().backgroundColor = new Color((float)(bg + 30) / 255, (float)(bg + 30) / 255, (float)(bg + 30) / 255);
        GameObject.FindGameObjectWithTag("RightEye").GetComponent<Camera>().backgroundColor = new Color((float)(bg + 30) / 255, (float)(bg + 30) / 255, (float)(bg + 30) / 255);
        StartCoroutine(Sample_Third_MixRed(depth, depth2, bg));
    }
    public void OnSample3_Third_MixRed()
    {
        int bg = 120;
        GameObject.FindGameObjectWithTag("Second").transform.position = Outside;
        float depth = GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().GetDepth();
        float depth2 = 1.5f;
        GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().addlayer();
        GameObject.FindGameObjectWithTag("LeftEye").GetComponent<Camera>().backgroundColor = new Color((float)(bg + 30) / 255, (float)(bg + 30) / 255, (float)(bg + 30) / 255);
        GameObject.FindGameObjectWithTag("RightEye").GetComponent<Camera>().backgroundColor = new Color((float)(bg + 30) / 255, (float)(bg + 30) / 255, (float)(bg + 30) / 255);
        StartCoroutine(Sample_Third_MixRed(depth, depth2, bg));
    }

    public void OnSample4_Third_MixRed()
    {
        int bg = 120;
        GameObject.FindGameObjectWithTag("Second").transform.position = Outside;
        float depth = GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().GetDepth();
        float depth2 = 0.5f;
        GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().addlayer();
        GameObject.FindGameObjectWithTag("LeftEye").GetComponent<Camera>().backgroundColor = new Color((float)(bg + 30) / 255, (float)(bg + 30) / 255, (float)(bg + 30) / 255);
        GameObject.FindGameObjectWithTag("RightEye").GetComponent<Camera>().backgroundColor = new Color((float)(bg + 30) / 255, (float)(bg + 30) / 255, (float)(bg + 30) / 255);
        StartCoroutine(Sample_Third_MixRed(depth, depth2, bg));
    }

    IEnumerator Sample_Third_MixRed(float depth, float depth2, int bg)
    {

        StartCoroutine((GameObject.FindGameObjectWithTag("ImageDisplay").GetComponent<UsetStudyMix>()).Go2(depth, depth2, bg));
        yield return null;

    }

    //-----------------------------------------------------------------------------


    //不跟
    //------------------------------------------------------------------------------------------
    public void OnSample1_Second()
	{
		int qp1 = 22;
		int qp2 = 22;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		speed = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetSpeed ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		StartCoroutine (Sample_Second (speed, qp1, qp2));
	}

	public void OnSample2_Second()
	{
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		int qp1 = 22;
		int qp2 = 32;
		speed = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetSpeed ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		StartCoroutine (Sample_Second (speed, qp1, qp2));

	}
	public void OnSample3_Second()
	{
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		int qp1 = 22;
		int qp2 = 37;
		speed = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetSpeed ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		StartCoroutine (Sample_Second (speed, qp1, qp2));

	}
	public void OnSample4_Second()
	{
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		int qp1 = 22;
		int qp2 = 22;
		speed = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetSpeed ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		StartCoroutine (Sample_Second (speed, qp1, qp2));
	}
	public void OnSample5_Second()
	{
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		int qp1 = 22;
		int qp2 = 27;
		speed = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetSpeed ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		StartCoroutine (Sample_Second (speed, qp1, qp2));
	}
	public void OnSample6_Second()
	{
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		int qp1 = 22;
		int qp2 = 42;
		speed = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetSpeed ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		StartCoroutine (Sample_Second (speed, qp1, qp2));
	}

	IEnumerator Sample_Second(float speed,int qp1,int qp2)
	{
		GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponentInChildren<TextMesh> ().text = "接下来将会为屏幕中心会出现一个黑点\n请您保持跟随注视它,请您给出\n跟随过程中图片是否发生变化";
		yield return  new WaitForSeconds (5f);
		StartCoroutine ((GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponent<UserStudyTwo> ().Rotate (speed, qp1, qp2)));

	}

	//------------------------------------------------------------------------------------------





	//跟
	//------------------------------------------------------------------------------------------
	public void OnSample1_Seccond_Mix()
	{
		int bg = 127;
		int bg2 = 0;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		speed = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetSpeed ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		GameObject.FindGameObjectWithTag ("LeftEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		GameObject.FindGameObjectWithTag ("RightEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		Sample_Second_Mix (bg2, speed);
	}

	public void OnSample2_Seccond_Mix()
	{
		int bg = 127;
		int bg2 = 75;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		speed = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetSpeed ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer ();
		GameObject.FindGameObjectWithTag ("LeftEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		GameObject.FindGameObjectWithTag ("RightEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		Sample_Second_Mix (bg2, speed);

	}
	public void OnSample3_Seccond_Mix()
	{
		int bg = 127;
		int bg2 = 120;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		speed = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetSpeed ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		GameObject.FindGameObjectWithTag ("LeftEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		GameObject.FindGameObjectWithTag ("RightEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		Sample_Second_Mix (bg2, speed);
	}
	public void OnSample4_Seccond_Mix()
	{
		int bg = 127;
		int bg2 = 165;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		speed = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetSpeed ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		GameObject.FindGameObjectWithTag ("LeftEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		GameObject.FindGameObjectWithTag ("RightEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		Sample_Second_Mix (bg2, speed);

	}
	public void OnSample5_Seccond_Mix()
	{
		int bg = 127;
		int bg2 = 210;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		speed = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetSpeed ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		GameObject.FindGameObjectWithTag ("LeftEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		GameObject.FindGameObjectWithTag ("RightEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		Sample_Second_Mix (bg2, speed);

	}
	public void OnSample6_Seccond_Mix()
	{
		int bg = 127;
		int bg2 = 255;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		speed = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetSpeed ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		GameObject.FindGameObjectWithTag ("LeftEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		GameObject.FindGameObjectWithTag ("RightEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		Sample_Second_Mix (bg2, speed);
	}

	public void Sample_Second_Mix(int bg,float speed)
	{

		StartCoroutine ((GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponent<UserStudyMixSpeed> ().Rotate (bg,speed)));
	}
	//------------------------------------------------------------------------------------------














	//跟

	//------------------------------------------------------------------------------------------
	IEnumerator Sample(float speed,int qp1,int qp2)
	{
		GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponentInChildren<TextMesh> ().text = "接下来将请您用眼睛跟随图片旋转,头部不要旋转\n最后给出图片是否发生变化";
		yield return  new WaitForSeconds (5f);
		StartCoroutine ((GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponent<UserStudyOne> ().Rotate (speed,qp1,qp2)));
	
	}


	public void OnSampel1()
	{
		int qp1 = 22;
		int qp2 = 37;
 		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		speed = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetSpeed ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		StartCoroutine (Sample (speed, qp1, qp2));
	}


	public void  OnSampel2()
	{
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		int qp1 = 22;
		int qp2 = 22;
		speed = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetSpeed ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		StartCoroutine (Sample (speed, qp1, qp2));
	}

	public void  OnSampel3()
	{
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		int qp1 = 22;
		int qp2 = 27;
		speed = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetSpeed ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		StartCoroutine (Sample (speed, qp1, qp2));
	}

	public void  OnSampel4()
	{
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		int qp1 = 22;
		int qp2 = 42;
		speed = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetSpeed ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		StartCoroutine (Sample (speed, qp1, qp2));
	}

	public void  OnSampel5()
	{
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		int qp1 = 22;
		int qp2 = 32;
		speed = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetSpeed ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		StartCoroutine (Sample (speed, qp1, qp2));
	}

	public void  OnSampel6()
	{
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		int qp1 = 22;
		int qp2 = 22;
		speed = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetSpeed ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		StartCoroutine (Sample (speed, qp1, qp2));
	}
	//------------------------------------------------------------------------------------------




	//不跟

	public void OnSample1_Mix()
	{
		int bg = 127;
		int bg2 = 0;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		speed = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetSpeed ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		GameObject.FindGameObjectWithTag ("LeftEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		GameObject.FindGameObjectWithTag ("RightEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		Sample_Mix (bg2, speed);
	}

	public void OnSample2_Mix()
	{
		int bg = 127;
		int bg2 = 75;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		speed = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetSpeed ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer ();
		GameObject.FindGameObjectWithTag ("LeftEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		GameObject.FindGameObjectWithTag ("RightEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		Sample_Mix (bg2, speed);

	}
	public void OnSample3_Mix()
	{
		int bg = 127;
		int bg2 = 120;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		speed = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetSpeed ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		GameObject.FindGameObjectWithTag ("LeftEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		GameObject.FindGameObjectWithTag ("RightEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		Sample_Mix (bg2, speed);
	}
	public void OnSample4_Mix()
	{
		int bg = 127;
		int bg2 = 165;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		speed = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetSpeed ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		GameObject.FindGameObjectWithTag ("LeftEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		GameObject.FindGameObjectWithTag ("RightEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		Sample_Mix (bg2, speed);

	}
	public void OnSample5_Mix()
	{
		int bg = 127;
		int bg2 = 210;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		speed = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetSpeed ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		GameObject.FindGameObjectWithTag ("LeftEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		GameObject.FindGameObjectWithTag ("RightEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		Sample_Mix (bg2, speed);

	}
	public void OnSample6_Mix()
	{
		int bg = 127;
		int bg2 = 255;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
		speed = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetSpeed ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		GameObject.FindGameObjectWithTag ("LeftEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		GameObject.FindGameObjectWithTag ("RightEye").GetComponent<Camera> ().backgroundColor = new Color ((float)bg/255, (float)bg/255, (float)bg/255);
		Sample_Mix (bg2, speed);
	}

	public void Sample_Mix(int bg,float speed)
	{
			StartCoroutine ((GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponent<UserStudyMixSpeed> ().Rotate (bg,speed)));
	}
}
