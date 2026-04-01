using UnityEngine;

public class Grass : MonoBehaviour
{
    void Start()
    {
        //TODO remove this it is just for temp testing and should be done elsewhere
        PublicInfo.reference.grassList.Add(this);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Duck"))
        {
            //TODO This check should be redone, grass should be a building
            if (collision.GetComponent<DuckWalk>().beingDragged)
            {
                return;
            }
            CrumbManager.reference.GainCrumbs(1);
            PublicInfo.reference.crumbieGainedFromGrass += 1;
            SoundSystem.instance.PlaySound("grass");
            CrumbManager.reference.SpawnCrumbiePopupIncrease(transform.position, 1);
        }
    }
}
