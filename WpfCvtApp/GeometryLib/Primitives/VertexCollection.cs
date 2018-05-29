using System.Collections;
using System.Collections.Generic;

namespace GeometryLib.Primitives
{
    public class VertexCollection : IList<Vertex>
    {
        private int _elementCouter;
        private List<Vertex> _vertices = new List<Vertex>();

        public VertexCollection()
        {
            _elementCouter = 0;
        }

        public IEnumerator<Vertex> GetEnumerator()
        {
            return _vertices.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Vertex item)
        {
            _vertices.Add(new Vertex(item.X, item.Y, _elementCouter));
        }

        public void Clear()
        {
            _vertices.Clear();
        }

        public bool Contains(Vertex item)
        {
            return _vertices.Contains(item);
        }

        public void CopyTo(Vertex[] array, int arrayIndex)
        {
            _vertices.CopyTo(array, arrayIndex);
        }

        public bool Remove(Vertex item)
        {
            return _vertices.Remove(item);
        }

        public int Count => _vertices.Count;
        public bool IsReadOnly => false;

        public int IndexOf(Vertex item)
        {
            return _vertices.IndexOf(item);
        }

        public void Insert(int index, Vertex item)
        {
            _vertices.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _vertices.RemoveAt(index);
        }

        public Vertex this[int index]
        {
            get => _vertices[index];
            set => _vertices[index] = value;
        }
    }
}