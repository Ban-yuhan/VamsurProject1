using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 가중치 기반 룰렛 선택
/// 가중치 합에서 랜덤하게 1개를 뽑는다.
/// </summary>
public static class WeightedPicker
{
    
    public static int PickIndexByWeight(List<float>weights, System.Random rng)
    {

        if(weights == null || weights.Count == 0) //weight에 데이터가 없으면
        {
            return -1;
        }


        float sum = 0.0f;

        for (int i = 0; i < weights.Count; ++i)
        {
            sum += weights[i]; //가중치 값 누적(가중치 합 계산)
        }

        if (sum <= 0.0f)
        {
            return -1;
        }

        double r = rng.NextDouble() * sum; //난수 획득
        float acc = 0.0f;

        for (int i = 0; i < weights.Count; ++i)
        {
            float w = weights[i];
            if (w <= 0.0f)
            {
                continue;
            }

            acc += w;
            if(r <= acc)
            {
                return i;
            }
        }

        return weights.Count - 1;

    }

}
