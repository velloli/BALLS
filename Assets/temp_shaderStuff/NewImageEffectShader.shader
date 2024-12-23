Shader "Custom/NewImageEffectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DitherTex ("Texture", 2D) = "white" {}
		_Position ("Position", Vector) = (0, 0, 0)


    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

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
		

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _DitherTex;
            float4 _DitherTex_ST;
            float3 _Position;


            fixed4 frag (v2f i) : SV_Target
            {
                float downscaleFactor = 250.0; // Adjust this to control downscaling intensity
                float2 downscaledUV = floor(i.uv * downscaleFactor) / downscaleFactor;
                // Sample the texture at the downscaled UV coordinates
                fixed4 downscaledCol = tex2D(_MainTex, downscaledUV);
    
                fixed4 col = tex2D(_MainTex, i.uv);
    //return col;
                float lum = dot(downscaledCol, float3(0.699f, 0.687f, 0.614f));
                fixed4 dith = tex2D(_DitherTex, i.uv*10);
                float noise = fractalNoise(i.uv *1000, 0.5f, 1); 
    //return noise*dith;
                float thresholdLum = dot(noise, float3(0.1f, 0.1f, 0.1f));
                float rampVal = lum < thresholdLum ? thresholdLum - lum : 1.0f;
                //float3 rgb = tex2D(_ColorRampTex, float2(rampVal, 0.5f));
                //return rampVal;
                // just invert the colors
                //col.rgb = 1 - col.rgb;
    
                
                // Upscale by sampling the original texture with the original UV coordinates
                fixed4 originalCol = tex2D(_MainTex, i.uv);
                //return originalCol;
                // Blend or combine the downscaled and original textures as needed
                // For example, a simple interpolation:
                float blend = 0.5; // Adjust blend factor as desired
                return downscaledCol;
                fixed4 finalColor = lerp(originalCol, downscaledCol, blend);

                return finalColor;
    //return rampVal;
                //return downscaledCol * (rampVal + 0.5);
    //return col;
    
    
    
    
            }
            ENDCG
        }
    }
}
