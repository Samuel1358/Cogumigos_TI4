using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;

//[RequireComponent(typeof(Animation))]
public class SwitchableDoor : Switchable
{
    [SerializeField] public List<DoorTweenAnimation> animationList { get; private set; } = new List<DoorTweenAnimation>();
    private List<DoorTweenAnimation> animationCloseList = new List<DoorTweenAnimation>();
    [SerializeField] public int selectedIndex = 0;

    [SerializeField] public float value;

    public void OnEnable()
    {
        BasicOpenAnimation basicOpenAnimation = new BasicOpenAnimation(this);
        RotateOpenAnimation rotateOpenAnimation = new RotateOpenAnimation(this);

        BasicCloseAnimation basicCloseAnimation = new BasicCloseAnimation();
        RotateCloseAnimation rotateCloseAnimation = new RotateCloseAnimation();

        animationList.Add(basicOpenAnimation);
        animationList.Add(rotateOpenAnimation);

        animationCloseList.Add(basicCloseAnimation);
        animationCloseList.Add(rotateCloseAnimation);
    }

    // Public Inherit Methods
    public override void Activate()
    {
        OpenAnimation();
    }

    public override void Disable()
    {
        CloseAnimation();
    }

    // Private Methods
    [ContextMenu("OpenAnimation")]
    private void OpenAnimation()
    {
        animationList[selectedIndex].TweenAnimation(transform);       
    }

    [ContextMenu("CloseAnimation")]
    private void CloseAnimation()
    {
        animationCloseList[selectedIndex].TweenAnimation(transform);
    }

    #region // Animations

    internal class BasicOpenAnimation : DoorTweenAnimation
    {
        private SwitchableDoor _switchableDoor;
        private SerializedObject _serializedObject;

        private SerializedProperty _value;

        public BasicOpenAnimation(SwitchableDoor obj) : base("BasicOpenAnimation")
        {
            _switchableDoor = obj;
            _serializedObject = new SerializedObject(_switchableDoor);

            _value = _serializedObject.FindProperty("value");
        }

        public override void TweenAnimation(Transform transform)
        {
            transform.DOMoveY(_switchableDoor.value, 1.5f).SetEase(Ease.InBounce);
        }

        public override void InspectorGUI()
        {
            _serializedObject.Update();

            GUILayout.Label("Move");
            EditorGUILayout.PropertyField(_value);

            _serializedObject.ApplyModifiedProperties();
        }
    }

    internal class BasicCloseAnimation : DoorTweenAnimation
    {
        public BasicCloseAnimation() : base("BasicClosenAnimation") { }

        public override void TweenAnimation(Transform transform)
        {
            transform.DOMoveY(0f, 1.5f).SetEase(Ease.InBounce);
        }

        public override void InspectorGUI() { }
    }

    internal class RotateOpenAnimation : DoorTweenAnimation
    {
        private SwitchableDoor _switchableDoor;
        private SerializedObject _serializedObject;

        private SerializedProperty _value;

        public RotateOpenAnimation(SwitchableDoor obj) : base("RotateOpenAnimation")
        {
            _switchableDoor = obj;
            _serializedObject = new SerializedObject(_switchableDoor);

            _value = _serializedObject.FindProperty("value");
        }

        public override void TweenAnimation(Transform transform)
        {
            transform.DORotate(new Vector3(0f, _switchableDoor.value, 0f), 1.5f).SetEase(Ease.InBounce);
        }

        public override void InspectorGUI()
        {
            _serializedObject.Update();

            GUILayout.Label("Rotate");
            EditorGUILayout.PropertyField(_value);

            _serializedObject.ApplyModifiedProperties();
        }
    }

    internal class RotateCloseAnimation : DoorTweenAnimation
    {
        public RotateCloseAnimation() : base("RotateClosenAnimation") { }

        public override void TweenAnimation(Transform transform)
        {
            transform.DORotate(new Vector3(0f, 0f, 0f), 1.5f).SetEase(Ease.InBounce);
        }

        public override void InspectorGUI() { }
    }

    #endregion

    public abstract class DoorTweenAnimation
    {
        public string name;

        public DoorTweenAnimation(string name)
        {
            this.name = name;
        }

        public abstract void TweenAnimation(Transform transform);
        public abstract void InspectorGUI();
    }
}

[CustomEditor(typeof(SwitchableDoor))]
public class SwitchableDoorInspector : Editor
{
    private SwitchableDoor _switchableDoor;

    private void OnEnable()
    {
        _switchableDoor = target as SwitchableDoor;
        _switchableDoor.OnEnable();
    }

    public override void OnInspectorGUI()
    {
        _switchableDoor.selectedIndex = EditorGUILayout.Popup("", _switchableDoor.selectedIndex, GetAnimationsNames());

        _switchableDoor.animationList[_switchableDoor.selectedIndex].InspectorGUI();
    }

    public string[] GetAnimationsNames()
    {
        List<string> names = new List<string>();
        foreach (SwitchableDoor.DoorTweenAnimation aux in _switchableDoor.animationList)
        {
            names.Add(aux.name);
        }

        return names.ToArray();
    }
}
