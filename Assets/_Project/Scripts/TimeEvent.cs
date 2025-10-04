using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeEvent : MonoBehaviour
{
    private bool isEventDone = false;
    public float eventTime = 1f; // Time in seconds before the event occurs
    public UnityEvent OneTimeEvent;

    public bool AutoStart = false;

    void Start()
    {
        if(AutoStart)
        {
            StartCoroutine(TriggerEventAfterTime());
        }
    }

    public void StartEvent()
    {
        if (!isEventDone)
        {
            StartCoroutine(TriggerEventAfterTime());
        }
    }

    private IEnumerator TriggerEventAfterTime()
    {
        yield return new WaitForSeconds(eventTime);
        if (!isEventDone)
        {
            OneTimeEvent.Invoke();
            isEventDone = true;
        }
    }
}
