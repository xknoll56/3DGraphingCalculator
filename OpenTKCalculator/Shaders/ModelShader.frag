#version 330 core
out vec4 FragColor;

in vec2 texCoord;
in vec3 Normal;  
in vec3 FragPos; 


uniform int type;
uniform sampler2D texture0;
uniform vec3 color;


void main()
{
    vec3 ambient = vec3(1, 1, 1);
    vec3 lightDir = normalize(vec3(1, -1,  1));
    float diffuse = max(dot(Normal, lightDir), 0.35);

    if(type == 3)
        FragColor = texture(texture0, texCoord)*vec4(color, 1.0);
    else if(type == 1)
        FragColor = vec4(vec3(texture(texture0, texCoord))*diffuse, 1.0);
    else if(type == 2)
        FragColor = vec4(color*diffuse, 1.0);
}