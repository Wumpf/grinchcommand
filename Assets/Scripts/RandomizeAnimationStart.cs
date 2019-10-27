using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RandomizeAnimationStart : MonoBehaviour
{
    private void Start()
    {
        Animator anim = GetComponent<Animator>();
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        anim.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
    }
}