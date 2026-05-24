using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ZeroPoint.Core;

public class QuadTree<T>
{
    private readonly Rectangle _bounds;
    private readonly List<T> _objects;
    private readonly QuadTree<T>[] _nodes;
    private readonly int _capacity;
    private readonly Func<T, Rectangle> _boundsAccessor;

    public QuadTree(Rectangle bounds, Func<T, Rectangle> boundsAccessor, int capacity = 4)
    {
        _bounds = bounds;
        _boundsAccessor = boundsAccessor ?? throw new ArgumentNullException(nameof(boundsAccessor));
        _capacity = Math.Max(1, capacity);
        _objects = new List<T>();
        _nodes = new QuadTree<T>[4];
    }

    public void Insert(T item)
    {
        var itemBounds = _boundsAccessor(item);

        if (!_bounds.Intersects(itemBounds))
        {
            return;
        }

        if (_nodes[0] != null)
        {
            int index = GetChildIndex(itemBounds);
            if (index != -1)
            {
                _nodes[index].Insert(item);
                return;
            }
        }

        _objects.Add(item);

        if (_objects.Count > _capacity && _nodes[0] == null)
        {
            Split();

            for (int i = _objects.Count - 1; i >= 0; i--)
            {
                var o = _objects[i];
                int index = GetChildIndex(_boundsAccessor(o));
                if (index != -1)
                {
                    _nodes[index].Insert(o);
                    _objects.RemoveAt(i);
                }
            }
        }
    }

    public List<T> Query(Rectangle area, List<T> result)
    {
        if (!_bounds.Intersects(area))
            return result;

        foreach (var obj in _objects)
        {
            if (area.Intersects(_boundsAccessor(obj)))
            {
                result.Add(obj);
            }
        }

        if (_nodes[0] != null)
        {
            foreach (var node in _nodes)
            {
                node.Query(area, result);
            }
        }

        return result;
    }

    private void Split()
    {
        int halfWidth = Math.Max(1, _bounds.Width / 2);
        int halfHeight = Math.Max(1, _bounds.Height / 2);

        _nodes[0] = new QuadTree<T>(new Rectangle(_bounds.X, _bounds.Y, halfWidth, halfHeight), _boundsAccessor, _capacity);
        _nodes[1] = new QuadTree<T>(new Rectangle(_bounds.X + halfWidth, _bounds.Y, halfWidth, halfHeight), _boundsAccessor, _capacity);
        _nodes[2] = new QuadTree<T>(new Rectangle(_bounds.X, _bounds.Y + halfHeight, halfWidth, halfHeight), _boundsAccessor, _capacity);
        _nodes[3] = new QuadTree<T>(new Rectangle(_bounds.X + halfWidth, _bounds.Y + halfHeight, halfWidth, halfHeight), _boundsAccessor, _capacity);
    }

    private int GetChildIndex(Rectangle rect)
    {
        int verticalMidpoint = _bounds.X + _bounds.Width / 2;
        int horizontalMidpoint = _bounds.Y + _bounds.Height / 2;

        bool topQuadrant = rect.Y < horizontalMidpoint && rect.Bottom <= horizontalMidpoint;
        bool bottomQuadrant = rect.Y >= horizontalMidpoint;
        bool leftQuadrant = rect.X < verticalMidpoint && rect.Right <= verticalMidpoint;
        bool rightQuadrant = rect.X >= verticalMidpoint;

        if (leftQuadrant)
        {
            if (topQuadrant)
                return 0;
            if (bottomQuadrant)
                return 2;
        }
        else if (rightQuadrant)
        {
            if (topQuadrant)
                return 1;
            if (bottomQuadrant)
                return 3;
        }

        return -1;
    }
}
