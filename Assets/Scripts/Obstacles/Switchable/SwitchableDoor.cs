using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;

public class SwitchableDoor : Switchable
{
    private List<DoorTweenAnimation> openAnimationList = new List<DoorTweenAnimation>();
    private List<DoorTweenAnimation> closeAnimationList = new List<DoorTweenAnimation>();

    private Vector3 _initialTransform;

    // Inspector
    [HideInInspector] public int selectedIndex = 0;

    #region // Inspector properties

    [SerializeField] private float value;
    [SerializeField] private float durationTime;

    #endregion

    #region // DoorTweenAnimation

    private DoorTweenAnimation moveOpenAnimation;
    private DoorTweenAnimation rotateOpenAnimation;
    private DoorTweenAnimation moveCloseAnimation;
    private DoorTweenAnimation rotateCloseAnimation;

    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();
        EnableAnimationLists();
    }

    private void Start()
    {
        _initialTransform = transform.position;
    }

    #region // SwitchableDoor Methods

    // Public Inherit Methods
    public override void Activate()
    {
        OpenAnimation();
        NeedReset = true;
    }

    public override void Disable()
    {
        CloseAnimation();
        NeedReset = true;
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
        AudioManager.Instance.PlaySFX(SoundEffectNames.PORTA_CRISTAL);
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
        //transform.DOMoveY(value, durationTime).SetEase(Ease.InBounce);
        TweenHandler.MoveY(transform, value, durationTime, Ease.Linear);
    }
    private void RotateOpenBounce()
    {
        //transform.DORotate(new Vector3(0f, value, 0f), durationTime).SetEase(Ease.InBounce);
        TweenHandler.Rotate(transform, new Vector3(0f, value, 0f), durationTime, Ease.Linear);
    }

    private void MoveCloseBounce()
    {
        //transform.DOMoveY(0, durationTime).SetEase(Ease.InBounce);
        TweenHandler.MoveY(transform, 0, durationTime, Ease.Linear);
    }
    private void RotateCloseBounce()
    {
        //transform.DORotate(new Vector3(0f, 0f, 0f), durationTime).SetEase(Ease.InBounce);
        TweenHandler.Rotate(transform, new Vector3(0f, 0f, 0f), durationTime, Ease.Linear);
    }

    #endregion 

    // Public Methods
    public void EnableAnimationLists()
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

    // Resetable
    public override void ResetObject()
    {
        
        if (NeedReset)
        {
            Debug.Log("DoorReset");
            transform.position = _initialTransform;

            NeedReset = false;
        }
    }

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

#if UNITY_EDITOR
[CustomEditor(typeof(SwitchableDoor))]
public class SwitchableDoorInspector : Editor
{
    private SwitchableDoor _switchableDoor;

    private void OnEnable()
    {
        _switchableDoor = target as SwitchableDoor;
        _switchableDoor.EnableAnimationLists();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SerializedFields();
    }

    #region // OnInspectorGUI

    private void SerializedFields()
    {
        GUILayout.BeginHorizontal();

        GUILayout.Label("Animation Mode");

        _switchableDoor.selectedIndex = EditorGUILayout.Popup("", _switchableDoor.selectedIndex, _switchableDoor.GetAnimationsNames());

        GUILayout.EndHorizontal();
    }

    #endregion
}
#endif
