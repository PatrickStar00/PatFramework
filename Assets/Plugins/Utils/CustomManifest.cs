
using System.Collections.Generic;

[System.Serializable]
public class DependienciesValue
{
    public List<int> m_values = new List<int>();
}


[System.Serializable]
public class DependenciesDictionary : SerializableDictionary<int, DependienciesValue>
{
}

[System.Serializable]
public class HashDictionary : SerializableDictionary<int, string>
{
}

[System.Serializable]
public class CustomManifest
{
    public DependenciesDictionary m_assetDeps = new DependenciesDictionary();
    public HashDictionary m_hashMap = new HashDictionary();
    [System.NonSerialized]
    public Dictionary<string, List<string>> m_depMapCache = new Dictionary<string, List<string>>();

    public void Insert(string assetName)
    {
        int hashCode = assetName.GetHashCode();
        if(m_hashMap.ContainsKey(hashCode))
        {
            UnityEngine.Debug.LogError("CustomManifest: has two same asset " + assetName+" "+m_hashMap[hashCode]);
            return;
        }
        m_hashMap[hashCode] = assetName;
        m_assetDeps[hashCode] = new DependienciesValue();
    }

    public CustomManifest()
    {
    }

    private HashSet<int> InternalGetAllDependencies(int assetHash)
    {
        HashSet<int> tempSet = new HashSet<int>();
        var direct = InternalGetDirectDependencies(assetHash);
        if (direct == null||direct.Count == 0)
            return null;
        tempSet.UnionWith(direct);
        foreach (var dep in direct)
        {
            var innerDeps = InternalGetAllDependencies(dep);
            if(innerDeps != null)
                tempSet.UnionWith(innerDeps);
        }
        return tempSet;
    }


    public List<string> GetAllDependencies(string assetName)
    {
        if (m_depMapCache.ContainsKey(assetName))
            return m_depMapCache[assetName];
        var innerData = InternalGetAllDependencies(assetName.GetHashCode());
        List<string> shouldCacheData = new List<string>();
        if (innerData != null)
        {
            foreach (var data in innerData)
            {
                shouldCacheData.Add(m_hashMap[data]);
            }
            m_depMapCache[assetName] = shouldCacheData;
        }
        return shouldCacheData;
    }

    private List<int> InternalGetDirectDependencies(int assetHash)
    {
        DependienciesValue value = null;
        if (m_assetDeps.TryGetValue(assetHash, out value))
        {
            return value.m_values;
        }
        else
        {
            UnityEngine.Debug.Log(assetHash);
            return null;
        }
    }

    public List<string> GetDirectDependencies(string assetName)
    {
        int assetHashCode = assetName.GetHashCode();
        List<string> shouldCache = new List<string>();
        List<int> internalDeps = InternalGetDirectDependencies(assetHashCode);
        if (internalDeps != null)
        {
            foreach (var dep in internalDeps)
            {
                string strDep;
                if (m_hashMap.TryGetValue(dep, out strDep))
                {
                    shouldCache.Add(strDep);
                }
            }
        }
        return shouldCache;
    }

}