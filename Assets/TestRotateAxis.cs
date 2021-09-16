#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestRotateAxis : MonoBehaviour {

    /// <summary> 围绕旋转的中心点  </summary>
    private Vector3 m_center;

    /// <summary> 未点击时保持的旋转角 </summary>
    private Quaternion m_initRotation;

    private Camera m_camera;
    private RaycastHit[] m_tempRaycastHits = new RaycastHit[32];


    /// <summary>
    /// 获取鼠标位置在平面上的世界坐标
    /// </summary>
    /// <returns></returns>
    private Vector3? GetMousePointOnPlane () {
        Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
        int count = Physics.RaycastNonAlloc(ray, m_tempRaycastHits);
        for (int i = 0; i < count; i++) {
            RaycastHit raycastHit = m_tempRaycastHits[i];
            if (raycastHit.collider && raycastHit.collider.gameObject.name == "Plane") {
                return raycastHit.point;
            }
        }
        return null;
    }

    private void Start () {
        m_center = transform.position;
        m_initRotation = transform.rotation;
        m_camera = Camera.main;
    }


    private void Update () {
        if (Input.GetMouseButtonDown(0)) {
            Vector3? mousePointOnPlane = GetMousePointOnPlane();
            if (mousePointOnPlane != null) {
                // 由’中心‘指向‘点击的位置’的向量
                Vector3 releative = mousePointOnPlane.Value - m_center;
                releative.Normalize();

                Vector3 axis = -transform.right; // 默认的旋转轴向为需要旋转物体的左方向
                axis = Quaternion.LookRotation(releative) * axis;
                transform.RotateAround(m_center, axis, -20f);
            }
        } else {
            transform.rotation = Quaternion.Lerp(transform.rotation,m_initRotation,0.1f);
        }
    }
}
