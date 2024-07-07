/*
 * 感谢Bilibili up主 Small-K 提供的gdshader源码！
 * 本人对于源码做出了少量修改，包括：
 * 1. 将原本屏幕空间的纹理采样改为ViewportTexture
 * 2. screen_UVg改为UV
*/

shader_type canvas_item;
// y 方向的缩放
uniform float y_scale = 0.5;
// 噪声图，为了换取随机值
uniform sampler2D noise_tex;
// 波浪摇曳速度
uniform float speed = 0.25;
// 波浪摇曳强度
uniform float strength = 0.0125;
// 色调
uniform vec4 tint_color:source_color = vec4(1., 1., 1., 1.);

uniform float blur_radius = 0.5;
const float MATRIX = 2.3999632;
const int ITERATIONS = 200;

// 模糊算法
vec3 blur(sampler2D tex, vec2 uv, float radius)
{
	mat2 rot = mat2(vec2(cos(MATRIX), sin(MATRIX)), vec2(-sin(MATRIX), cos(MATRIX)));
	vec3 acc = vec3(0), div = acc;
	float r = 1.0;
    vec2 vangle = vec2(0.0,radius*.01 / sqrt(float(ITERATIONS)));
	
	for (int j = 0;j < ITERATIONS; j++){
		r += 1. / r;
		vangle = rot * vangle;
		vec3 col = texture(tex, uv + (r-1.) * vangle).xyz;
		col = col * col *1.8;
		vec3 bokeh = pow(col, vec3(4));
		acc += col * bokeh;
		div += bokeh;
	}
	return acc / div;
}

void fragment() {
	// 倒置
	vec2 uv = vec2(UV.x, 1. - UV.y * (1. / y_scale));
	// 经典扰动算法
	float noise = texture(noise_tex, fract(uv + TIME * speed)).r;
	// 模糊算法
	vec4 color = vec4(blur(TEXTURE, uv - noise * strength + strength / 2., blur_radius),1.);
	// 调整色调
	COLOR = color * tint_color;
}