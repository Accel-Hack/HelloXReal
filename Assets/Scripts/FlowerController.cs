/****************************************************************************
* Copyright 2019 Xreal Techonology Limited. All rights reserved.
*                                                                                                                                                          
* This file is part of NRSDK.                                                                                                          
*                                                                                                                                                           
* https://www.xreal.com/        
* 
*****************************************************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace NRKernal.NRExamples
{
    /// <summary> Controls the HelloAR example. </summary>
    [HelpURL("https://developer.xreal.com/develop/unity/controller")]
    public class FlowerController : MonoBehaviour
    {
        private const float COOLTIME = 2.0f;
        private bool switchIsNotCoolingDown = true;
        private bool instantiateIsNotCoolingDown = true;
        private int currentIndex = 0;
        private readonly WaitForSeconds twosec = new(COOLTIME);
        [SerializeField] NRPointerRaycaster leftPointer;
        [SerializeField] List<GameObject> flowers;

        /// <summary> Updates this object. </summary>
        void Update()
        {
            HandState handState = NRInput.Hands.GetHandState(HandEnum.LeftHand);
            switch (handState.currentGesture) {
                case HandGesture.Pinch:
                    if (switchIsNotCoolingDown) {
                        currentIndex++;
                        if (currentIndex >= flowers.Count) {
                            currentIndex = 0;
                        }
                        StartCoroutine(CoolSwitchDown());
                    }
                    break;
                case HandGesture.Point:
                    if (instantiateIsNotCoolingDown) {
                        RaycastResult result = leftPointer.FirstRaycastResult();
                        if (result.gameObject != null && result.gameObject.GetComponent<NRTrackableBehaviour>() != null) {
                            var behaviour = result.gameObject.GetComponent<NRTrackableBehaviour>();
                            if (behaviour.Trackable.GetTrackableType() != TrackableType.TRACKABLE_PLANE)
                            {
                                return;
                            }

                            // Instantiate Andy model at the hit point / compensate for the hit point rotation.
                            Instantiate(flowers[currentIndex], result.worldPosition, Quaternion.identity, behaviour.transform);
                        }
                        StartCoroutine(CoolInstantiateDown());
                    }
                    break;
            }
        }
        private IEnumerator CoolSwitchDown() {
            switchIsNotCoolingDown = false;
            yield return twosec;
            switchIsNotCoolingDown = true;
        }

        private IEnumerator CoolInstantiateDown() {
            instantiateIsNotCoolingDown = false;
            yield return twosec;
            instantiateIsNotCoolingDown = true;
        }
    }
}
