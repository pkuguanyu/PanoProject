using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class FirstEventTrigger : MonoBehaviour {

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
		GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponent<Image> ().GetComponent<RectTransform> ().sizeDelta = GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().GetImageSize ();
	}
	//--------------------------------------------------------------------------------------------
	public void OnDepth2()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetDepth (0.5f);
	}

	public void OnDepth5()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetDepth (1.5f);
	}

	public void OnDepth10()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetDepth (2.5f);
	}

	public void OnDepth50()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetDepth (7500f);
	}
	//--------------------------------------------------------------------------------------------











	//--------------------------------------------------------------------------------------------

	public void OnDepth2_Mix()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetDepth (0.5f);
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetDepth2 (100f);
	}

	public void OnDepth5_Mix()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetDepth (0.5f);
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetDepth2 (120f);
	}

	public void OnDepth10_Mix()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetDepth (0.5f);
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetDepth2 (150f);
	}

	public void OnDepth50_Mix()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetDepth (0.5f);
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetDepth2 (150f);
	}
    //--------------------------------------------------------------------------------------------

    //--------------------------------------------------------------------------------------------

    public void OnDepth1_MixRed()
    {
        GameObject.FindGameObjectWithTag("First").transform.position = Outside;
        GameObject.FindGameObjectWithTag("Second").transform.position = Original;
        GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().addlayer();
        GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().SetDepth(0.5f);
     //   GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().SetDepth2(100f);
    }

    public void OnDepth15_MixRed()
    {
        GameObject.FindGameObjectWithTag("First").transform.position = Outside;
        GameObject.FindGameObjectWithTag("Second").transform.position = Original;
        GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().addlayer();
        GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().SetDepth(1.5f);
       // GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().SetDepth2(120f);
    }

    public void OnDepth3_MixRed()
    {
        GameObject.FindGameObjectWithTag("First").transform.position = Outside;
        GameObject.FindGameObjectWithTag("Second").transform.position = Original;
        GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().SetDepth(3f);
      //  GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().SetDepth2(150f);
    }

    public void OnDepth200_MixRed()
    {
        GameObject.FindGameObjectWithTag("First").transform.position = Outside;
        GameObject.FindGameObjectWithTag("Second").transform.position = Original;
        GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().SetDepth(200f);
        //GameObject.FindGameObjectWithTag("Pause").GetComponent<InterTrigger>().SetDepth2(150f);
    }
    //--------------------------------------------------------------------------------------------



    //--------------------------------------------------------------------------------------




    public void OnBright30()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().setbrightness (30f);
	}

	public void OnBright127()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pau-两bkuuse").GetComponent<InterTrigger> ().addlayer();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().setbrightness (127f);
	}

	public void OnBright255()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer ();
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().setbrightness (255f);
	}





	//--------------------------------------------------------------------------------------

	//--------------------------------------------------------------------------------------------
	public void Onspeed5_Mix()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetSpeed (5f);
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
	}

	public void Onspeed20_Mix()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetSpeed (35f);
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
	}

	public void Onspeed30_Mix()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetSpeed (65f);
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
	}

	public void Onspeed60_Mix()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetSpeed (95f);
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
	}
	//--------------------------------------------------------------------------------------------



	//--------------------------------------------------------------------------------------------
	public void Onspeed5()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetSpeed (5f);
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
	}

	public void Onspeed20()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetSpeed (35f);
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
	}

	public void Onspeed30()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetSpeed (65f);
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
	}

	public void Onspeed60()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetSpeed (95f);
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
	}
	//--------------------------------------------------------------------------------------------


	//--------------------------------------------------------------------------------------------

	public void Onspeed2_Second()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetSpeed (2f);
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
	}

	public void Onspeed5_Second()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetSpeed (5f);
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
	}

	public void Onspeed7_Second()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetSpeed (7f);
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
	}

	public void Onspeed10_Second()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetSpeed (10f);
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
	}

	//--------------------------------------------------------------------------------------------


	//--------------------------------------------------------------------------------------------------------
	public void Onspeed2_Second_Mix()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetSpeed (2f);
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
	}

	public void Onspeed5_Second_Mix()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetSpeed (5f);
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
	}

	public void Onspeed7_Second_Mix()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetSpeed (7f);
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
	}

	public void Onspeed10_Second_Mix()
	{
		GameObject.FindGameObjectWithTag ("First").transform.position = Outside;
		GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().SetSpeed (10f);
		GameObject.FindGameObjectWithTag ("Pause").GetComponent<InterTrigger> ().addlayer();
	}

	//--------------------------------------------------------------------------------------------------------

}
 