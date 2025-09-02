// This file includes code from [] (https://example.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

sampler scrTex : register(s0); // Screen texture
sampler palTex : register(s1); // Palette texture
int palWid; // Palette width

float grayscale(float3 rgb) {
    return (rgb.r * 0.3 + rgb.g * 0.59 + rgb.b * 0.11);
}

float4 FilterMyShader(float2 coords : TEXCOORD0) : COLOR0 {
    // We don't want users to soft-lock themselves if there's just one color in palette
    if (palWid < 2)
        return tex2D(scrTex, coords);
    
    float4 currentColor = tex2D(scrTex, coords);
    float currentGrayscale = grayscale(currentColor.rgb);

    float3 closestColor = { 0, 0, 0 };
    float minDiff = 123;
    
    for (float x = 0; x < palWid; x++) {
        float3 paletteColor = tex2Dlod(palTex, float4((x + .5) / palWid, .5, 0, 0)).rgb;
        float diff = abs(grayscale(paletteColor) - currentGrayscale);
        
        if (diff < minDiff) {
            minDiff = diff;
            closestColor = paletteColor;
        }
    }
    
    return float4(closestColor, currentColor.a);
}

technique Technique1 {
    pass FilterMyShader {
        PixelShader = compile ps_3_0 FilterMyShader();
    }
}
