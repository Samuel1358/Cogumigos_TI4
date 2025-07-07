using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDataController : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject _level02Button;
    [SerializeField] private GameObject _level03Button;
    private bool _isLevelComplited = false;

    public bool IsLevelComplited { get {  return _isLevelComplited; } set { _isLevelComplited = value; } }

    public void LoadData(GameData data)
    {
        SetActiveButton02(false);
        SetActiveButton03(false);

        if (data == null)
            return;

        foreach (var levelData in data.Levels)
        {
            if (levelData.Key == "Level01")
            {
                SetActiveButton02(levelData.Value);
            }
            else if (levelData.Key == "Level02")
            {
                SetActiveButton03(levelData.Value);
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        if (!_isLevelComplited)
            return;

        if (!data.Levels.ContainsKey(SceneManager.GetActiveScene().name))
            data.Levels.Add(SceneManager.GetActiveScene().name, true);
    }

    #region // Buttons

    private void SetActiveButton02(bool value)
    {
        if (_level02Button != null)
            _level02Button.SetActive(value);
    }

    private void SetActiveButton03(bool value)
    {
        if (_level03Button != null)
            _level03Button.SetActive(value);
    }

    #endregion
}
