Shader "FX/ProceduralClouds"
{
    Properties
    {
        _CloudColor("Cloud Color", Color) = (1,1,1,0.6)

        _Coverage("Coverage", Range(0, 1)) = 0.55
        _Softness("Softness", Range(0.001, 0.25)) = 0.06
        _Opacity("Opacity", Range(0, 2)) = 1.0

        _Scale("Base Scale", Range(0.1, 32)) = 4.0
        _DetailScale("Detail Scale", Range(0.5, 96)) = 18.0
        _DetailStrength("Detail Strength", Range(0, 1)) = 0.45

        _WindDirSpeed("Wind Dir (XY) + Speed (Z)", Vector) = (1, 0.2, 0.08, 0)
        _DistortStrength("Distort Strength", Range(0, 1)) = 0.25
        _Seed("Seed", Float) = 0

        _SunDir("Sun Dir (XYZ)", Vector) = (0.6, 0.6, 0.2, 0)
        _SunColor("Sun Color", Color) = (1,1,1,1)
        _LightStrength("Light Strength", Range(0, 2)) = 0.6
        _RimStrength("Rim Strength", Range(0, 2)) = 0.35
        _RimPower("Rim Power", Range(0.5, 8)) = 2.5

        _UseSkyProjection("Use Sky Projection (0/1)", Range(0, 1)) = 1
        _CloudPlaneHeight("Cloud Plane Height", Range(1, 5000)) = 250
        _WorldMappingScale("World Mapping Scale", Range(0.0001, 0.05)) = 0.004
        _MaxProjectionDistance("Max Projection Distance", Range(100, 200000)) = 50000

        _HorizonFadeY("Horizon Fade Start (DirY)", Range(0, 1)) = 0.12
        _HorizonFadeWidthY("Horizon Fade Width (DirY)", Range(0.001, 1)) = 0.20

        _EdgeFade("Edge Fade (UV)", Range(0, 0.35)) = 0.0
    }

        SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _CloudColor;

            float _Coverage;
            float _Softness;
            float _Opacity;

            float _Scale;
            float _DetailScale;
            float _DetailStrength;

            float4 _WindDirSpeed;
            float _DistortStrength;
            float _Seed;

            float4 _SunDir;
            fixed4 _SunColor;
            float _LightStrength;
            float _RimStrength;
            float _RimPower;

            float _UseSkyProjection;
            float _CloudPlaneHeight;
            float _WorldMappingScale;
            float _MaxProjectionDistance;

            float _HorizonFadeY;
            float _HorizonFadeWidthY;

            float _EdgeFade;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            float Hash21(float2 p)
            {
                float3 p3 = frac(float3(p.x, p.y, p.x) * 0.1031);
                p3 += dot(p3, p3.yzx + 33.33);
                return frac((p3.x + p3.y) * p3.z);
            }

            float2 SeedOffset2()
            {
                float s0 = frac(_Seed * 0.1031);
                float s1 = frac((_Seed + 19.19) * 0.13787);
                return (float2(s0, s1) * 512.0) + 7.13;
            }

            float NoiseValue(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                float2 u = f * f * (3.0 - 2.0 * f);

                float a = Hash21(i + float2(0.0, 0.0));
                float b = Hash21(i + float2(1.0, 0.0));
                float c = Hash21(i + float2(0.0, 1.0));
                float d = Hash21(i + float2(1.0, 1.0));

                float x1 = lerp(a, b, u.x);
                float x2 = lerp(c, d, u.x);
                return lerp(x1, x2, u.y);
            }

            float Fbm(float2 p)
            {
                float v = 0.0;
                float a = 0.5;
                float2 shift = float2(37.0, 17.0);

                v += a * NoiseValue(p); p = p * 2.0 + shift; a *= 0.5;
                v += a * NoiseValue(p); p = p * 2.0 + shift; a *= 0.5;
                v += a * NoiseValue(p); p = p * 2.0 + shift; a *= 0.5;
                v += a * NoiseValue(p); p = p * 2.0 + shift; a *= 0.5;
                v += a * NoiseValue(p);

                return v;
            }

            float SampleCloudDensity(float2 p, float t)
            {
                float2 seedOfs = SeedOffset2();

                float2 windDir = normalize(_WindDirSpeed.xy + 1e-5);
                float windSpeed = _WindDirSpeed.z;

                float2 pp = p + seedOfs;

                float2 p0 = pp * _Scale + windDir * (t * windSpeed);

                float2 warpP = (pp * (_Scale * 0.6)) + windDir.yx * (t * windSpeed * 0.8) + 11.3;
                float warp = Fbm(warpP) * 2.0 - 1.0;
                p0 += warp * _DistortStrength;

                float baseN = Fbm(p0);

                float2 p1 = (pp * _DetailScale) + windDir * (t * windSpeed * 1.3) + 53.7;
                float detailN = Fbm(p1);

                float d = baseN + (detailN - 0.5) * _DetailStrength;
                return saturate(d);
            }

            float EdgeFadeMaskUv(float2 uv)
            {
                float e = min(min(uv.x, uv.y), min(1.0 - uv.x, 1.0 - uv.y));
                float w = max(1e-5, _EdgeFade);
                return (_EdgeFade <= 0.0) ? 1.0 : smoothstep(0.0, w, e);
            }

            float HorizonFadeMaskDirY(float dirY)
            {
                float w = max(1e-5, _HorizonFadeWidthY);
                return smoothstep(_HorizonFadeY, _HorizonFadeY + w, saturate(dirY));
            }

            float SoftLimitDistance(float dist, float maxDist)
            {
                float m = max(1.0, maxDist);
                return (m * dist) / (dist + m);
            }

            float2 GetCloudPFromSky(float3 worldPos)
            {
                float3 dir = normalize(worldPos - _WorldSpaceCameraPos);
                float denom = max(1e-4, dir.y);

                float dist = _CloudPlaneHeight / denom;
                dist = SoftLimitDistance(dist, _MaxProjectionDistance);

                float3 hit = _WorldSpaceCameraPos + dir * dist;
                return hit.xz * _WorldMappingScale;
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float t = _Time.y;

                float3 dir = normalize(i.worldPos - _WorldSpaceCameraPos);

                float2 pUv = i.uv;
                float2 pSky = GetCloudPFromSky(i.worldPos);

                float useSky = step(0.5, _UseSkyProjection);
                float2 p = lerp(pUv, pSky, useSky);

                float d = SampleCloudDensity(p, t);

                float threshold = lerp(0.88, 0.22, saturate(_Coverage));
                float edge = max(0.0005, _Softness);

                float a = smoothstep(threshold, threshold + edge, d);
                a *= _Opacity * _CloudColor.a;

                float2 eps = float2(0.0025, 0.0025);
                float dX = SampleCloudDensity(p + float2(eps.x, 0.0), t) - SampleCloudDensity(p - float2(eps.x, 0.0), t);
                float dY = SampleCloudDensity(p + float2(0.0, eps.y), t) - SampleCloudDensity(p - float2(0.0, eps.y), t);

                float3 n = normalize(float3(-dX, -dY, 1.0));
                float3 l = normalize(_SunDir.xyz + 1e-5);

                float ndl = saturate(dot(n, l));
                float lightTerm = ndl * _LightStrength;
                float rim = pow(saturate(1.0 - ndl), _RimPower) * _RimStrength;

                float fadeUv = EdgeFadeMaskUv(i.uv);
                float fadeSky = HorizonFadeMaskDirY(dir.y);
                float fade = lerp(fadeUv, fadeSky, useSky);

                a *= fade;

                float3 col = _CloudColor.rgb;
                col *= (1.0 + lightTerm);
                col = lerp(col, col + _SunColor.rgb * rim, saturate(a));

                return fixed4(col, saturate(a));
            }
            ENDCG
        }
    }
}
