﻿using System.Collections.Generic;
using System.Linq;
using Generator3.Model.Public;

namespace Generator3.Generation.Class.Standard
{
    public class PublicMethodsModel
    {
        private readonly GirModel.Class _class;

        public string Name => _class.Name;
        public string NamespaceName => _class.Namespace.Name;

        public IEnumerable<Method> Methods { get; }

        public PublicMethodsModel(GirModel.Class @class)
        {
            _class = @class;
            Methods = _class.Methods.Select(x => new Method(x, Name));
        }
    }
}