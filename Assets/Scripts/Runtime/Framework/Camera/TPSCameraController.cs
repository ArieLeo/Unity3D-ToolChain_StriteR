﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode,RequireComponent(typeof(Camera))]
public class TPSCameraController : CameraController
{
    [Header("Shake Param")]
    public int I_ShakeParam;
    public float m_ShakeReverseDuration=.2f;
    Vector3 v3_Recoil;
    Vector3 v3_Shake;
    float inverseCheck = 0;
    bool b_shakeReverse;
    protected override Vector3 GetRootOffsetAdditive(float _deltaTime)
    {
        inverseCheck += _deltaTime;
        if (inverseCheck > m_ShakeReverseDuration)
        {
            b_shakeReverse = !b_shakeReverse;
            inverseCheck -= m_ShakeReverseDuration;
        }

        v3_Shake = Vector3.Lerp(v3_Shake, Vector3.zero, I_ShakeParam * Time.deltaTime);
        return (b_shakeReverse ? -1 : 1) * v3_Shake;
    }
    protected override Vector3 GetRootRotateAdditive(float _deltaTime)
    {
        v3_Recoil = Vector3.Lerp(v3_Recoil, Vector3.zero, _deltaTime* 5f);
        return v3_Recoil;
    }
    public void AddRecoil(float recoilAmount)=>v3_Recoil +=new Vector3(0,( URandom.RandomBool() ? 1 : -1) * recoilAmount, 0);
    public void AddShake(float shakeAmount) => v3_Shake += Random.insideUnitSphere * shakeAmount;
    public void SetImpact(Vector3 impactDirection)
    {
        v3_Shake = impactDirection;
        b_shakeReverse = false;
    } 
}
