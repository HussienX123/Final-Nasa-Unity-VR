using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;

public class GalaxyAnimations : MonoBehaviour
{
    public float scalingSpeedOne = 1f;
    public float scalingSpeedTwo = 1f;

    public Vector3 EndScale1;
    public Vector3 EndScale2;

    public Vector3 EndScale3;

    public MeshRenderer[] Plantes;
    public MeshRenderer Earth;

    public UnityEvent Event2;
    public UnityEvent Event3;

    private void Start()
    {
        Earth.material.DOFloat(0, "_Alpha", 0.1f);
    }

    [Button("S1")]
    public void ScaleUp1()
    {
        transform.DOScale(EndScale1, scalingSpeedOne);
    }

    [Button("S2")]
    public void ScaleUp2()
    {
        transform.DOScale(EndScale2, scalingSpeedTwo);
        Event2.Invoke();
    }

    [Button("S3")]
    public void ScaleUp3()
    {
        transform.DOScale(EndScale3, 2f).OnComplete(() => {
            Earth.transform.DOScale(0.65f, 1f).OnComplete(() => { Earth.material.DOFloat(1, "_Alpha", 0.3f); });
        });

        

        Event3.Invoke();
    }
}
