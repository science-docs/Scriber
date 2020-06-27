using System;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Engine
{
    public static class ElementConverters
    {
        private static readonly Dictionary<Tuple<Type, Type>, IElementConverter> converters = new Dictionary<Tuple<Type, Type>, IElementConverter>();
        private static readonly Dictionary<Type, List<Type>> types = new Dictionary<Type, List<Type>>();

        public static void Add(IElementConverter converter, Type source, params Type[] targets)
        {
            foreach (var target in targets)
            {
                if (!types.TryGetValue(source, out var targetTypes))
                {
                    types[source] = new List<Type> { target };
                }
                else
                {
                    targetTypes.Add(target);
                }

                converters[new Tuple<Type, Type>(source, target)] = converter;
            }
        }

        public static void ResolvePaths()
        {
            foreach (var type in types.Keys)
            {
                var node = new TypeTreeNode(null, type);
                ResolvePath(type, node);
                var branches = node.GetBranches();

                foreach (var branch in branches.Where(e => e.Count > 1))
                {
                    var mergedConverter = ConvertBranchToConverter(branch);
                    var lastType = branch[^1].Item1;
                    Add(mergedConverter, type, lastType);
                }
            }
        }

        private static void ResolvePath(Type type, TypeTreeNode tree)
        {
            if (types.TryGetValue(type, out var childTypes))
            {
                foreach (var childType in childTypes)
                {
                    var tuple = new Tuple<Type, Type>(type, childType);
                    if (converters.ContainsKey(tuple) && tree.CanAddType(childType))
                    {
                        var node = new TypeTreeNode(tree, childType);
                        tree.Add(node, converters[tuple]);
                        ResolvePath(childType, node);
                    }
                }
            }
        }

        private static MergedElementConverter ConvertBranchToConverter(List<Tuple<Type, IElementConverter>> branch)
        {
            var converter = new MergedElementConverter(branch[0].Item2);

            for (int i = 1; i < branch.Count; i++)
            {
                converter.Add(branch[i - 1].Item1, branch[i].Item2);
            }

            return converter;
        }

        public static IElementConverter? Find(Type source, Type target)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (target.IsEnum)
            {
                target = typeof(Enum);
            }

            converters.TryGetValue(new Tuple<Type, Type>(source, target), out var converter);
            return converter;
        }

        public static object? Convert(object? source, Type target)
        {
            if (source == null)
            {
                return null;
            }

            var type = source.GetType();
            var converter = Find(type, target);

            if (converter != null)
            {
                return converter.Convert(source, target);
            }
            else
            {
                return null;
            }
        }

        private class TypeTreeNode
        {
            public TypeTreeNode? Parent { get; }
            public Type From { get; }

            private readonly Dictionary<Type, Tuple<TypeTreeNode, IElementConverter>> nodes = new Dictionary<Type, Tuple<TypeTreeNode, IElementConverter>>();

            public TypeTreeNode(TypeTreeNode? parent, Type from)
            {
                Parent = parent;
                From = from;
            }

            public void Add(TypeTreeNode node, IElementConverter converter)
            {
                nodes.Add(node.From, new Tuple<TypeTreeNode, IElementConverter>(node, converter));
            }

            public bool CanAddType(Type type)
            {
                if (type == From)
                {
                    return false;
                }

                if (Parent != null)
                {
                    if (!Parent.CanAddType(type))
                    {
                        return false;
                    }
                }

                return true;
            }

            public List<List<Tuple<Type, IElementConverter>>> GetBranches()
            {
                var branches = new List<List<Tuple<Type, IElementConverter>>>();
                
                foreach (var child in nodes)
                {
                    var node = child.Value.Item1;
                    var type = child.Key;
                    var converter = child.Value.Item2;

                    var childBranches = node.GetBranches();
                    var tuple = new Tuple<Type, IElementConverter>(type, converter);

                    foreach (var childBranch in childBranches)
                    {
                        childBranch.Insert(0, tuple);
                        branches.Add(childBranch);
                    }

                    if (childBranches.Count == 0)
                    {
                        branches.Add(new List<Tuple<Type, IElementConverter>>
                        {
                            tuple
                        });
                    }
                }

                return branches;
            }


        }
    }
}
