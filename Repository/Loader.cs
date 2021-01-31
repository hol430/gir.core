﻿using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Repository.Graph;
using Repository.Services;
using Repository.Xml;

#nullable enable

namespace Repository
{
    public class Loader
    {
        private readonly IResolver<ILoadedProject> _resolver;
        private readonly IInfoFactory _infoFactory;
        private readonly IXmlService _xmlService;
        private readonly INamespaceInfoConverterService _namespaceInfoConverterService;
        private ResolveFileFunc? _lookupFunc;

        private bool _projectLoadFailed;

        public Loader(IResolver<ILoadedProject> resolver, IInfoFactory infoFactory, IXmlService xmlService, INamespaceInfoConverterService namespaceInfoConverterService)
        {
            _resolver = resolver;
            _infoFactory = infoFactory;
            _xmlService = xmlService;
            _namespaceInfoConverterService = namespaceInfoConverterService;
        }

        // Where 'targets' are toplevel projects. Loader resolves
        // dependent gir libraries automatically.
        public IEnumerable<ILoadedProject> LoadOrdered(IEnumerable<string> targets, ResolveFileFunc lookupFunc)
        {
            _lookupFunc = lookupFunc;
            var loadedProjects = new List<ILoadedProject>();

            foreach (var target in targets)
                LoadAndAddProjects(loadedProjects, target);

            if (_projectLoadFailed)
                Log.Warning($"Failed to load some projects. Please check the log for more information.");

            return _resolver.ResolveOrdered(loadedProjects).Cast<ILoadedProject>();
        }

        private void LoadAndAddProjects(ICollection<ILoadedProject> loadedProjects, string target)
        {
            var info = new FileInfo(target);
            _ = LoadRecursive(loadedProjects, info);
        }

        private ILoadedProject? LoadRecursive(ICollection<ILoadedProject> loadedProjects, FileInfo target)
        {
            try
            {
                var repoinfo = LoadRepositoryInfo(target);
                var repositoryInfoData = _infoFactory.CreateFromNamespaceInfo(repoinfo.Namespace);

                if (TryLoadProject(loadedProjects, repositoryInfoData, out ILoadedProject? project))
                    return project;

                var dependencies = LoadDependencies(loadedProjects, repoinfo.Includes);
                
                //TODO: References obsolete?
                var (nspace, references) = _namespaceInfoConverterService.Convert(repoinfo.Namespace);
                
                project = new LoadedProject(nspace.ToCanonicalName(), nspace, dependencies);
                loadedProjects.Add(project);
                Log.Information($"Loaded '{nspace.ToCanonicalName()}' (provided by '{target.Name}')");

                return project;
            }
            catch (Exception e)
            {
                Log.Error($"Failed to load gir file '{target.Name}'.");
                Log.Exception(e);
                _projectLoadFailed = true;

                return null;
            }
        }

        private IEnumerable<ILoadedProject> LoadDependencies(ICollection<ILoadedProject> loadedProjects, IEnumerable<IncludeInfo> includes)
        {
            var dependencies = new List<ILoadedProject>();
            foreach (var dependency in _infoFactory.CreateFromIncludes(includes))
            {
                FileInfo dependentGirFile = GetGirFileInfo(dependency);
                var dependentProject = LoadRecursive(loadedProjects, dependentGirFile);

                if (dependentProject is not null)
                    dependencies.Add(dependentProject);
            }

            return dependencies;
        }

        private bool TryLoadProject(IEnumerable<ILoadedProject> loadedProjects, Info repo, [NotNullWhen(true)] out ILoadedProject? loadedProject)
        {
            var foundProjects = loadedProjects.Where(x => NameMatches(x, repo)).ToArray();
            switch (foundProjects.Length)
            {
                case 0:
                    loadedProject = null;
                    return false;
                case 1:
                    loadedProject = foundProjects[0];
                    return true;
                default:
                    throw new Exception("Inconsistent data. Projects are loaded multiple times. Aborting");
            }
        }

        private bool NameMatches(ILoadedProject loadedProject, Info repo)
            => loadedProject.Name == repo.ToCanonicalName();

        private FileInfo GetGirFileInfo(Info data)
        {
            if (_lookupFunc is null)
                throw new Exception("Lookup func is not initialized");

            return _lookupFunc(data.Name, data.Version);
        }

        private RepositoryInfo LoadRepositoryInfo(FileInfo target)
        {
            var repoInfo = _xmlService.Deserialize<RepositoryInfo>(target);

            if (repoInfo.Namespace == null)
                throw new InvalidDataException($"File '{target} does not define a namespace.");

            return repoInfo;
        }
    }
}
