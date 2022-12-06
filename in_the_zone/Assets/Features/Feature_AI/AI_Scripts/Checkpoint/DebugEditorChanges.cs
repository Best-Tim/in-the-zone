using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEditorChanges : MonoBehaviour {
    [SerializeField][Range(1,20)] private int TimeScale = 1;
    void Awake() {
        UnityEngine.Time.timeScale = TimeScale;
    }
}
