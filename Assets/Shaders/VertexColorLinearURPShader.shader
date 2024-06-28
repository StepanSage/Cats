Shader "Custom/VertexColorLinearURP" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
    }

        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 100

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                    float4 color : COLOR;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float4 vertex : SV_POSITION;
                    fixed4 color : COLOR;
                    float2 uv : TEXCOORD0;
                };

                sampler2D _MainTex;
                fixed4 _Color;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.color = v.color;
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    fixed4 texColor = tex2D(_MainTex, i.uv);
                    fixed4 vertexColor = i.color * _Color;
                    vertexColor.rgb = pow(vertexColor.rgb, 2.2); // Apply gamma correction
                    return texColor * vertexColor;
                }

                ENDCG
            }
        }
            FallBack "Diffuse"
}