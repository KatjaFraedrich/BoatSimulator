Shader "Custom/WaveShaderTransparent"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Speed("Speed",Float) = 1.0
        _OffSet("Offset",Float) = 1.0
        _Amplitude("Amplitude",Float) = 1.0
        _WaveTime("WaveTime",Float) = 0
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _NormalIntensity("Normal Intensity", Float) = 1.0 // Multiplier for normal map intensity
        [Toggle] _SimpleWaves("Use simple Waves",Float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha //for transparency

        ZWrite On

        ZTest LEqual

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha:fade vertex:vert  

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpMap; // Normal map texture

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _Speed;
        float _OffSet;
        float _Amplitude;
        float _NormalIntensity; // Multiplier for normal map intensity
        float _SimpleWaves;
        float _WaveTime;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)
         
        void vert(inout appdata_full v, out Input o)
        {
            float3 worldSpace = mul(unity_ObjectToWorld, v.vertex).xyz;
            o.worldPos = worldSpace;
            o.uv_MainTex = v.texcoord;
            float waveOffSet = _WaveTime * _Speed + worldSpace.x * _OffSet; //
            if (_SimpleWaves>0.5)
            {
                float sinOffSet = sin(waveOffSet) * _Amplitude;
                v.vertex.y += sinOffSet; //mul(unity_WorldToObject, float4(worldSpace.x,sinOffSet,worldSpace.z, 1.0));

            }
            else
            {
                
                float3 offSetRotate = float3(sin(waveOffSet) * _Amplitude, cos(waveOffSet) * _Amplitude, 0);
            
                        //v.vertex.xyz += v.normal * _Amount;
                v.vertex.xyz += mul(unity_WorldToObject, offSetRotate);
            }
            
            
        }

            
        void surf(Input IN, inout SurfaceOutputStandard o)
        {
                    // Albedo comes from a texture tinted by color
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
                    // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
    
            // Normal mapping
            float2 modifiedUV =  IN.worldPos.xz * _NormalIntensity;
            o.Normal =  UnpackNormal(tex2D(_BumpMap,modifiedUV));
        }
        ENDCG
    }
FallBack"Diffuse"
}
