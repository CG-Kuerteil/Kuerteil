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
		inside = false;
	}

	void OnTriggerEnter(Collider col)
	{
		Debug.Log("Ladder ON " + col.gameObject.name);
		if (col.gameObject.name == "Ladder")
		{
			Debug.Log("Climbing");
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
			inside = !inside;
		}
	}

	void Update()
	{
		if (inside == true && Input.GetKey("w"))
		{
			Debug.Log("Pressing W");
			FPSInput.transform.position += Vector3.up * speedUpDown;
		}

		if (inside == true && Input.GetKey("s"))
		{
			FPSInput.transform.position += Vector3.down * speedUpDown;
		}
	}

}
