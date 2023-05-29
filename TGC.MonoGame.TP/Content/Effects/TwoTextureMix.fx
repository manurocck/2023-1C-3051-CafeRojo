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

float LerpAmount;

texture Texture;
sampler2D textureSampler = sampler_state
{
    Texture = (Texture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};
texture Texture2;
sampler2D textureSampler2 = sampler_state
{
    Texture = (Texture2);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

float4 rgbToHsv(float4 color) {
  color.r /= 255, color.g /= 255, color.b /= 255;

  float max1 = max(color.r, color.g); 
  float maxOK = max(max1, color.b); 
  float min1 = min(color.r, color.g);
  float minOK = min(min1, color.b);
  float h, s, v = maxOK;

  float d = maxOK - minOK;
  s = maxOK == 0 ? 0 : d / maxOK;

  if (maxOK == minOK) {
    h = 0; // achromatic
  } else {
    if(maxOK == color.r) h = (color.g - color.b) / d + (color.g < color.b ? 6 : 0);
    else if(maxOK == color.g) h = (color.b - color.r) / d + 2;
    else if(maxOK == color.b) h = (color.r - color.g) / d + 4;
    
    h /= 6;
  }
  return float4(h, s, v, color.a);
}

float4 hsvToRgb(float4 colorHSV) {
    float r, g, b;

    float i = floor(colorHSV.r * 6);
    float f = colorHSV.r * 6 - i;
    float p = colorHSV.b * (1 - colorHSV.g);
    float q = colorHSV.b * (1 - f * colorHSV.g);
    float t = colorHSV.b * (1 - (1 - f) * colorHSV.g);
    
    float aMod = fmod(i,6);

    if(aMod == 0){
        r = colorHSV.b, g = t, b = p;
    }else if( aMod == 1){
        r = q, g = colorHSV.b, b = p;
    }else if( aMod == 2){
        r = p, g = colorHSV.b, b = t;
    }else if( aMod == 3){
        r = p, g = q, b = colorHSV.b;
    }else if( aMod == 4){
        r = t, g = p, b = colorHSV.b;
    }else if( aMod == 5){
        r = colorHSV.b, g = p, b = q;
    }

    return float4(r, g, b, colorHSV.a);
}

// a.rgba == a.hsva
float4 LerpHSV (float4 colorA, float4 colorB, float t) {
    // Hue interpolation
    float h;
    float d = colorB.r - colorA.r;
    if (colorA.r > colorB.r)
    {
        // Swap (a.h, b.h)
        float h3 = colorB.r;
        colorB.r = colorA.r;
        colorA.r = h3;

        d = -d;
        t = 1 - t;
    }

    if (d > 0.5) // 180deg
    {
        colorA.r = colorA.r + 1; // 360deg
        h = ( colorA.r + t * (colorB.r - colorA.r) ) % 1; // 360deg
    }
    if (d <= 0.5) // 180deg
    {
        h = colorA.r + t * d;
    }

    // Interpolates the rest
    return float4
    (
        h,            // H
        colorA.g + t * (colorB.g-colorA.g),    // S
        colorA.b + t * (colorB.b-colorA.b),    // V
        colorA.a + t * (colorB.a-colorA.a)    // A
    );
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
    // output.TextureCoordinate = input.TextureCoordinate*2;
    output.TextureCoordinate = input.TextureCoordinate;

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 textureColor  = tex2D(textureSampler, input.TextureCoordinate.xy);
    float4 textureColor2 = tex2D(textureSampler2, input.TextureCoordinate.xy);

    float4 TextureColorHSL  = rgbToHsv(textureColor);
    float4 TextureColorHSL2 = rgbToHsv(textureColor2);

    float4 TextLerpHSV = LerpHSV(TextureColorHSL, TextureColorHSL2, LerpAmount);

    return TextLerpHSV;
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};

