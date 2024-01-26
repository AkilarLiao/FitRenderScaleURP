Shader "Unlit/GammaAlphaQuad"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags 
        {            
            "Queue" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }
        LOD 100

        Pass
        {
            Tags{"LightMode" = "ConvertGamma"}
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            HLSLPROGRAM
            #pragma vertex VertexProgram
            #pragma fragment FragmentProgram
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            //#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

            struct VertexInput
            {
                float4 positionOS : POSITION;
                float2 texcoordOS : TEXCOORD0;
            };

            struct VertexOutput
            {
                real2 baseUV : TEXCOORD0;
                float4 clipPosition : SV_POSITION;
            };

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            CBUFFER_END

            VertexOutput VertexProgram(VertexInput input)
            {
                VertexOutput output;
                output.clipPosition = TransformObjectToHClip(input.positionOS.xyz);
                output.baseUV = TRANSFORM_TEX(input.texcoordOS, _MainTex);
                return output;
            }

            //bcgCol 。rgb = LinearToGammaSpace(bcgCol.rgb);
            //uiCol 。rgb = LinearToGammaSpace(uiCol.rgb);
            //bcgCol 。rgb = bcgCol 。rgb * (1 - uiCol.a) + uiCol.a ) RGB; // 預乘 alpha
            //bcgCol 。rgb = GammaToLinearSpace(bcgCol.rgb);
            //返回bcgCol ；

            //但這還不是全部。從技術上講，UI 仍然在線性空間中渲染，只是針對 sRGB 目標。
            //當您將項目設置為線性顏色空間到 sRGB 渲染紋理進行渲染時，我相信著色器和混合
            //仍然在線性空間中運行，GPU 只是在讀取時（在混合）並寫入（混合後）渲染紋理。
            //這處理起來有點煩人。據說你可以向你的 UI 相機添加一個腳本來設置GL.sRGBWrite
            //= true OnPreRender 並將其關閉 OnPostRender，但說實話我從來沒有讓它工作過。

            half4 FragmentProgram(VertexOutput input) : SV_Target
            {
                half4 resultColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.baseUV);
                return half4(1.0, 0.0, 0.0, resultColor.a);
            }
            ENDHLSL
        }
    }
}
