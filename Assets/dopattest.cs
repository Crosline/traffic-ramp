using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class dopattest : MonoBehaviour
    {
        public Transform car;


        public Transform[] paths;


        private Vector3[] nodes;

        private void Start()
        {
            nodes = new Vector3[paths.Length];
            for (int i = 0; i < paths.Length; i++) nodes[i] = paths[i].position;
        }

        [Button]
        public void test(float duration, int resolution)
        {
            car.DOPath(nodes, duration, PathType.CubicBezier, PathMode.Full3D, resolution, Color.red).SetEase(Ease.Linear);
        }
    }
}
