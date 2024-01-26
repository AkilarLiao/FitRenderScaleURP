using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class FitRenderScaleControler : MonoBehaviour
{
    private void OnEnable()
    {
        m_originalScaleRatio = UniversalRenderPipeline.asset.renderScale;
        m_targetCamera = GetComponent<Camera>();
        RenderPipelineManager.beginCameraRendering -=
            OnBeginCameraRendering;
        RenderPipelineManager.beginCameraRendering +=
            OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering -=
            OnEndCameraRendering;
        RenderPipelineManager.endCameraRendering +=
            OnEndCameraRendering;

        RenderPipelineManager.beginContextRendering -= OnBeginContextRendering;
        RenderPipelineManager.beginContextRendering += OnBeginContextRendering;

    }
    private void OnDisable()
    {
        RenderPipelineManager.beginContextRendering -= OnBeginContextRendering;

        UniversalRenderPipeline.asset.renderScale = m_originalScaleRatio;
        RenderPipelineManager.beginCameraRendering -=
            OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering -=
            OnEndCameraRendering;
    }
    private void OnBeginCameraRendering(ScriptableRenderContext context,
        Camera camera)
    {
        if (m_targetCamera == camera)
            UniversalRenderPipeline.asset.renderScale = 1.0f;
    }
    private void OnEndCameraRendering(ScriptableRenderContext context,
        Camera camera)
    {
        if (m_targetCamera == camera)
            UniversalRenderPipeline.asset.renderScale = m_originalScaleRatio;
    }

    private void OnBeginContextRendering(ScriptableRenderContext context,
        List<Camera> cameras)
    {
        m_originalScaleRatio = UniversalRenderPipeline.asset.renderScale;
    }

    private float m_originalScaleRatio = 1.0f;
    private Camera m_targetCamera = null;
}