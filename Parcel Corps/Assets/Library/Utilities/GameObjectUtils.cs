using UnityEngine;

public static class GameObjectUtils
{
  public static int FindClosestTransformIndex(Transform source, Transform[] objects)
  {
    if (source == null || objects == null || objects.Length == 0)
      return -1;

    float closestDistance = float.MaxValue;
    int closestIndex = -1;

    for (int i = 0; i < objects.Length; i++)
    {
      if (objects[i] == null)
      {
        continue;
      }

      float distance = Vector3.Distance(source.position, objects[i].position);
      if (distance < closestDistance)
      {
        closestDistance = distance;
        closestIndex = i;
      }
    }

    return closestIndex;
  }
}