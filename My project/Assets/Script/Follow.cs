using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Color lineColor = Color.green;

    private LineRenderer lineRenderer;
    public PlayerAnim anim;
    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Standard"));
        lineRenderer.material.color = lineColor;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = false; // ���� ��ǥ�� ���
    }

    void Update()
    {
        // ���� ������Ʈ�� ��ġ�� ������ ������ ����Ͽ� ���� ������ ����
        Vector3 forwardEndPoint = transform.position + anim.forward() * 2f;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, forwardEndPoint);
    }
}
