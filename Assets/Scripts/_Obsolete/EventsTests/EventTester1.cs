using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EventTester1 : MonoBehaviour {

    private UnityAction someListener;

    void Awake()
    {
        someListener = new UnityAction(SomeFunction);
    }

    void OnEnable()
    {
        EventManager.StartListening("testEvent", someListener);
    }

    void OnDisable()
    {
        EventManager.StopListening("testEvent", someListener); //VITAL to avoid memory leaks
    }

    void SomeFunction()
    {
        Debug.Log("SOME FUNCTION CALLED");
    }
}
