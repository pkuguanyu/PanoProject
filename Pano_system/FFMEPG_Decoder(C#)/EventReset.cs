using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventReset : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void  OnPointClick()
	{
		GameObject.FindGameObjectWithTag ("ImageDisPlay").GetComponentInChildren<TextMesh> ().text = "你按下了扳机！！！！！";
	}
}
