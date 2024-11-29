Shader"Custom/GrassShellLit"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _SecondaryColor ("SecondaryColor", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _WindTex ("WindTexture", 2D) = "white" {}
        _ParticleTexture ("ParticleTexture", 2D) = "white" {}
        _ScrollSpeed ("ScrollSpeed", Float) = 0.2
        _WindStrength ("WindStrength", Float) = 0.2
        _OrthographicCamSize ("OrthographicCamSize", Float) = 15
        _Position ("Position", Vector) = (0, 0, 0)
        _EM ("EDIT MODE TOGGLE", Float) = 0
        
        // New lighting and shadow properties
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    
    SubShader
    {
        Tags { 
            "RenderType"="Opaque" 
        }
        LOD 300
        Cull Off

        CGPROGRAM
        // Add forward rendering base pass, shadows, and physically based rendering
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0
        

struct Input
{
    float2 uv_MainTex;
    float4 color;
    float3 worldPos;
};

        // Existing noise functions from the original shader
float proceduralNoise(float2 p)
{
    return frac(sin(dot(p, float2(12.9898, 78.233))) * 43758.5453);
}

float smoothNoise(float2 p)
{
    float2 i = floor(p);
    float2 f = frac(p);
            
    float2 u = f * f * (3.0 - 2.0 * f);
            
    return lerp(
                lerp(proceduralNoise(i), proceduralNoise(i + float2(1.0, 0.0)), u.x),
                lerp(proceduralNoise(i + float2(0.0, 1.0)), proceduralNoise(i + float2(1.0, 1.0)), u.x),
                u.y
            );
}

float fractalNoise(float2 p, float persistence, int octaves)
{
    float total = 0.0;
    float frequency = 1.0;
    float amplitude = 1.0;
    float maxValue = 0.0;
            
    for (int i = 0; i < octaves; i++)
    {
        total += smoothNoise(p * frequency) * amplitude;
                
        maxValue += amplitude;
                
        amplitude *= persistence;
        frequency *= 2.0;
    }
            
    return total / maxValue;
}

        // Shader properties
fixed4 _Color;
fixed4 _SecondaryColor;
sampler2D _MainTex;
sampler2D _WindTex;
sampler2D _ParticleTexture;
float _ScrollSpeed;
float _WindStrength;
float _OrthographicCamSize;
float3 _Position;
float _EM;
float _Glossiness;
float _Metallic;

void surf(Input IN, inout SurfaceOutputStandard o)
{
            // Wind and noise calculations
    float noise = fractalNoise(IN.worldPos.xz * 0.5f, 0.5f, 1);
    float tex = tex2D(_MainTex, IN.uv_MainTex).x;
    float windTex = tex2D(_WindTex, IN.uv_MainTex + _Time.x).x;
            
            // Distance and edit mode influence
    float distanceToPlayer = distance(IN.worldPos.xyz, _Position);
    float noiseScale = lerp(0.5f, 0.01f, saturate(distanceToPlayer / 50));
    float editModeInfluence = lerp(1, noiseScale, _EM);
            
            // Color calculation
    fixed4 finalColor = lerp(_SecondaryColor, _Color,
                fractalNoise(IN.worldPos.xz * 0.005, 0.5f, 1) * 0.3f +
                windTex * 0.2f +
                tex * editModeInfluence
            );

            // Set surface properties
    o.Albedo = finalColor.rgb;
    o.Metallic = _Metallic;
    o.Smoothness = _Glossiness;
    o.Alpha = finalColor.a;
            
            // Clip based on original shader's alpha test
    clip(tex - IN.color.w);
}
        ENDCG
    }
FallBack"Diffuse"
}