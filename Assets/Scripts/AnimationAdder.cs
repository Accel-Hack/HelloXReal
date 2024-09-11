using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;

public class AnimationAdder : MonoBehaviour
{
    private Animator animator;
    private AnimationClip newAnimationClip;

    void Start()
    {
        // Animatorを取得
        animator = GetComponent<Animator>();

        // AnimatorControllerを取得
        AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;

        newAnimationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/model.fbx");

        if (animatorController != null)
        {
            AddAnimation(animatorController, newAnimationClip);
        }
        else
        {
            Debug.LogError("Animator Controllerが見つかりません。");
        }
    }

    void AddAnimation(AnimatorController animatorController, AnimationClip clip)
    {
        // 新しいステートを作成してアニメーションクリップを割り当てる
        AnimatorState newState = animatorController.layers[0].stateMachine.AddState(clip.name);
        newState.motion = clip;

        // 必要に応じて、他のステートとの遷移を設定
        AnimatorState defaultState = animatorController.layers[0].stateMachine.defaultState;
        animatorController.layers[0].stateMachine.AddAnyStateTransition(newState);
    }
}
