using System;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Engine
{
    public class ConverterCollection
    {
        private readonly Dictionary<(Type, Type), IElementConverter> converters = new Dictionary<(Type, Type), IElementConverter>();
        private readonly Dictionary<Type, List<Type>> types = new Dictionary<Type, List<Type>>();

        public void Add(IElementConverter converter, Type source, params Type[] targets)
        {
            if (converter is null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (targets is null)
            {
                throw new ArgumentNullException(nameof(targets));
            }

            foreach (var target in targets)
            {
                if (target == null)
                {
                    throw new ArgumentException("A target type is null", nameof(targets));
                }

                if (!types.TryGetValue(source, out var targetTypes))
                {
                    types[source] = new List<Type> { target };
                }
                else
                {
                    targetTypes.Add(target);
                }

                converters[(source, target)] = converter;
            }
        }

        public void ResolvePaths()
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

        private void ResolvePath(Type type, TypeTreeNode tree)
        {
            if (types.TryGetValue(type, out var childTypes))
            {
                foreach (var childType in childTypes)
                {
                    var tuple = (type, childType);
                    if (converters.ContainsKey(tuple) && tree.CanAddType(childType))
                    {
                        var node = new TypeTreeNode(tree, childType);
                        tree.Add(node, converters[tuple]);
                        ResolvePath(childType, node);
                    }
                }
            }
        }

        private MergedElementConverter ConvertBranchToConverter(List<(Type node, IElementConverter converter)> branch)
        {
            var converter = new MergedElementConverter(branch[0].converter);

            for (int i = 1; i < branch.Count; i++)
            {
                converter.Add(branch[i - 1].node, branch[i].converter);
            }

            return converter;
        }

        public IElementConverter? Find(Type source, Type target)
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

            converters.TryGetValue((source, target), out var converter);
            return converter;
        }

        public bool TryConvert(object source, Type target, out object value)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            var type = source.GetType();
            var converter = Find(type, target);

            if (converter != null)
            {
                try
                {
                    value = converter.Convert(source, target);
                }
                catch (ConverterException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new ConverterException(type, target, ex);
                }
                return true;
            }
            else
            {
                value = source;
                return false;
            }
        }

        private class TypeTreeNode
        {
            public TypeTreeNode? Parent { get; }
            public Type From { get; }

            private readonly List<(Type from, TypeTreeNode node, IElementConverter converter)> nodes = new List<(Type, TypeTreeNode, IElementConverter)>();

            public TypeTreeNode(TypeTreeNode? parent, Type from)
            {
                Parent = parent;
                From = from;
            }

            public void Add(TypeTreeNode node, IElementConverter converter)
            {
                nodes.Add((node.From, node, converter));
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

            public List<List<(Type, IElementConverter)>> GetBranches()
            {
                var branches = new List<List<(Type, IElementConverter)>>();
                
                foreach (var child in nodes)
                {
                    var node = child.node;
                    var type = child.from;
                    var converter = child.converter;

                    var childBranches = node.GetBranches();
                    var tuple = (type, converter);

                    foreach (var childBranch in childBranches)
                    {
                        childBranch.Insert(0, tuple);
                        branches.Add(childBranch);
                    }

                    if (childBranches.Count == 0)
                    {
                        branches.Add(new List<(Type, IElementConverter)>
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
