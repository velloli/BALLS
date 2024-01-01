using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomPostProcessrenderFeature : ScriptableRendererFeature
{
    [SerializeField]
    private Shader m_bloomShader;

    [SerializeField]
    private Shader m_compositeShader;

    private Material m_bloomMaterial;
    private Material m_compositeMaterial;


    private CustomPostProcessPass m_processPass;
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_processPass);

    }

    public override void Create()
    {
        m_bloomMaterial = CoreUtils.CreateEngineMaterial(m_bloomShader);
        m_compositeMaterial = CoreUtils.CreateEngineMaterial(m_compositeShader);
        //Debug.Log(m_bloomMaterial);
        m_processPass = new CustomPostProcessPass(m_bloomMaterial, m_compositeMaterial);

    }


    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(m_bloomMaterial);
        CoreUtils.Destroy(m_compositeMaterial);
    }
    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        if(renderingData.cameraData.cameraType == CameraType.Game || renderingData.cameraData.cameraType == CameraType.SceneView)
        {
            m_processPass.ConfigureInput(ScriptableRenderPassInput.Depth | ScriptableRenderPassInput.Color);
            m_processPass.SetTarget(renderer.cameraColorTargetHandle, renderer.cameraDepthTargetHandle);

        }
    }

}
