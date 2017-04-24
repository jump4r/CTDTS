using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomHelper {
    
    public static int RandomSign()
    {
        return Random.Range(0, 2) * 2 - 1;
    }

    // How fast debris shoudl move
    public static float RandomDebrisMovementSpeed()
    {
        return Random.Range(.5f, 1f);
    }

    public static float RandomAsteroidMass()
    {
        return Random.Range(.5f, 10f);
    }

    public static float RandomAsteroidMoveSpeed()
    {
        return Random.Range(1f, 3f);
    }
}
