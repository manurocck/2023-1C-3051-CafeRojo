#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// Custom Effects - https://docs.monogame.net/articles/content/custom_effects.html
// High-level shader language (HLSL) - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl
// Programming guide for HLSL - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-pguide
// Reference for HLSL - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-reference
// HLSL Semantics - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-semantics

float4x4 World;
float4x4 View;
float4x4 Projection;

// float MetrosAncho;
// float MetrosLargo;

texture Texture;
sampler2D TextureSampler = sampler_state
{
    Texture = (Texture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

texture Filter;
sampler2D FilterSampler = sampler_state
{
    Texture = (Filter);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Mirror;
    AddressV = Mirror;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
    float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
    float2 TextureCoordinate : TEXCOORD0;
};
float4 rgba_to_hsva(float4 rgba)
{
    float r = rgba.r;
    float g = rgba.g;
    float b = rgba.b;
    float a = rgba.a;

    // Convert RGB to HSL
    float cmax = max(max(r, g), b);
    float cmin = min(min(r, g), b);
    float delta = cmax - cmin;

    float h = 0.0;
    float s = 0.0;
    float v = cmax;

    if (delta != 0.0)
    {
        // Calculate Hue
        if (cmax == r)
            h = (g - b) / delta;
        else if (cmax == g)
            h = 2.0 + (b - r) / delta;
        else if (cmax == b)
            h = 4.0 + (r - g) / delta;

        h /= 6.0;

        // Calculate Saturation
        s = delta / cmax;
    }
    return float4(h, s, v, a);
}
float4 hsva_to_rgba(float4 hsva)
{
    float h = hsva.x;
    float s = hsva.y;
    float v = hsva.z;
    float a = hsva.w;

    // Convert HSL to RGB
    float4 rgba = float4(0.0, 0.0, 0.0, a);

    if (s == 0.0)
    {
        // If saturation is 0, color is grayscale
        rgba.rgb = v.xxx;
    }
    else
    {
        float h6 = h * 6.0;
        float i = floor(h6);
        float f = h6 - i;
        float p = v * (1.0 - s);
        float q = v * (1.0 - s * f);
        float t = v * (1.0 - s * (1.0 - f));

        if (i == 0.0)
            rgba.rgb = float3(v, t, p);
        else if (i == 1.0)
            rgba.rgb = float3(q, v, p);
        else if (i == 2.0)
            rgba.rgb = float3(p, v, t);
        else if (i == 3.0)
            rgba.rgb = float3(p, q, v);
        else if (i == 4.0)
            rgba.rgb = float3(t, p, v);
        else
            rgba.rgb = float3(v, p, q);
    }

    return rgba;
}
VertexShaderOutput MainVS(in VertexShaderInput input)
{
    // Clear the output
	VertexShaderOutput output = (VertexShaderOutput)0;
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    output.TextureCoordinate = input.TextureCoordinate;
	
    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR0
{
    float4 colorTexture = tex2D(TextureSampler, input.TextureCoordinate);
    
    input.TextureCoordinate.x = input.TextureCoordinate.x*0.25;
    input.TextureCoordinate.y = input.TextureCoordinate.y;
    float4 colorOverlap = tex2D(FilterSampler, input.TextureCoordinate);

    if(colorOverlap.r<0.2){
        discard;
    }
    return float4(colorTexture.rgb,1);
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
