﻿using System.Collections.Generic;
using System.Threading.Tasks;
using FlowEnt;
using UnityEngine;

public class FlowEntExampleController : MonoBehaviour
{
    private const float CameraJourneyLength = 6f;

    [SerializeField]
    private Transform cameraTransform;
    private Transform CameraTransform => cameraTransform;

    [SerializeField]
    private List<Transform> objects;
    private List<Transform> Objects => objects;

    [SerializeField]
    private List<Vector3> splinePoints;
    private List<Vector3> SplinePoints => splinePoints;

    [SerializeField]
    private AnimationCurve animationCurve;
    private AnimationCurve AnimationCurve => animationCurve;

    private async void Start()
    {
        await Task.Delay(3000);

        //await BezierFlow(Objects[0]);
        Objects[0].Tween(2)
            .SetTimeScale(0.5f)
            .Move(Vector3.up)
            .RotateY(180f);

        new Flow()
            .Queue(new Tween(1).OnAfterUpdate(t => Debug.Log($"1 {Time.time}")))
            .Queue(new Tween(1).OnAfterUpdate(t => Debug.Log($"1 {Time.time}")))
            .Queue(new Tween(1).OnAfterUpdate(t => Debug.Log($"1 {Time.time}")))
            .Queue(new Tween(1).OnAfterUpdate(t => Debug.Log($"1 {Time.time}")))
            .At(0, new Tween(2).OnAfterUpdate(t => Debug.Log($"2 {Time.time}")))
            .Queue(new Tween(2).OnAfterUpdate(t => Debug.Log($"2 {Time.time}")))
            .At(0, new Tween(4).OnAfterUpdate(t => Debug.Log($"3 {Time.time}")))
            .Start();

        await new Flow(new FlowOptions() { LoopCount = 2, AutoStart = true })
            .SetTimeScale(10)
            .Queue(t => t
                .SetTime(2)
                .For(Objects[2])
                    .MoveY(2)
                .For(Objects[2].GetComponent<MeshRenderer>())
                    .ColorTo(Color.black))
            .Queue(Objects[2].Tween(2f).MoveY(2))
            .Queue(t => t.SetTime(1))
            .At(1, Objects[2].Tween(3f).MoveY(2))
            .Queue(t => t.SetOptions(o => o.SetTime(1)))
            .AsAsync();
    }

    #region Flow

    private async Task BezierFlow(Transform transform)
    {
        BezierSpline spline = new BezierSpline(SplinePoints);
        await transform
            .Tween(50f)
                .MoveTo(spline)
            .OrientToPath()
            .Tween
            .OnComplete(() => transform.transform.rotation = Quaternion.identity)
            .AsAsync();
    }

    #endregion

    #region Editor

    private void OnDrawGizmos()
    {
        DrawSpline();
    }

    private void DrawSpline()
    {
        if (SplinePoints.Count < 1)
        {
            return;
        }
        for (int i = 0; i < SplinePoints.Count - 1; i++)
        {
            Gizmos.DrawLine(SplinePoints[i], SplinePoints[i + 1]);
        }
    }

    #endregion
}
