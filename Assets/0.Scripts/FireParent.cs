using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireParent : MonoBehaviour
{
	public float rotSpeed;

	private void FixedUpdate()
	{
		transform.Rotate(new Vector3(0,0,rotSpeed), Space.Self);
	}
}
