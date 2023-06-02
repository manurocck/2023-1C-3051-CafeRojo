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

float Time;

texture Texture;
sampler2D textureSampler = sampler_state
{
    Texture = (Texture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
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
struct VertexShaderInput
{
	float4 Position : POSITION0;
    float4 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
    float4 TextureCoordinate : TEXCOORD0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    // Clear the output
	VertexShaderOutput output = (VertexShaderOutput)0;
    // Model space to World space
    float4 worldPosition = mul(input.Position, World);
    // World space to View space
    float4 viewPosition = mul(worldPosition, View);	
	// View space to Projection space
    output.Position = mul(viewPosition, Projection);
    output.TextureCoordinate = input.TextureCoordinate;

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 textureColor = tex2D(textureSampler, input.TextureCoordinate.xy);

    if(textureColor.a<0.01) discard;

    textureColor = rgba_to_hsva(textureColor);
    textureColor.z -= Time;
    textureColor.a -= Time;
    textureColor = hsva_to_rgba(textureColor);

    return textureColor;
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
