using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandFuncs
{
    public static class ExpandFuncs
    {
        public static IEnumerator ScaleUp(this RectTransform rt, Vector3 target, float time)
        {
            float curTime = 0f;
            Vector3 origin = rt.localScale;
            Vector3 speed = (target - origin)/time;
            while(curTime < time)
            {
                rt.localScale = origin + speed * curTime;
                yield return null;
                curTime += Time.deltaTime;
            }
            rt.localScale = target;
            yield break;
        }
    }
}

