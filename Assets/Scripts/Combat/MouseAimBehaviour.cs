using UnityEngine;
using System.Collections;

public class MouseAimBehaviour : MonoBehaviour
{
	void Update()
    {

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, transform.position - mousePosition);
        transform.rotation = rotation;
    }
}