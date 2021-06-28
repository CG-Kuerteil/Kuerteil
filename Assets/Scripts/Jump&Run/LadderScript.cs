using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderScript : MonoBehaviour
{

	public Transform chController;
	bool inside = false;
	public float speedUpDown = 1.5f;
	public FirstPersonController FPSInput;

	void Start()
	{	
		//FPSInput = GetComponent<FirstPersonController>();
		if (FPSInput != null)
        {
			//Debug.Log("The class was loaded:");
        }
        else
        {
			//Debug.Log("The class was not loaded");
        }
		inside = false;
	}

	void OnTriggerEnter(Collider col)
	{
		Debug.Log("Ladder ON " + col.gameObject.name);
		if (col.gameObject.name == "Ladder")
		{
			Debug.Log("Climbing");
			//FPSInput.enabled = false;
			//FPSInput.playerCanMove = false;
			inside = !inside;
			Debug.Log("inside value: " + inside);
		}
	}

	void OnTriggerExit(Collider col)
	{
		Debug.Log("Ladder OFF");
		if (col.gameObject.name == "Ladder")
		{
			Debug.Log("Stop Climbing");
			//FPSInput.enabled = true;
			//FPSInput.playerCanMove = true;
			inside = !inside;
		}
	}

	void Update()
	{
		Debug.Log("In Update, inside value : " + inside);
		if (inside == true && Input.GetKey("w"))
		{
			Debug.Log("Pressing W");
			//chController.transform.position += Vector3.up / speedUpDown;
			FPSInput.transform.position += Vector3.up * speedUpDown;
		}

		if (inside == true && Input.GetKey("s"))
		{
			//chController.transform.position += Vector3.down / speedUpDown;
			FPSInput.transform.position += Vector3.down * speedUpDown;
		}
	}

}
