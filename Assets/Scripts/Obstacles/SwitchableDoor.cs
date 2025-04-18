using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;

public class SwitchableDoor : Switchable
{
    private List<DoorTweenAnimation> openAnimationList = new List<DoorTweenAnimation>();
    private List<DoorTweenAnimation> closeAnimationList = new List<DoorTweenAnimation>();
    public int selectedIndex = 0;

    #region // Inspector properties

    [SerializeField] public float value;
    [SerializeField] public float durationTime;

    #endregion

    #region // DoorTweenAnimation

    private DoorTweenAnimation moveOpenAnimation;
    private DoorTweenAnimation rotateOpenAnimation;
    private DoorTweenAnimation moveCloseAnimation;
    private DoorTweenAnimation rotateCloseAnimation;

    #endregion

    public void OnEnable()
    {
        #region // DoorTweenAnimation enable

        moveOpenAnimation = new DoorTweenAnimation("Move Open Animation", MoveOpenBounce);
        rotateOpenAnimation = new DoorTweenAnimation("Rotate Open Animation", RotateOpenBounce);

        moveCloseAnimation = new DoorTweenAnimation("Move Close Animation", MoveCloseBounce);
        rotateCloseAnimation = new DoorTweenAnimation("Rotate Close Animation", RotateCloseBounce);

        #endregion

        openAnimationList = new List<DoorTweenAnimation>
        {
            moveOpenAnimation,
            rotateOpenAnimation
        };

        closeAnimationList = new List<DoorTweenAnimation>
        {
            moveCloseAnimation,
            rotateCloseAnimation
        };
    }

    #region // SwitchableDoor Methods

    // Public Inherit Methods
    public override void Activate()
    {
        OpenAnimation();
    }

    public override void Disable()
    {
        CloseAnimation();
    }

    // public Methods
    public string[] GetAnimationsNames()
    {
        List<string> names = new List<string>();
        foreach (DoorTweenAnimation aux in openAnimationList)
        {
            names.Add(aux.ToString());
        }

        return names.ToArray();
    }

    // Private Methods
    [ContextMenu("OpenAnimation")]
    private void OpenAnimation()
    {
        openAnimationList[selectedIndex].TweenAnimation();
    }

    [ContextMenu("CloseAnimation")]
    private void CloseAnimation()
    {
        closeAnimationList[selectedIndex].TweenAnimation();
    }

    #endregion


    #region // TweenAnimation

    private void MoveOpenBounce()
    {
        transform.DOMoveY(value, durationTime).SetEase(Ease.InBounce);
    }
    private void RotateOpenBounce()
    {
        transform.DORotate(new Vector3(0f, value, 0f), durationTime).SetEase(Ease.InBounce);
    }

    private void MoveCloseBounce()
    {
        transform.DOMoveY(0, durationTime).SetEase(Ease.InBounce);
    }
    private void RotateCloseBounce()
    {
        transform.DORotate(new Vector3(0f, 0f, 0f), durationTime).SetEase(Ease.InBounce);
    }

    #endregion 


    internal class DoorTweenAnimation
    {
        private string name;
        private Action tweenAnimation;

        public DoorTweenAnimation(string name, Action tweenAnimation)
        {
            this.name = name;
            this.tweenAnimation = tweenAnimation;
        }

        public void TweenAnimation()
        {
            tweenAnimation?.Invoke();
        }

        public override string ToString()
        {
            return name;
        }
    }
}

[CustomEditor(typeof(SwitchableDoor))]
public class SwitchableDoorInspector : Editor
{
    private SwitchableDoor _switchableDoor;
    private SerializedObject _serializedObject;

    private SerializedProperty _value;
    private SerializedProperty _durationTime;

    private void OnEnable()
    {
        _switchableDoor = target as SwitchableDoor;
        _switchableDoor.OnEnable();

        _serializedObject = new SerializedObject(_switchableDoor);

        _value = _serializedObject.FindProperty("value");
        _durationTime = _serializedObject.FindProperty("durationTime");
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();

        GUILayout.Label("Animation Mode");

        _switchableDoor.selectedIndex = EditorGUILayout.Popup("", _switchableDoor.selectedIndex, _switchableDoor.GetAnimationsNames());

        GUILayout.EndHorizontal();

        _serializedObject.Update();

        EditorGUILayout.PropertyField(_value);
        EditorGUILayout.PropertyField(_durationTime);

        _serializedObject.ApplyModifiedProperties();
    }   
}
