using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;

public class ObjectSpawnAnimatorParameters : Singleton<ObjectSpawnAnimatorParameters>
{
	public float AnimationTimeIn = 1f;
	public float AnimationTimeOut = 0.75f;
	public Ease AnimationEase = Ease.Linear;
}
