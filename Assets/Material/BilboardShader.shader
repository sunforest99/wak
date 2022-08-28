Shader "Unlit/BilboardShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

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

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4x4 GetRotationMatrix(float xRadian, float yRadian, float zRadian)
            {
                float sina, cosa;
                sincos(xRadian, sina, cosa);

                float4x4 xMatrix;

                xMatrix[0] = float4(1, 0, 0, 0);
                xMatrix[1] = float4(0, cosa, -sina, 0);
                xMatrix[2] = float4(0, sina, cosa, 0);
                xMatrix[3] = float4(0, 0, 0, 1);

                sincos(yRadian, sina, cosa);

                float4x4 yMatrix;

                yMatrix[0] = float4(cosa, 0, sina, 0);
                yMatrix[1] = float4(0, 1, 0, 0);
                yMatrix[2] = float4(-sina, 0, cosa, 0);
                yMatrix[3] = float4(0, 0, 0, 1);

                sincos(zRadian, sina, cosa);

                float4x4 zMatrix;

                zMatrix[0] = float4(cosa, -sina, 0, 0);
                zMatrix[1] = float4(sina, cosa, 0, 0);
                zMatrix[2] = float4(0, 0, 1, 0);
                zMatrix[3] = float4(0, 0, 0, 1);

                return mul(mul(yMatrix, xMatrix), zMatrix);
            }

            v2f vert(appdata v)
            {
                v2f o;
                
                // model matrix의 rotate부분을 yaw(y축)으로만 회전하게 해야함.
                // yaw의 회전량을 알기위해선 basis를 알아야함 forward 벡터로만 알수없음
                // 쿼드는 기본으로 앞면이 z방향이여서 안돌리면됨
                // 스크립트에서하면 오브젝트수만큼 연산되는데 셰이더에서 하면 병렬로 한번에 계산돼서
                // 빠르고 실행시키지 않아도 뷰포트에서 바로 볼수있음
                // + 그냥 카메라 정면만 보게하려면 로테이션행렬 수정안해도된다. translate만 viewing하고
                // + 나머지는 world에 그냥놔두고 projection하면된다.

                //float3 cameraPos = _WorldSpaceCameraPos;
                float3 translated = float3(unity_ObjectToWorld._m03, unity_ObjectToWorld._m13, unity_ObjectToWorld._m23);
                //float3 forward = normalize(cameraPos-translated);
                float3 forward = UNITY_MATRIX_IT_MV[2].xyz;
                float3 up = float3(0,1,0);
                float3 right = cross(forward,up);
                float yaw = atan2(right.x, forward.x) - UNITY_PI / 2;;
                float sina, cosa;
                sincos(yaw, sina, cosa);
                float3x3 transposed = transpose((float3x3)unity_ObjectToWorld);
                // rotate행렬은 basis가 행백터로 나열되어있어서 transpose하고 곱해진 scale을 길이로 찾아낸다.
                float3 scale = float3(length(transposed[0]), length(transposed[1]), length(transposed[2]));
                
                float4x4 yawAndScale;
                yawAndScale[0] = float4(cosa * scale.x, 0, sina, translated.x);
                yawAndScale[1] = float4(0, scale.y, 0, translated.y);
                yawAndScale[2] = float4(-sina, 0, cosa * scale.z, translated.z);
                yawAndScale[3] = float4(0, 0, 0, 1);

                o.vertex = mul(UNITY_MATRIX_VP, mul(yawAndScale, v.vertex));

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
