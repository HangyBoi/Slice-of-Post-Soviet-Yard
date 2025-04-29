#ifndef ADDITIONAL_LIGHT_INCLUDED
#define ADDITIONAL_LIGHT_INCLUDED


void MainLight_float(in float3 WorldPos, out float3 Direction, out float3 LightColor, out float DistanceAtten)
{ 
    
#ifdef SHADERGRAPH_PREVIEW
    Direction = normalize(float3(1.0f, 1.0f, 0.0f));
    LightColor = 1.0f;
    DistanceAtten = 1.0f;
#else   
    Light mainLight = GetMainLight();
    Direction = mainLight.direction;
    LightColor = mainLight.color;
    DistanceAtten = mainLight.distanceAttenuation;
#endif
  
}


void MainLight_half(in half3 WorldPos, out half3 Direction, out half3 LightColor, out half DistanceAtten)
{
    
#if SHADERGRAPH_PREVIEW
    Direction = normalize(half3(0.0f, 1.0f, 1.0f));
    LightColor = 1.0f;
    DistanceAtten = 1.0f;
#else
    Light mainLight = GetMainLight();
    Direction = mainLight.direction;
    LightColor = mainLight.color;
    DistanceAtten = mainLight.distanceAttenuation;
#endif   

}



void AdditionalLight_float(in float3 WorldPos, in int lightID, out float3 Direction, out float3 LightColor, out float DistanceAtten)
{
    Direction = normalize(float3(1.0f, 1.0f, 0.0f));
    LightColor = 0.0f;
    DistanceAtten = 0.0f;

#ifndef SHADERGRAPH_PREVIEW
    int lightCount = GetAdditionalLightsCount();
    if (lightID < lightCount)
    {
        Light light = GetAdditionalLight(lightID, WorldPos);
        Direction = light.direction;
        LightColor = light.color;
        DistanceAtten = light.distanceAttenuation;
    }
#endif
    
}


void AdditionalLight_half(in half3 WorldPos, in int lightID, out half3 Direction, out half3 LightColor, out half DistanceAtten)
{
    Direction = normalize(half3(0.0f, 1.0f, 1.0f));
    LightColor = 0.0f;
    DistanceAtten = 0.0f;

#ifndef SHADERGRAPH_PREVIEW
    int lightCount = GetAdditionalLightsCount();
    if (lightID < lightCount)
    {
        Light light = GetAdditionalLight(lightID, WorldPos);
        Direction = light.direction;
        LightColor = light.color;
        DistanceAtten = light.distanceAttenuation;
    }
#endif
    
}



void AllAdditionalLights_float(in float3 WorldPos, in float3 WorldNormal, in float2 CutoffThresholds, out float3 diffuseColor)
{
    diffuseColor = 0.0f;

#ifndef SHADERGRAPH_PREVIEW
    int lightCount = GetAdditionalLightsCount();

    for (int i = 0; i < lightCount; ++i)
    {
        Light light = GetAdditionalLight(i, WorldPos);

        float3 color = dot(light.direction, WorldNormal);
        color = smoothstep(CutoffThresholds.x, CutoffThresholds.y, color);
        color *= light.color;
        color *= light.distanceAttenuation;

        diffuseColor += color;
    }
#endif
    
}


void AllAdditionalLights_half(in half3 WorldPos, in half3 WorldNormal, half2 CutoffThresholds, out half3 diffuseColor)
{
    diffuseColor = 0.0f;

#ifndef SHADERGRAPH_PREVIEW
    int lightCount = GetAdditionalLightsCount();

    for (int i = 0; i < lightCount; ++i)
    {
        Light light = GetAdditionalLight(i, WorldPos);
        
        float3 color = dot(light.direction, WorldNormal);
        color = smoothstep(CutoffThresholds.x, CutoffThresholds.y, color);
        color *= light.color;
        color *= light.distanceAttenuation;

        diffuseColor += color;
    }
#endif
    
}

#endif // ADDITIONAL_LIGHT_INCLUDED
