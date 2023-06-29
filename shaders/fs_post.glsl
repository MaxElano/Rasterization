#version 330

// shader inputs
in vec2 uv;						// fragment uv texture coordinates
in vec2 positionFromBottomLeft;
in vec2 positionFromCenter;
uniform sampler2D pixels;		// input texture (1st pass render target)

// shader output
out vec3 outputColor;

// fragment shader
void main()
{
	// retrieve input pixel
	outputColor = texture(pixels, uv).rgb;

	vec2 CAoffset = vec2(0.005, 0.005);

	float r = texture(pixels, uv + CAoffset).r;
	float b = texture(pixels, uv - CAoffset).b;

	outputColor.r = r;
	outputColor.b = b;

	// apply dummy postprocessing effect
	float dist = length(positionFromCenter);
	outputColor *= -sin(dist * 0.3) * 2 + 1;

}
