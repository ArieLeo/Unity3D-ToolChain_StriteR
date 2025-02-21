﻿//#pragma multi_compile_local _ _WAVEINTERACTION
#define MAX_ADDITIONAL_WAVE_COUNT 16u
uint _AdditionalWavesCount;
float4 _AdditionalWavesShape[MAX_ADDITIONAL_WAVE_COUNT];
float4 _AdditionalWavesDir[MAX_ADDITIONAL_WAVE_COUNT];

float2 WaveInteraction(float3 positionWS)
{
    #ifndef _WAVEINTERACTION
        return 0;
    #endif
    float2 waveDirection=0;
    uint additionalWaveCount=min(_AdditionalWavesCount,MAX_ADDITIONAL_WAVE_COUNT);
    for(uint index=0u;index<additionalWaveCount;index++)
    {
        float4 shapes=_AdditionalWavesShape[index];
        float4 directions=_AdditionalWavesDir[index];
        float2 waveOffset=positionWS.xz-shapes.xy;
        float sqrDistance=dot(waveOffset,waveOffset);
        float waveStrength= step(shapes.z,sqrDistance)*step(sqrDistance,shapes.w);
        waveOffset*=step(-.25,dot(normalize(waveOffset),directions.xy))*waveStrength;
        waveDirection+=waveOffset;
    }
    return waveDirection;
}