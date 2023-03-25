using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Sirenix.OdinInspector;
using DG.Tweening;

public class BlitVFXController : MonoBehaviour
{
    [SerializeField] private GameEventManager gameEventManager = null;
    [SerializeField] private ForwardRendererData forwardRendererData;
    [SerializeField] private string renderFeatureName;
    [SerializeField] private Material voronoiFireMat = null;
    private string voronoiHoleSizeParamName = "_holeSize";
    [SerializeField] private float voronoiFadeInDur = .25f;
    [SerializeField] private float voronoiFadeOutDur = .25f;
    [SerializeField] private float voronoiFireUptime = .5f;


    private Tween voronoiTween = null;

    private void OnDisable()
    {
        gameEventManager.OnDestructibleDestroyed -= GameEventManager_OnDestructibleDestroyed;
    }
    private void Start()
    {
        gameEventManager.OnDestructibleDestroyed += GameEventManager_OnDestructibleDestroyed;

        SetVoronoiFire(false);
    }

    private void GameEventManager_OnDestructibleDestroyed()
    {
        Invoke(nameof(FadeIn), 0f);
        Invoke(nameof(FadeOut), voronoiFireUptime + voronoiFadeInDur);
    }

    #region materials

    [Button]
    public void FadeIn()
    {
        voronoiTween.Kill();
        voronoiTween = 
            DOTween.To(() => 0.2f,
            x => voronoiFireMat.SetFloat(voronoiHoleSizeParamName, x),
            0.60f,
            voronoiFadeInDur)
            .SetUpdate(false)
                .OnStart(()=> { SetVoronoiFire(true); });
        
    }
    [Button]
    public void FadeOut()
    {

        voronoiTween.Kill();
        voronoiTween = 
            DOTween.To(() => 0.60f,
            x => voronoiFireMat.SetFloat(voronoiHoleSizeParamName, x),
            0.2f,
            voronoiFadeOutDur)
            .SetUpdate(false)
                .OnComplete(() => { SetVoronoiFire(false); }); ;
    }

    #endregion

    #region render features

    [Button]
    public void SetVoronoiFire(bool _arg)
    {

        // Find the specific render feature to disable
        ScriptableRendererFeature myRenderFeature = GetRenderFeatureByName(renderFeatureName);

        // Check if the render feature exists in the rendererFeatures list
        if (myRenderFeature != null && forwardRendererData.rendererFeatures.Contains(myRenderFeature))
        {
            // Remove the render feature from the rendererFeatures list
            myRenderFeature.SetActive(_arg);
        }
    }

    private ScriptableRendererFeature GetRenderFeatureByName(string name)
    {
        // Get all the renderer features currently assigned to the Forward Renderer
        ScriptableRendererFeature[] renderFeatures = forwardRendererData.rendererFeatures.ToArray();

        // Iterate through the renderer features to find the specific render feature by name
        foreach (ScriptableRendererFeature renderFeature in renderFeatures)
        {
            if (renderFeature.name == name)
            {
                Debug.Log("found: " + name);
                return renderFeature;
            }
        }
        Debug.Log("didnt find: " + name);
        // The specific render feature could not be found
        return null;
    }

    #endregion
}
