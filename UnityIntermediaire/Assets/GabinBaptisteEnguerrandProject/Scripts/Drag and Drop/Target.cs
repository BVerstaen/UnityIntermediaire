using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public enum placement
    {
        Invalid,
        Valid,
        Docker
    };

    public placement _current;
}
