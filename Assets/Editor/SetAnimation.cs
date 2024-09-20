using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class SetAnimation: MonoBehaviour
{
    [MenuItem("Assets/Set Animation")]
    static void Perform()
    {
        string animationClipName = "max_04_Ani";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/model.prefab");
        Animator animator = prefab.GetComponent<Animator>();
        AnimatorController controller = (AnimatorController) animator.runtimeAnimatorController;

        // Get all assets from the fbx
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath("Assets/model.fbx");

        // Choose AnimationClip named animationClipName, and add it.
        bool clipIsFound = false;
        foreach (Object obj in assets) {
            if (obj is AnimationClip && obj.name == animationClipName) {
                AddAnimation(animator.runtimeAnimatorController as AnimatorController, (AnimationClip) obj);
                clipIsFound = true;
            }
        }
        if (!clipIsFound) {
            Debug.LogError("AnimationClip named " + animationClipName + " is not found...");
        }
    }

    static void AddAnimation(AnimatorController animatorController, AnimationClip clip)
    {
        AnimatorStateMachine stateMachine = animatorController.layers[0].stateMachine;

        // Initialize Animator: Clear the state of previous execution.
        foreach (ChildAnimatorState state in stateMachine.states) {
            stateMachine.RemoveState(state.state);
        }
        // Create a new state with clip
        AnimatorState newState = stateMachine.AddState(clip.name);
        newState.motion = clip;

        // Create a new animator's transition from defaultState.
        AnimatorState defaultState = stateMachine.defaultState;
        animatorController.layers[0].stateMachine.AddAnyStateTransition(newState);
    }
}
