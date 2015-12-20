using System;
using System.Collections;
using UnityEngine;

namespace SpaceShooter
{
    public static class ActionGenerator
    {
        public static IEnumerator SmoothChangeAlpha(SpriteRenderer obj, float hideTo, float time)
        {
            if (time == 0)
            {
                Color color = new Color(obj.color.r, obj.color.g, obj.color.b, hideTo);
                obj.color = color;
            }
            else
            {
                float distance = Math.Abs(obj.color.a - hideTo);
                float speedMoving = distance / time;
                if (hideTo < obj.color.a)
                {
                    while (obj.color.a > 0.0f)
                    {
                        Color color = new Color(obj.color.r, obj.color.g, obj.color.b, obj.color.a - speedMoving * Time.deltaTime);
                        obj.color = color;
                        yield return null;
                    }
                }
                else
                {
                    while (obj.color.a < hideTo)
                    {
                        Color color = new Color(obj.color.r, obj.color.g, obj.color.b, obj.color.a + speedMoving * Time.deltaTime);
                        obj.color = color;
                        yield return null;
                    }
                }
            }
        }
    }
}

