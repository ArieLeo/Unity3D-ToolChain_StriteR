﻿using System;
using Unity.Mathematics;
using UnityEngine;

namespace Runtime.Geometry.Curves
{
    using static math;
    using static umath;

    [Serializable]
    public partial struct GBezierCurveQuadratic : ICurveTangents<float3>
    {
        public float3 source;
        public float3 control;
        public float3 destination;

        [HideInInspector] public float3 tangentSource;
        [HideInInspector] public float3 tangentDestination;

        public GBezierCurveQuadratic(float3 _src, float3 _dst, float3 _control)
        {
            source = _src;
            destination = _dst;
            control = _control;
            tangentSource = default;
            tangentDestination = default;
            Ctor();
        }

        void Ctor()
        {
            tangentSource = normalize(control - source);
            tangentDestination = normalize(destination - control);
        }
        
        public float3 Evaluate(float _value)
        {
            float value = _value;
            float oneMinusValue = 1 - value;
            return sqr(oneMinusValue) * source + 2 * (oneMinusValue) * value * control + sqr(value) * destination;
        }

        public float3 EvaluateTangent(float _value) => normalize(lerp(tangentSource, tangentDestination, _value));
    }
    
    public partial struct GBezierCurveQuadratic : ISerializationCallbackReceiver
    {
        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize() => Ctor();
    }
    
    public static class GBezierCurveQuadratic_Extension
    {

        public static GBox GetBoundingBox(this GBezierCurveQuadratic _curve)
        {
            var source = _curve.source;
            var destination = _curve.destination;
            var control = _curve.control;
            
            var min = math.min(source, destination);
            var max = math.max(source, destination);
            GBox box = GBox.Minmax(min, max);
            if (!box.Contains(control))
            {
                float3 t = ((source - control) / (source - 2 * control + destination)).saturate();
                float3 s = 1f - t;
                float3 q = s * s * source + 2 * s * t * control + t * t * destination;
                box = GBox.Minmax(math.min(min, q), Vector3.Max(max, q));
            }

            return box;
        }
        
#if UNITY_EDITOR
        public static void DrawGizmos(this GBezierCurveCubic _curve,int _density=64)
        {
            var source = _curve.source;
            var destination = _curve.destination;
            var controlSource = _curve.controlSource;
            var controlDestination = _curve.controlDestination;

            UnityEngine.Gizmos.color = Color.green;
            UnityEngine.Gizmos.DrawWireSphere(source,.05f);
            UnityEngine.Gizmos.DrawLine(source,controlSource);
            UnityEngine.Gizmos.DrawLine(destination,controlDestination);
            UnityEngine.Gizmos.color = Color.blue;
            UnityEngine.Gizmos.DrawWireSphere(destination,.05f);
            UnityEngine.Gizmos.color = Color.red;
            UnityEngine.Gizmos.DrawWireSphere(controlSource,.05f);
            UnityEngine.Gizmos.DrawWireSphere(controlDestination,.05f);
            UnityEngine.Gizmos.color = Color.white;
            
            UCurve.DrawGizmos(_curve);
        }
#endif
    }
}