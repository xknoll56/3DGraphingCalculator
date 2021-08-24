﻿#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aColor;
layout(location = 2) in vec2 aTexCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 vertColor;
out vec2 texCoord;

void main()
{
    //vertColor = aColor;
    texCoord = aTexCoord;
    gl_Position = projection*model*view*vec4(aPosition, 1.0);
}