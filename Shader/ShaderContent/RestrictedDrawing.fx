sampler firstSampler;

float4 RestrictedPaintig(float2 p: TEXCOORD0, float4 c: COLOR0) : COLOR0
{
	float4 b = tex2D(firstSampler, p);
	if(b.a > 0)
		return c;
	else
		return float4(0, 0, 0, 0);
}
float4 BlurredPainting(float2 p: TEXCOORD0) : COLOR0
{
	float4 v = float4(0,0,0,0);
	float b = 1;
	int n = 15;
	for(int i = 0; i < n; i++)
	{
		v += tex2D(firstSampler, p + float2(i * 0.01, 0)) * b;
		b *= 0.7;
	}
	return v / 3;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 BlurredPainting();
    }
}
