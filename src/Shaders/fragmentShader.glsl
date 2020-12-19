#version 330 core

out vec4 fragColor;
in vec3 surfaceNormal;
in vec3 fragPosition;

uniform vec3 lightPosition;
uniform vec3 cameraPosition;
uniform vec3 lightColor;
uniform vec3 color;

void main() {

    float ambientStrength = 0.4;
    vec3 ambient = ambientStrength * lightColor;

    vec3 normal = normalize(surfaceNormal);
    vec3 lightDirection = normalize(lightPosition - fragPosition);

    float diff = max(dot(normal, lightDirection), 0.0)/5;
    vec3 diffuse = lightColor*diff;

    float specularStrength = 0.8;
    vec3 viewDirection = normalize(cameraPosition - fragPosition);
    vec3 reflectionDirection = reflect(-lightDirection, normal);
    float spec = pow(max(dot(viewDirection, reflectionDirection), 0.0), 32);
    vec3 specular = specularStrength * spec * lightColor;

    fragColor = vec4((ambient+diffuse+specular)*color, 1.0);
}