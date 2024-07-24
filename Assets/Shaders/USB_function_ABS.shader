Shader "Unlit/USB_function_ABS"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Rotation ("Rotation", Range(0,360)) = 0    
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Rotation;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            /*
            void Unity_Rotate_Degrees_float
            ( 
               float2 UV, 
               float2 Center, 
               float2 Rotation, 
               out float2 Out)    
            {
                Rotation = Rotation * (Unity_PI/180.0f);
                UV -= Center; 
                float s = sin(Rotation);
                float c = cos(Rotation); 
                float2x2 rMatrix = float2x2 (c,-s,c);
                rMatrix *= 0.5; 
                rMatrix += 0.5; 
                rMatrix = rMatrix * 2 - 1; 
                UV.xy = mul(UV.yx, rMatrix);
                UV += Center;
                Out = UV;
            }*/

            fixed4 frag (v2f i) : SV_Target
            {   /*
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;*/
                float u = abs(i.uv.x - 0.5);
                float v = abs(i.uv.y - 0.5);

                float rotation = _Rotation; 
                float center = 0.5f; 
                float2 uv = 0; 

                //Unity_Rotate_Degrees_float(float2(u,v), center, rotation,uv);
               // fixed4 col = tex2d(_MainTex, uv);
              //  UNITY_APPLY_FOG(i.fogCoord, col);
               // return col; 

                fixed col = tex2D (_MainTex, float2(u, v));
               // UNITY_APPLY_FOG(i.fogCoord, col);
                return col;


            }
            ENDCG
        }
    }
}
