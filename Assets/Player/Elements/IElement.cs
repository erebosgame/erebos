using UnityEngine;
public interface IElement
{
    void Start();
    void End();
    void Update(float delta);
    void Trigger(Collider other);
}

public interface OnEndListener
{
    void OnEnd();
}