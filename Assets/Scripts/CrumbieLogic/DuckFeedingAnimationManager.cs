using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DuckFeedingAnimationManager : MonoBehaviour
{
    public static DuckFeedingAnimationManager reference;

    [SerializeField] RectTransform crumbUIIcon;
    [SerializeField] FlyingCrumbie flyingCrumbPrefab;
    [SerializeField] Camera worldCamera;
    void Awake()
    {
        reference = this;
    }

    private void Start()
    {
        worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }
    public void PlayFeedSequence(List<DuckWalk> ducksToFeed)
    {
        StartCoroutine(FeedRoutine(ducksToFeed));
    }

    IEnumerator FeedRoutine(List<DuckWalk> ducksToFeed)
    {
        foreach (DuckWalk duck in ducksToFeed)
        {
            SpawnFlyingCrumb(duck);
            yield return new WaitForSeconds(0.08f);
        }
    }

    public void SpawnFlyingCrumb(DuckWalk duck)
    {
        Vector3 startWorldPos = GetUIIconWorldPosition();
        Vector3 targetWorldPos = duck.transform.position;

        FlyingCrumbie crumb = Instantiate(flyingCrumbPrefab, startWorldPos, Quaternion.identity);
        crumb.Init(startWorldPos, targetWorldPos, () =>
        {
            //duck.OnFedOneCrumb();
        });
    }

    Vector3 GetUIIconWorldPosition()
    {
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(null, crumbUIIcon.position);
        Vector3 worldPos = worldCamera.ScreenToWorldPoint(screenPos);
        worldPos.z = 0f;
        return worldPos;
    }
}
