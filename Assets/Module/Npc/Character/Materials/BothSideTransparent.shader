Shader "Custom/BothSideTransparent" 
{ 
	Properties 
	{
		_Color ("Front Main Color", Color) = (1,1,1,1)
		_MainTex ("Front Base (RGB) Trans (A)", 2D) = "white" {}
	}

	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 200
		Cull back
		
		CGPROGRAM
		#pragma surface surf BothSideTransparent alpha

		sampler2D _MainTex;
		fixed4 _Color;
		inline fixed4 LightingBothSideTransparent(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			fixed4 Col;
			Col.rgb = s.Albedo * atten * _LightColor0.rgb;
			Col.a = s.Alpha;
			return Col;
		}
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
		
		Cull front
		
		CGPROGRAM
		#pragma surface surf BothSideTransparent alpha
		sampler2D _MainTex;
		fixed4 _Color;
		inline fixed4 LightingBothSideTransparent(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			fixed4 Col;
			Col.rgb = s.Albedo * atten * _LightColor0.rgb;
			Col.a = s.Alpha;
			return Col;
		}
		struct Input
		{
			float2 uv_MainTex;
		};
		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a; 
		}
		ENDCG
		}
	Fallback "Transparent/VertexLit"
}
