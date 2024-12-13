Shader"Unlit/ShellTexture"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_SecondaryColor ("SecondaryColor", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_WindTex ("WindTex", 2D) = "white" {}
		_GrassMask("Grass Mask", 2D) = "white" {}

		_ParticleTexture ("ParticleTexture", 2D) = "white" {}
		_ScrollSpeed ("ScrollSpeed", Float) = 0.2
		_WindStrength ("WindStrength", Float) = 0.2
		_OrthographicCamSize ("OrthographicCamSize", Float) = 15
		_Position ("Position", Vector) = (0, 0, 0)
		_EM ("EDIT MODE TOGGLE", Float) = 0
		

	}  
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		Cull Off

Pass
{
CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"
			
struct v2f
{
	float2 uv : TEXCOORD0;
	float4 vertex : SV_POSITION;
	float4 color : COLOR;
	float4 worldPos : TEXCOORD1;
};

fixed4 _Color;
fixed4 _SecondaryColor;
sampler2D _MainTex;
float4 _MainTex_ST;
sampler2D _WindTex;
float4 _WindTex_ST;
sampler2D _ParticleTexture;
float4 _ParticleTexture_ST;
sampler2D _GrassMask;
float4 _GrassMask_ST;
float _ScrollSpeed;
float _WindStrength;
float _OrthographicCamSize;
float3 _Position;
float _EM;

struct DrawVertex
{
	float3 position;
	float3 normal;
	float2 uv;
	float4 vertexColor;
};

struct DrawTriangle
{
	DrawVertex drawVertices[3];
};

StructuredBuffer<DrawTriangle> _DrawTrianglesBuffer;
float proceduralNoise(float2 p)
{
    // Use a combination of dot products and sine to create pseudo-random noise
    return frac(sin(dot(p, float2(12.9898, 78.233))) * 43758.5453);
}

float smoothNoise(float2 p)
{
    // Get integer and fractional parts of coordinates
    float2 i = floor(p);
    float2 f = frac(p);
    
    // Smooth interpolation
    float2 u = f * f * (3.0 - 2.0 * f);
    
    // Sample and interpolate noise at corners
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
			

v2f vert (uint vertexID : SV_VertexID)
{
	v2f o;
	DrawTriangle tri = _DrawTrianglesBuffer[vertexID / 3];
	DrawVertex v = tri.drawVertices[vertexID % 3];
				

	o.worldPos = mul(unity_ObjectToWorld, float4(v.position, 1));
	o.vertex = UnityObjectToClipPos(v.position);
	o.uv = v.uv * _MainTex_ST.xy;;
	o.color = v.vertexColor;
	return o;
}

fixed4 frag (v2f i) : SV_Target
{
	// sample the texture

    float noise = fractalNoise(i.worldPos.xz * 0.5f, 0.5f, 1);
	
    float wind = tex2D(_WindTex, (i.worldPos.xz * 0.1) + float2(0, _Time.y * _ScrollSpeed)).x;
    wind =  (wind - 0.5) * 0.5;
	
	//float tex = tex2D(_MainTex, i.uv).x;	
    float mask = tex2D(_GrassMask, i.uv * _GrassMask_ST);
    float tex = tex2D(_MainTex, i.uv + float2(( wind * 0.05 * i.color.w * i.color.w), ( wind * 0.1 * i.color.w * i.color.w))).
    x;
    float Ntex = tex2D(_MainTex, i.uv * noise).x;
    clip(tex - i.color.w);
    clip(tex - mask);
				
				
	//this cloudShadows is the cloud shadows
    float cloudShadows = tex2D(_WindTex, (i.uv * _WindTex_ST.xy) + _WindTex_ST.zw + (_Time.x)).x;
    //float4 col = lerp(_SecondaryColor, _Color, (Ntex / 4 + (tex / (fractalNoise(i.worldPos.yz, 0.5f, 1) * 8)) + fractalNoise(i.uv, 0.5f, 1) / 4 + //cloudShadows / 2));
	
    float distanceToPlayer = distance(i.worldPos.xyz, _Position);
    float noiseScale = lerp(0.5f, 0.01f,
    saturate(distanceToPlayer / 50));
	
    float editModeInfluence = lerp(1, noiseScale, _EM);
	fixed4 final = lerp(_SecondaryColor, _Color, fractalNoise(i.worldPos.xz * 0.005, 0.5f, 1) * 0.3f + cloudShadows * 0.2f + tex * editModeInfluence);
	
    return final ;
	
	

	
}
ENDCG
		}
	}
}
