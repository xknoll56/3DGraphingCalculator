#version 330 core
out vec4 FragColor;

in vec2 texCoord;
uniform int type;
uniform sampler2D texture0;
uniform vec4 color;


void main()
{
    if(type == 3)
        FragColor = texture(texture0, texCoord)*color;
    else if(type == 1)
        FragColor = texture(texture0, texCoord);
    else if(type == 2)
        FragColor = color;
}