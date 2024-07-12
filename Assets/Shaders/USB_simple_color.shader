Shader "Unlit/USB_simple_color"
{
    Properties
    {
        [Header(Texture Properties)]
        [Space(15)]
        _MainTex ("Texture", 2D) = "white" {}
        _Reflection ("Reflection", Cube) = "black"{}
        _3DTexture ("3D Texture", 3D) = "white"{}

        [Header(Specular Properties)]
        [Space(15)]    
        _Specular ("Specular", Range(0.0, 1.1)) = 0.3
        _Factor ("Color Factor", Float) = 0.3
        _Cid ("Color id", Int) = 2
        _Color ("Tint", Color) = (1,1,1,1)
        _VPos ("Vertex Position", Vector) = (0,0,0,1)

        //[Header(My Header)] _Property("Prop", float) = 1
        [Header(Settings)]
        [Space(25)]
        [Toggle] _Enable ("Enable?", Float) = 0
        [KeywordEnum(Off, Red, Blue)]
        _Options ("Color Options", Float) = 0
        [Enum(Off, 0, Front, 1, Back,  2 )]
        _Face ("Face Culling", Float) = 0 

        [Header(Power Settings)]
        [Space(20)]
        [PowerSlider(3.0)]
        _Brightness ("Brightness", Range(0.01, 1)) = 0.08
        [IntRange]
        _Samples ("Samples", Range (0, 255)) = 100 



    }
    SubShader
    {
        Tags { "RenderType"="Opaque"}
        LOD 100
        Cull[_Face]
        Blend SrcAlpha OneMinusSrcAlpha
        

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            #pragma shader_feature _ENABLE_ON
            #pragma multi_compile _OPTIONS_OFF _OPTIONS_RED _OPTIONS_BLUE

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 pos : POSITION;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                
            };

           // Texture2D _MainTex;
            sampler2D _MainTex; 
            SamplerState sampler_MainTex;
            float4 _MainTex_ST;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            /*
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            */
            half4 frag (v2f i) : SV_TARGET
            {
                
                half4 col = tex2D(_MainTex, i.uv);
               // half4 col = _MainTex.Sample(sampler_MainTex, i.uv);
                
                #if _ENABLE_ON
                    return col;
                #endif

                #if _OPTIONS_OFF 
                    return col * _Color; 
                #elif _OPTIONS_RED
                    return col * float4(1,0,0,1);
                #elif _OPTIONS_BLUE
                    return col * float4(0,0,1,1);
                #endif
               
            }


            ENDCG
        }
    }
}
