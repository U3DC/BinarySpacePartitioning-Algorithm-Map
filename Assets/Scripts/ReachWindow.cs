using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachWindow : MonoBehaviour {
	
	public int Boundary = 50; // distance from edge scrolling starts
	private int theScreenWidth;
	private int theScreenHeight;

	float speed = 15.0f;

	private void Update(){
		if (Input.GetAxis ("Mouse X") > 0) {
			transform.position += new Vector3 (Input.GetAxisRaw ("Mouse X") * Time.deltaTime * speed,
				0.0f, Input.GetAxisRaw ("Mouse Y") * Time.deltaTime * speed);
		}else if(Input.GetAxis("Mouse X") < 0){
			transform.position += new Vector3 (Input.GetAxisRaw("Mouse X") * Time.deltaTime * speed,
				0.0f, Input.GetAxisRaw("Mouse Y") * Time.deltaTime * speed);
		}
	}
}
