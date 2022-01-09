using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldEnemyList : MonoBehaviour, IEnumerable
{
	public List<EnemyInfo> Enemies;
    public OverworldEnemyList(List<EnemyInfo> enemyList)
    {
        foreach(EnemyInfo ei in enemyList)
        {
            Enemies.Add(ei);
        }

    }
	
	IEnumerator IEnumerable.GetEnumerator()
    {
       return (IEnumerator) GetEnumerator();
    }

    public OverworldEnemyListEnum GetEnumerator()
    {
        return new OverworldEnemyListEnum(Enemies);
    }
}

// When you implement IEnumerable, you must also implement IEnumerator.
public class OverworldEnemyListEnum : IEnumerator
{
    public List<EnemyInfo> _enemies;

    // Enumerators are positioned before the first element
    // until the first MoveNext() call.
    int position = -1;

    public OverworldEnemyListEnum(List<EnemyInfo> list)
    {
        _enemies = list;
    }

    public bool MoveNext()
    {
        position++;
        return (position < _enemies.Count);
    }

    public void Reset()
    {
        position = -1;
    }

    object IEnumerator.Current
    {
        get
        {
            return Current;
        }
    }

    public EnemyInfo Current
    {
        get
        {
            //try
            //{
                return _enemies[position];
            //}
            //catch (IndexOutOfRangeException)
            //{
            //    throw new InvalidOperationException();
            //}
        }
    }
}