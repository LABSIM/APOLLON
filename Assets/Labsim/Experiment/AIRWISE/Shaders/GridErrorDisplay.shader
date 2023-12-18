Shader "Custom/GridErrorDisplay"
{
    Properties
    {
        _Blend("Relative Error", float) = 0.23
        _ErrorColor("Error Color", Color) = (0,0,1,1)
        _DefaultColor("Default Color", Color) = (0,1,0,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _MaskTex("Maks (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags {"IgnoreProjector" = "True" "RenderType" = "Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        //Cull front
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
        };

        float _Blend;
        fixed4 _ErrorColor;
        fixed4 _DefaultColor;

        sampler2D _MainTex;
        sampler2D _MaskTex;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // COmpute albedo comes from mask texture
            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex) * tex2D(_MaskTex, IN.uv_MainTex) * lerp(_DefaultColor, _ErrorColor, _Blend);
            o.Albedo = tex.rgb;

            // Keep alpha from main texture
            o.Alpha = tex2D(_MainTex, IN.uv_MainTex).a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}