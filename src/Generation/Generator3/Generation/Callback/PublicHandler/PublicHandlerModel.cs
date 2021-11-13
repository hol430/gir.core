﻿using System.Collections.Generic;
using Generator3.Model;

namespace Generator3.Generation.Callback
{
    public class PublicHandlerModel
    {
        private readonly GirModel.Callback _callback;
        private ReturnType? _returnType;
        private IEnumerable<Parameter>? _parameters;

        public string Name => _callback.Name + "Handler";
        public string DelegateType => _callback.Name;
        public string NativeDelegateType => _callback.Namespace.GetNativeName() + "." + _callback.Name;
        
        public string NamespaceName => _callback.Namespace.Name;

        public ReturnType ReturnType => _returnType ??= _callback.ReturnType.CreatePublicModel();
        public IEnumerable<Parameter> Parameters => _parameters ??= _callback.Parameters.CreatePublicModel();
        
        public PublicHandlerModel(GirModel.Callback callback)
        {
            _callback = callback;
        }
    }
}
