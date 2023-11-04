using System.Collections.Generic;
using UnityEngine;

namespace Vanks.FPS
{
    public class RyunmLayoutFloors : MonoBehaviour
    {
        public Vector3 cellCount = Vector3.zero; // 地板格子数量
        public Vector3 floorSize = Vector3.zero; // 地板尺寸大小
        public List<Transform> floorTransforms = new();

        [ContextMenu("Reset Floor Layout")]
        private void ResetFloorLayout()
        {
            floorTransforms.Clear();
            var childrenTransforms = GetComponentsInChildren<Transform>();

            for (var i = 1; i < childrenTransforms.Length; i++)
            {
                if (childrenTransforms[i].gameObject.CompareTag($"Floor"))
                    floorTransforms.Add(childrenTransforms[i]);
            }

            if (floorTransforms.Count == 0) return;

            for (var i = 0; i < floorTransforms.Count; i++)
            {
                floorTransforms[i].name = $"Floor {i}";
            }

            if (floorTransforms.Count < cellCount.x * cellCount.z)
            {
                // 复制地板
                var floorPrefab = floorTransforms[0].gameObject;
                for (var i = floorTransforms.Count; i < cellCount.x * cellCount.z; i++)
                {
                    var newFloor = Instantiate(floorPrefab, transform);
                    newFloor.name = $"Floor {i}";
                    floorTransforms.Add(newFloor.transform);
                }
            }
            else if (floorTransforms.Count > cellCount.x * cellCount.z)
            {
                // 删除地板
                for (var i = floorTransforms.Count - 1; i >= cellCount.x * cellCount.z; i--)
                {
                    DestroyImmediate(floorTransforms[i].gameObject);
                    floorTransforms.RemoveAt(i);
                }
            }

            int currentIndex = 0;
            for (var x = 0; x < cellCount.x; x++)
            {
                for (var z = 0; z < cellCount.z; z++)
                {
                    Vector3 localPos = new Vector3(
                        x * floorSize.x,
                        0,
                        z * floorSize.z
                    );
                    floorTransforms[currentIndex].localPosition = localPos;
                    currentIndex++;
                }
            }

            this.gameObject.transform.position = new Vector3(
                -floorSize.x * cellCount.x / 2,
                -floorSize.y / 2,
                -floorSize.z * cellCount.z / 2
            );
        }
    }
}