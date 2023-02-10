using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class FitRenderScaleControler : MonoBehaviour
{
    private void OnEnable()
    {   
        m_targetCamera = GetComponent<Camera>();
        RenderPipelineManager.beginCameraRendering -=
            OnBeginCameraRendering;
        RenderPipelineManager.beginCameraRendering +=
            OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering -=
            OnEndCameraRendering;
        RenderPipelineManager.endCameraRendering +=
            OnEndCameraRendering;

        //var universalAdditionalCameraData = m_targetCamera.GetUniversalAdditionalCameraData();
        //universalAdditionalCameraData.

        //m_targetCamera.clearFlags = CameraClearFlags.Nothing;
    }
    private void OnDisable()
    {
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
        {
            m_originalScaleRatio = UniversalRenderPipeline.asset.renderScale;
            UniversalRenderPipeline.asset.renderScale = 1.0f;
            //camera.clearFlags = CameraClearFlags.Nothing;
        }
    }
    private void OnEndCameraRendering(ScriptableRenderContext context,
        Camera camera)
    {
        if (m_targetCamera == camera)
            UniversalRenderPipeline.asset.renderScale = m_originalScaleRatio;
    }
    private float m_originalScaleRatio = 1.0f;
    private Camera m_targetCamera = null;
}