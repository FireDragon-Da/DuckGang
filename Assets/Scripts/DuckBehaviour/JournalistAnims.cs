using System.Collections;
using UnityEngine;

public class JournalistAnims : MonoBehaviour
{

    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(changeAnim());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator changeAnim()
    {
        yield return new WaitForSeconds(Random.Range(1, 3));
        int clip = Random.Range(0, 3);
        animator.SetInteger("clip", clip);
        print("changed clip to clip " + clip);
        StartCoroutine(changeAnim());
    }
}
