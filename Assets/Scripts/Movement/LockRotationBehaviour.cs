using UnityEngine;
using System.Collections;

//locks rotation of a child object: we want our sprites to remain at rotation 0, we "fake" the rotation by displaying various sprites instead
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
