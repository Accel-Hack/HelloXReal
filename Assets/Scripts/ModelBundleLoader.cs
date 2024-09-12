using System.IO;
using UnityEngine;
// using UnityEditor;
// using UnityEditor.Animations;

// Load fbx from AssetBuilder, place it on the scene, and play its animation.
public class ModelBundleLoader : MonoBehaviour
{
    // An empty AnimatorController
    [SerializeField] RuntimeAnimatorController controller;

    // The name of AnimationClip to be played on start of the scene.
    [SerializeField] string animationClipName;

    void Start()
    {
        // Load AssetBundle from local files.
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "external"));

        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }

        // Get fbx as GameObject from loaded AssetBundle.
        var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("model.fbx");

        // Instantiate fbx as GameObject.
        GameObject model = Instantiate(prefab, new Vector3(0, 0, 10), Quaternion.identity);

        // Add an Animator Component.
        Animator animator = model.AddComponent<Animator>();

        // Set AnimatorController on the Animator.
        animator.runtimeAnimatorController = controller;

        // Get all assets from the fbx
        // Object[] assets = AssetDatabase.LoadAllAssetsAtPath("Assets/model.fbx");

        // Flag to identify whether an AnimationClip named animationClipName is found.
        bool clipIsFound = false;

        // Choose AnimationClip named animationClipName, and add it.
        // foreach (Object obj in assets) {
        //     if (obj is AnimationClip && obj.name == animationClipName) {
        //         // AddAnimation(animator.runtimeAnimatorController as AnimatorController, (AnimationClip) obj);
        //         // clipIsFound = true;
        //         controller.animationClips = new AnimationClip[]{ (AnimationClip) obj };
        //     }
        // }
        // if (!clipIsFound) {
        //     Debug.LogError("AnimationClip named " + animationClipName + " is not found...");
        // }
    }

    // void AddAnimation(AnimatorController animatorController, AnimationClip clip)
    // {
    //     AnimatorStateMachine stateMachine = animatorController.layers[0].stateMachine;

    //     // Initialize Animator: Clear the state of previous execution.
    //     foreach (ChildAnimatorState state in stateMachine.states) {
    //         stateMachine.RemoveState(state.state);
    //     }
    //     // Create a new state with clip
    //     AnimatorState newState = stateMachine.AddState(clip.name);
    //     newState.motion = clip;

    //     // Create a new animator's transition from defaultState.
    //     AnimatorState defaultState = stateMachine.defaultState;
    //     animatorController.layers[0].stateMachine.AddAnyStateTransition(newState);
    // }
}