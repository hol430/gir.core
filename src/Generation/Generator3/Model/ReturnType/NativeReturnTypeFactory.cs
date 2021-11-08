﻿namespace Generator3.Model
{
    public static class NativeReturnTypeFactory
    {
        public static ReturnType CreateNativeModel(this GirModel.ReturnType returnValue) => returnValue.AnyType.Match<ReturnType>(
            type => type switch
            {
                GirModel.String => new Native.StringReturnType(returnValue),
                GirModel.Record => new Native.RecordReturnType(returnValue),
                GirModel.Class => new Native.ClassReturnType(returnValue),
                _ => new Native.StandardReturnType(returnValue)
            },
            arrayType => arrayType.Type switch 
            {
                GirModel.Record => new Native.ArrayRecordReturnType(returnValue),
                _ => new Native.StandardReturnType(returnValue)
            }
        );
    }
}
