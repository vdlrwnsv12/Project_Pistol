using UnityEngine;

public static class ExtensionMethod
{
    /// <summary>
    /// 이름으로 직계 자식(Child) 오브젝트를 찾는다.
    /// </summary>
    public static Transform FindChildByName(this Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;
        }

        return null;
    }

    /// <summary>
    /// 이름으로 모든 하위(재귀 포함) 오브젝트를 찾는다.
    /// </summary>
    public static Transform FindDeepChildByName(this Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;

            Transform found = child.FindDeepChildByName(name);
            if (found != null)
                return found;
        }

        return null;
    }
}
