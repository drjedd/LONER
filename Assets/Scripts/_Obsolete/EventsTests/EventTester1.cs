using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EventTester1 : MonoBehaviour {

    private UnityAction someListener;
	private UnityAction anotherListener;

    void Awake()
    {
        someListener = new UnityAction(SomeFunction);
		anotherListener = new UnityAction(SomeOtherFunction);
    }

    void OnEnable()
    {
        EventManager.StartListening("testEvent", someListener);
		EventManager.StartListening("testEvent3", someListener);

		EventManager.StartListening("testEvent2", someListener);
		EventManager.StartListening("testEvent2", anotherListener);
	}

    void OnDisable()
    {
        EventManager.StopListening("testEvent", someListener); //VITAL to avoid memory leaks
		EventManager.StopListening("testEvent3", someListener);

		EventManager.StopListening("testEvent2", someListener);
		EventManager.StopListening("testEvent2", anotherListener);
	}

    void SomeFunction()
    {
        Debug.Log("SOME FUNCTION CALLED");
    }

	void SomeOtherFunction()
	{
		Debug.Log("ANOTHER FUNCTION CALLED");
	}
}
