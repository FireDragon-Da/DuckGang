using UnityEngine;

[CreateAssetMenu(fileName = "NewBuildingVFX", menuName = "GameData/BuildingVFX")]
public class BuildingVFXSO : ScriptableObject
{
    [Header("Scale Bounce")]
    public bool enableScaleBounce;
    public Vector3 bounceScale = new Vector3(1.2f, 0.8f, 1f); 
    public float scaleUpDuration = 0.1f;
    public float scaleDownDuration = 0.15f;

    [Header("Shake")]
    public bool enableShake;
    public float shakeDuration = 0.2f;
    public float shakeIntensity = 0.1f;

    [Header("Color Flash")]
    public bool enableColorFlash;
    public Color flashColor = Color.white;
    public float flashDuration = 0.2f;
}