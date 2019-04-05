using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AutoChess.Prototype
{
    public class ChessPrefabTransformInformation : MonoBehaviour
    {
        public Vector3 LocalPosition;
        /// <summary>
        /// góc xoay nếu model đứng xoay lưng về phía người chơi
        /// </summary>
        public Vector3 LocalRotation = new Vector3(25f, 0f, 0f);
        /// <summary>
        /// góc xoay nếu model đứng xoay mặt về phía người chơi
        /// </summary>
        public Vector3 LocalRotationOpposite = new Vector3(-25f, 180f, 0f);
        public float LocalScale = 1f;
    }
}
