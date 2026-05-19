using Oculus.Interaction;
using UnityEngine;

public abstract class IThrowable : InteractableUnityEventWrapper
{
    public abstract void Grab();
    public abstract void Throw();
}
