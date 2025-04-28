Shader "Custom/VignetteBlurShader"
{
    Properties
    {
        _VignetteIntensity ("Vignette Intensity", Range(0, 1)) = 0.5
        _BlurStrength ("Blur Strength", Range(0, 5)) = 1.5
        _MainTex ("Main Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _VignetteIntensity;
            float _BlurStrength;
            float4 _MainTex_TexelSize;

            fixed4 frag(v2f_img i) : SV_Target
            {
                // Basic blur
                float2 uv = i.uv;
                fixed4 col = tex2D(_MainTex, uv);
                col += tex2D(_MainTex, uv + float2(_MainTex_TexelSize.x * _BlurStrength, 0));
                col += tex2D(_MainTex, uv - float2(_MainTex_TexelSize.x * _BlurStrength, 0));
                col += tex2D(_MainTex, uv + float2(0, _MainTex_TexelSize.y * _BlurStrength));
                col += tex2D(_MainTex, uv - float2(0, _MainTex_TexelSize.y * _BlurStrength));
                col /= 5.0;

                // Vignette
                float2 centeredUV = uv - 0.5;
                float vignette = 1.0 - smoothstep(0.2, 0.5, length(centeredUV));
                col.rgb *= lerp(1.0, vignette, _VignetteIntensity);

                return col;
            }
            ENDCG
        }
    }
}