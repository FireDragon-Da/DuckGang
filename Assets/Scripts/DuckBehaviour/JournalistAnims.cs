using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class JournalistAnims : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject journalist;
    Animator jAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jAnimator = journalist.GetComponent<Animator>();
        StartCoroutine(changeAnim());
    }

    IEnumerator changeAnim()
    {
        yield return new WaitForSeconds(Random.Range(10, 30));
        int clip = Random.Range(0, 3);
        jAnimator.SetInteger("clip", clip);
        print("changed clip to clip " + clip);
        StartCoroutine(changeAnim());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("pointer enter interface");
        journalist.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("pointer exit interface");
        journalist.SetActive(true);
    }
}
