#version 330
 
// shader inputs
in vec4 positionWorld;              // fragment position in World Space
in vec4 normalWorld;                // fragment normal in World Space
in vec2 uv;                         // fragment uv texture coordinates
uniform sampler2D diffuseTexture;	// texture sampler

uniform vec4 lightPosition1;
uniform vec3 lightColor1;

uniform vec4 lightPosition2;
uniform vec3 lightColor2;

uniform vec4 lightPosition3;
uniform vec3 lightColor3;

uniform vec4 lightPosition4;
uniform vec3 lightColor4;

//uniform vec3 ambientColor;  
uniform vec3 cameraPositionWorld;

// shader output
out vec4 outputColor;

// fragment shader
void main()
{
    
    //DIFFUSE
    //vec3 L = lightPosition - positionWorld.xyz;                                 //Vector from surface to light, unnormalized!
    //float attenuation = 1.0 / dot(L, L);                                        //distance attenuation
    //float NdotL = max(0, dot(normalize(normalWorld.xyz), normalize(L)));        //incoming angle attenuation
    //vec3 diffuseColor = texture(diffuseTexture, uv).rgb;                        //texture lookup
    //outputColor = vec4(lightColor * diffuseColor * attenuation * NdotL, 1.0);   //complete diffuse shading, A = 1.0 is opaque

    
    //PHONG
    outputColor.xyz = vec3(0, 0, 0);
    int n = 2;
    vec3 speculalColor = vec3(0.8, 0.8, 0.8);

    vec3 diffuseColor = texture(diffuseTexture, uv).rgb;                        //texture lookup
    
    vec3 lightPosition = vec3(0, 0, 0);
    vec3 lightColor = vec3(0, 0, 0);
    vec3 ambientLight = vec3(0, 0, 0);
    int ambientCounter = 0;

    for(int i = 1; i < 5; i++)
    {
        lightColor = vec3(0, 0, 0);
        if(i == 1 && lightColor1 != vec3(0, 0, 0))
        {
            lightPosition = lightPosition1.xyz;
            lightColor = lightColor1;
            ambientCounter++;
        }
        else if(i == 2 && lightColor2 != vec3(0, 0, 0))
        {
            lightPosition = lightPosition2.xyz;
            lightColor = lightColor2;
            ambientCounter++;
        }
        else if(i == 3 && lightColor3 != vec3(0, 0, 0))
        {
            lightPosition = lightPosition3.xyz;
            lightColor = lightColor3;
            ambientCounter++;
        }
        else if(i == 4 && lightColor4 != vec3(0, 0, 0))
        {
            lightPosition = lightPosition4.xyz;
            lightColor = lightColor4;
            ambientCounter++;
        }

        ambientLight += lightColor;

        if(lightColor != vec3(0, 0, 0))
        {
            vec3 L = lightPosition - positionWorld.xyz;                                 //Vector from surface to light, unnormalized!
            float attenuation = 1.0 / dot(L, L);                                        //distance attenuation
            vec3 R = normalize(-L - 2 * dot(normalize(-L), normalize(normalWorld.xyz)) * normalize(normalWorld.xyz));
            vec3 V = normalize(positionWorld.xyz - cameraPositionWorld);
            float power = pow(max(0, dot(-V, R)), n);
            vec3 a = lightColor * attenuation;
            vec3 b = diffuseColor * max(0, dot(normalize(normalWorld.xyz), normalize(L)));
            vec3 c = speculalColor * power;

            outputColor.xyz += a * (b + c);
        }
    }
    vec3 d;
    if(ambientCounter > 0)
    {
        vec3 ambientLightFinal = (ambientLight / ambientCounter) * 0.006;
        d = ambientLightFinal * diffuseColor;
    }
    else
        d = vec3(0, 0, 0);


    outputColor.xyz += d;

    //outputColor = lightColor * attenuation * (diffuseColor * max(0, dot(normalize(normalWorld.xyz), normalize(L))) + vec3(0.8,0.8,0.8) * pow(max(0, dot(V, R)), n)) + ambientLight * diffuseColor;

    //outputColor = texture(diffuseTexture, uv) + 0.5 * normalWorld;
}