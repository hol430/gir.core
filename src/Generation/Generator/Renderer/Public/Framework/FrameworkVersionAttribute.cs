﻿using Generator.Model;

namespace Generator.Renderer.Public;

internal static class FrameworkVersionAttribute
{
    public static string Render(GirModel.Namespace ns)
    {
        return $@"namespace {Namespace.GetPublicName(ns)}
{{
    // AUTOGENERATED FILE - DO NOT MODIFY

    public class VersionAttribute : System.Attribute
    {{
        public string Version {{ get; }}

        public VersionAttribute(string version)
        {{
            Version = version;
        }}
    }}
}}";
    }
}