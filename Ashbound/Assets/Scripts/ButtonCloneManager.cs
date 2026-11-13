using UnityEngine;

public class ButtonCloneManager : MonoBehaviour
{
    public GameObject _clone;
   
    public void CloneButton()
    {
        GameObject _buttonClone = Instantiate(_clone, transform.position, Quaternion.identity);
        _buttonClone.transform.SetParent(transform.parent);
        Destroy(_buttonClone, 1);
    }
}
