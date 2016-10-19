texture2D Restriction;
float4 Color;

sampler2D RestSamp = sampler_state {
    texture = <Restriction>;
    AddressU  = CLAMP;
    AddressV = CLAMP;
    FILTER = MIN_MAG_LINEAR_MIP_POINT;
};

float4 PixelShaderFunction(float2 uv : TEXCOORD0) : COLOR0
{
	float4 b = tex2D(RestSamp, uv);
	if(b.a > 0)
		return Color;
	else
		return float4(0, 0, 0, 0);
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
