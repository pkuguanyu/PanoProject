using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class InterTrigger : MonoBehaviour {

	float speed=0f;
	float depth=0f;
	float depth2=0f;
	int layer=1;
	float Distance=9f;
	float brightness;
	Vector3 Original;
	Vector3 Outside=new Vector3(1000f, 1000f, 0f);
	Vector2 ImageSize=new Vector2(15,10);
	void Start()
	{
		Original=new Vector3(0f,0f,Distance);
	}

	public void setbrightness(float b)
	{
		brightness = b;
	}

	public float getBrightness()
	{
		return brightness;
	}

	public Vector2 GetImageSize()
	{
		return ImageSize;
	}
	public float GetDistance()
	{
		return Distance;
	}
	public void SetSpeed(float s)
	{
		speed = s;
	}
	public float GetSpeed()
	{
		return speed;
	}

	public void SetDepth(float d)
	{
		depth = d;
	}

	public float GetDepth()
	{
		return depth;
	}
	public void SetDepth2(float d)
	{
		depth2 = d;
	}

	public float GetDepth2()
	{
		return depth2;
	}

	public void addlayer()
	{
		layer = layer + 1;
	}

	public void minuslayer()
	{
		layer = layer - 1;
	}



	public void returnlayer()
	{
		if (layer == 2)
		{
			GameObject.FindGameObjectWithTag ("First").transform.position = Original;
			GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
			GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponent<UserStudyOne> ().reset ();
			layer = layer - 1;
		}
		else if (layer == 3) 
		{
			GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
			GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponent<UserStudyOne> ().reset ();
			layer = layer - 1;
		}
		else if (layer == 1) //回主菜
		{
			SceneManager.LoadScene("MainScene");
			//layer = layer - 1;
		}
	}


	public void returnlayer2()
	{
		if (layer == 2)
		{
			GameObject.FindGameObjectWithTag ("First").transform.position = Original;
			GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
			GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponent<UserStudyTwo> ().reset ();
			layer = layer - 1;
		}
		else if (layer == 3) 
		{
			GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
			GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponent<UserStudyTwo> ().reset ();
			layer = layer - 1;
		}
		else if (layer == 1) //回主菜
		{
			SceneManager.LoadScene("MainScene");
			//layer = layer - 1;
		}
	}


	public void returnlayer3()
	{
		if (layer == 2)
		{
			GameObject.FindGameObjectWithTag ("First").transform.position = Original;
			GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
			GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponent<UserStudyThree> ().reset ();
			layer = layer - 1;
		}
		else if (layer == 3) 
		{
			GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
			GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponent<UserStudyThree> ().reset ();
			layer = layer - 1;
		}
		else if (layer == 1) //回主菜
		{
			SceneManager.LoadScene("MainScene");
			//layer = layer - 1;
		}
	}

	public void returnlayer4()
	{
		if (layer == 2)
		{
			GameObject.FindGameObjectWithTag ("First").transform.position = Original;
			GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
			GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponent<UsetStudyMix> ().reset ();
			layer = layer - 1;

		}
		else if (layer == 3) 
		{
			GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
			GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponent<UsetStudyMix> ().reset ();
			layer = layer - 1;
		}
		else if (layer == 1) //回主菜
		{
			SceneManager.LoadScene("MainScene");
			//layer = layer - 1;
		}
	}


	public void returnlayer5()
	{
		if (layer == 2)
		{
			GameObject.FindGameObjectWithTag ("First").transform.position = Original;
			GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
			GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponent<UserStudyMixSpeed> ().reset ();
			layer = layer - 1;

		}
		else if (layer == 3) 
		{
			GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
			GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponent<UserStudyMixSpeed> ().reset ();
			layer = layer - 1;
		}
		else if (layer == 1) //回主菜
		{
			SceneManager.LoadScene("MainScene");
			//layer = layer - 1;
		}
	}


	public void returnlayer6()
	{
		if (layer == 2)
		{
			GameObject.FindGameObjectWithTag ("First").transform.position = Original;
			GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
			GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponent<UserStudyMixSpeed> ().reset ();
			layer = layer - 1;

		}
		else if (layer == 3) 
		{
			GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
			GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponent<UserStudyMixSpeed> ().reset ();
			layer = layer - 1;
		}
		else if (layer == 1) //回主菜
		{
			SceneManager.LoadScene("MainScene");
			//layer = layer - 1;
		}
	}


	public void returnlayer7()
	{
		if (layer == 2)
		{
			GameObject.FindGameObjectWithTag ("First").transform.position = Original;
			GameObject.FindGameObjectWithTag ("Second").transform.position = Outside;
			GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponent<UsetBrightMix> ().reset ();
			layer = layer - 1;

		}
		else if (layer == 3) 
		{
			GameObject.FindGameObjectWithTag ("Second").transform.position = Original;
			GameObject.FindGameObjectWithTag ("ImageDisplay").GetComponent<UsetBrightMix> ().reset ();
			layer = layer - 1;
		}
		else if (layer == 1) //回主菜
		{
			SceneManager.LoadScene("MainScene");
			//layer = layer - 1;
		}
	}

}
