using System.Collections.Generic;
using UnityEngine;

namespace Game.Utilities
{
    public static class Extensions
    {
        public static void Fade(this SpriteRenderer renderer, float alpha)
        {
            var color = renderer.color;
            color.a = alpha;
            renderer.color = color;
        }

        public static T Rand<T>(this IList<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        public static T Last<T>(this IList<T> list, int index = 0)
        {
            return list[list.Count - 1 - index];
        }
        
        public static int LayerMaskToLayerIndex(LayerMask layerMask)
        {
            int layerIndex = 0;
            int layer = layerMask.value;

            // Find the index of the first bit that is set in the layer mask
            while (layer > 0)
            {
                layer >>= 1;
                layerIndex++;
            }

            // Convert the index to the actual layer number
            return layerIndex - 1;
        }

        public static void SetLayersRecursively(this GameObject g, LayerMask layer)
        {
            var layerVal = LayerMaskToLayerIndex(layer);
            SetLayersRecursively(g, layerVal);
        }

        public static void SetLayersRecursively(this GameObject g, int layer)
        {
            g.layer = layer;
            foreach (Transform t in g.transform)
            {
                t.gameObject.SetLayersRecursively(layer);
            }
        }

        public static void DestroyRecursively(this Transform t, float time = 0f)
        {
            foreach (Transform child in t)
            {
                DestroyRecursively(child, time);
            }

            Object.Destroy(t.gameObject, time);
        }

        public static void DestroyChildren(this Transform t, float time = 0f)
        {
            foreach (Transform child in t)
            {
                Object.Destroy(child.gameObject, time);
            }
        }

        public static Vector2 ToVector2(this Vector3 vec3)
        {
            return new Vector2(vec3.x, vec3.z);
        }

        public static Vector3 Flat(this Vector3 vec3)
        {
            return new Vector3(vec3.x, 0, vec3.z);
        }

        public static Vector3Int ToVector3Int(this Vector3 vec3)
        {
            return new Vector3Int((int)vec3.x, (int)vec3.y, (int)vec3.z);
        }
    }
}