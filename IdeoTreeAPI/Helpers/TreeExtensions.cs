﻿

namespace IdeoTreeAPI.Helpers
{
    public static class TreeExtensions
    {

        public interface ITree<T> : IEnumerable<ITree<T>>
        {
            public int Id { get; set; }
            T Data { get; }
            ITree<T> Parent { get; }
            ICollection<ITree<T>> Children { get; }
            bool IsRoot { get; }
            bool IsLeaf { get; }
            int Level { get; }
        }
        public static IEnumerable<TNode> Flatten<TNode>(this IEnumerable<TNode> nodes, Func<TNode, IEnumerable<TNode>> childrenSelector)
        {
            if (nodes == null) throw new ArgumentNullException(nameof(nodes));
            return nodes.SelectMany(c => childrenSelector(c).Flatten(childrenSelector)).Concat(nodes);
        }

        public static ITree<T> ToTree<T>(this IList<T> items, Func<T, T, bool> parentSelector)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            var lookup = items.ToLookup(item => items.FirstOrDefault(parent => parentSelector(parent, item)),
                child => child);
            return Tree<T>.FromLookup(lookup);
        }
        internal class Tree<T> : ITree<T>
        {
            public int Id { get; set; }
            public T Data { get; }
            public ITree<T> Parent { get; private set; }
            public ICollection<ITree<T>> Children { get; }
            public bool IsRoot => Parent == null;
            public bool IsLeaf => Children.Count == 0;
            public int Level => IsRoot ? 0 : Parent.Level + 1;
            internal Tree(T data)
            {
                Children = new LinkedList<ITree<T>>();
                Data = data;
            }
            public static Tree<T> FromLookup(ILookup<T, T> lookup)
            {
                var rootData = lookup.Count == 1 ? lookup.First().Key : default(T);
                var root = new Tree<T>(rootData);
                root.LoadChildren(lookup);
                return root;
            }
            private void LoadChildren(ILookup<T, T> lookup)
            {
                foreach (var data in lookup[Data])
                {
                    var child = new Tree<T>(data) { Parent = this };
                    Children.Add(child);
                    child.LoadChildren(lookup);
                }
            }
            internal Tree<T> AddChild(T data)
            {
                var node = new Tree<T>(data);
                this.Children.Add(node);
                node.Parent = this;
                return node;
            }

            public IEnumerator<ITree<T>> GetEnumerator()
            {
                yield return this;
                foreach (var directChild in this.Children)
                {
                    foreach (var anyChild in directChild)
                        yield return anyChild;
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
