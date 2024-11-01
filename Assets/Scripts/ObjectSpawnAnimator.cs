using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;

public class ObjectSpawnAnimator : MonoBehaviour
{
    private Vector3 originalScale;


    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;

        StartCoroutine(ScaleIn());
    }

    private IEnumerator ScaleIn()
    {
        yield return null;
        Tween.Scale(transform, originalScale, ObjectSpawnAnimatorParameters.Instance.AnimationTimeIn, ObjectSpawnAnimatorParameters.Instance.AnimationEase);
    }
    private IEnumerator ScaleOut()
    {
        yield return null;
        Tween.Scale(transform, Vector3.zero, ObjectSpawnAnimatorParameters.Instance.AnimationTimeOut, ObjectSpawnAnimatorParameters.Instance.AnimationEase);
    }

    public void StartCoroutineScaleOut()
    {
        StartCoroutine(ScaleOut());
    }
}
