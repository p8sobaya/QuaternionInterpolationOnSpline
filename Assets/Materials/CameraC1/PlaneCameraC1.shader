Shader "Unlit/PlaneCameraC1"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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

            sampler2D _MainTex;
            float4 _MainTex_ST;

            int mode;
            float _size;

            bool OnPath(float2 p) {
                p /= _size;
                if (mode == 10) {
                    float upperDist = length(p - float2(0.0, 1.0));
                    float lowerDist = length(p - float2(0.0, -1.0));
                    if (upperDist > 0.99 && upperDist < 1.01 && p.y > 1.0) {
                        return true;
                    }
                    else if (lowerDist > 0.99 && lowerDist < 1.01 && p.y < -1.0) {
                        return true;
                    }
                    else if (abs(p.x - 1.0) < 0.01 && abs(p.y - 0.0) < 1.0) {
                        return true;
                    }
                    else if (abs(p.x + 1.0) < 0.01 && abs(p.y - 0.0) < 1.0) {
                        return true;
                    }
                    else return false;
                }

                if (mode = 11) {
                    float sq2 = 1.41421356;
                    float distUp = length(p - float2(0.0, - sq2 * 0.5));
                    float distLeft = length(p - float2(-sq2 * 0.5, 0.0));
                    float distDown = length(p - float2(0.0, sq2 * 0.5));
                    float distRight = length(p - float2(sq2 * 0.5, 0.0));
                    if (distUp > 1.99 && distUp < 2.01 && p.y > sq2 * 0.5) {
                        return true;
                    }
                    else if (distDown > 1.99 && distDown < 2.01 && p.y < -sq2 * 0.5) {
                        return true;
                    }
                    else if (distLeft > 0.99 && distLeft < 1.01 && sq2 * 0.5 && p.x < -sq2) {
                        return true;
                    }
                    else if (distRight > 0.99 && distRight < 1.01 && sq2 * 0.5 && p.x > sq2) {
                        return true;
                    }
                    else return false;

                }

                else return false;

            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = float4(0.5, 0.5, 0.5, 1.0);
            float2 p = i.uv.xy * 20.0 - float2(10.0, 10.0);
            col = OnPath(p / _size) ? float4(1.0, 0.0, 0.0, 1.0) : col;


                return col;
            }
            ENDCG
        }
    }
}
