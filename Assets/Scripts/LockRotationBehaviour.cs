using UnityEngine;
using System.Collections;

public class LockRotationBehaviour : MonoBehaviour {

    private Quaternion originalRotation;

    void Awake()
    {
        originalRotation = transform.rotation;
    }
    void LateUpdate()
    {
        transform.rotation = originalRotation;
    }
}
