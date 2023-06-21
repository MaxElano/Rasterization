#version 330
 
// shader inputs
in vec4 positionWorld;              // fragment position in World Space
in vec4 normalWorld;                // fragment normal in World Space
in vec2 uv;                         // fragment uv texture coordinates
uniform sampler2D diffuseTexture;	// texture sampler

uniform vec3 lightPosition = vec3(10, 10, 10);
uniform vec3 lightColor = vec3(255, 255, 255);
//uniform vec3 ambientColor; 
uniform vec3 ambientLight = vec3(10, 10, 10);
uniform vec3 cameraPositionWorld = vec3(20, 20, 20);

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
    vec3 L = lightPosition - positionWorld.xyz;                                 //Vector from surface to light, unnormalized!
    float attenuation = 1.0 / dot(L, L);                                        //distance attenuation
    vec3 diffuseColor = texture(diffuseTexture, uv).rgb;                        //texture lookup
    int n = 2;
    vec3 R = normalize(-L - 2 * dot(normalize(-L), normalize(normalWorld.xyz)) * normalize(normalWorld.xyz));
    vec3 V = normalize(positionWorld.xyz - cameraPositionWorld);
    float power = pow(max(0, dot(V, R)), n);
    vec3 a = lightColor * attenuation;
    vec3 b = diffuseColor * max(0, dot(normalize(normalWorld.xyz), normalize(-L)));
    vec3 c = diffuseColor * power;
    vec3 d = ambientLight * diffuseColor;

    outputColor.xyz = a * (b + c) + d;

    //outputColor = lightColor * attenuation * (diffuseColor * max(0, dot(normalize(normalWorld.xyz), normalize(L))) + diffuseColor * pow(max(0, dot(V, R)), n)) + ambientLight * diffuseColor;

    //outputColor = texture(diffuseTexture, uv) + 0.5 * normalWorld;
}