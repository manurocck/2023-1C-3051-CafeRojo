#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 matInverseTransposeWorld;

#define MAX_LIGHTS 6 // Define the maximum number of lights

float3 lightPositions[MAX_LIGHTS]; // Array of light positions

float3 ambientColor;
float3 diffuseColor;
float3 specularColor;
float KAmbient;
float KDiffuse;
float KSpecular;
float shininess;

float3 eyePosition; // Camera position


texture Texture;
sampler2D textureSampler = sampler_state
{
    Texture = (Texture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
    float4 Normal : NORMAL;
    float2 TextureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
    float2 TextureCoordinates : TEXCOORD0;
    float4 WorldPosition : TEXCOORD1;
    float4 Normal : TEXCOORD2;    
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

    // Model space to World space
    float4 WorldPosition = mul(input.Position, World);
    output.WorldPosition = mul(input.Position, World);
    // World space to View space
    float4 ViewPosition = mul(WorldPosition, View);	
	// View space to Projection space
    output.Position = mul(ViewPosition, Projection);
    output.TextureCoordinates = input.TextureCoordinates;

    output.Normal = mul(input.Normal, matInverseTransposeWorld);
	
	return output;
}

float4 MainPS(VertexShaderOutput input) : SV_Target
{
    // Base vectors
    float3 viewDirection = normalize(eyePosition - input.WorldPosition.xyz);
    float3 halfVector;

    float4 finalColor = float4(0, 0, 0, 0); // Accumulate light contributions

    for (int i = 0; i < MAX_LIGHTS; i++)
    {
        float3 lightPosition = lightPositions[i]; // Get the light position

        // Calculate the light direction
        float3 lightDirection = normalize(lightPosition - input.WorldPosition.xyz);

        // Calculate the half vector
        halfVector = normalize(lightDirection + viewDirection);

        // Get the texture texel
        float4 texelColor = tex2D(textureSampler, input.TextureCoordinates);

        // Calculate the diffuse light
        float NdotL = saturate(dot(input.Normal.xyz, lightDirection));
        float3 diffuseLight = KDiffuse * diffuseColor * NdotL;

        // Calculate the specular light
        float NdotH = dot(input.Normal.xyz, halfVector);
        float3 specularLight = sign(NdotL) * KSpecular * specularColor * pow(saturate(NdotH), shininess);

        // Accumulate light contributions
        finalColor.rgb += saturate(ambientColor * KAmbient + diffuseLight) * texelColor.rgb + specularLight;
    }

    finalColor.a = 1; // Set the alpha value

    return finalColor;
}




technique BasicColorDrawing
{
	pass Pass0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};