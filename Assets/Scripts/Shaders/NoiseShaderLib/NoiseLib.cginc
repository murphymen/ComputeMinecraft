#include "SimplexNoise2D.hlsl"
#include "SimplexNoise3d.hlsl"



float terrace(float h)
{
    float W = 0.2;  //terraceSize;
    float k = floor(h / W);
    float f = (h - k * W) / W;
    float s = min(2 * f, 1.0);
    return (k + s) * W;

}

float perlin(float2 pos,
            float freq,
            int oct,
            float persist,
            float lacun)
{
    float total = 0;
    float fre = freq;
    float amp = 1; //amplitude;
    float maxValue = 0;
    
    for (int i = 0; i < oct; i++)
    {
        total += SimplexNoise(float2(pos.x * fre,
                                    pos.y * fre)) * amp;
        //if (i = 0)
            //total = terrace(total);
        maxValue += amp;
        amp *= persist;
        fre *= lacun;
    }

    //return total / maxValue;
    return total;
}

float perlin3d(float3 pos,
            float freq,
            float ampl,
            int oct,
            float persist,
            float lacun)
{
    float total = 0;
    float fre = freq;
    float amp = ampl; //amplitude;
    float maxValue = 0;
    
    for (int i = 0; i < oct; i++)
    {
        total += SimplexNoise(pos * fre) * amp;
        //if (i = 0)
            //total = terrace(total);
        maxValue += amp;
        amp *= persist;
        fre *= lacun;
    }

    return total / maxValue;
    return total;
}

/*
float ridgeNoise2D(float2 position)
{
    return 0.5 * (0.5f - abs(0.5f - SimplexNoise(position)));
}
*/