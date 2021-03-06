﻿// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
    // https://stackoverflow.com/questions/52635056/why-teamcity-build-fails-with-because-it-is-being-used-by-another-process-doe/55694056
    readonly DotNetVerbosity DotNetVerbosity = IsLocalBuild ? DotNetVerbosity.Detailed : DotNetVerbosity.Normal;

    [Solution("Teronis.DotNet.sln")]
    readonly Solution Solution;

    [Solution("Teronis.DotNet~Publish.sln")]
    readonly Solution PublishSolution;

    //[GitRepository] readonly GitRepository GitRepository;

    // GITVERSION... Forces "... process is used by another process"...
    //[GitVersion(Framework = "netcoreapp3.1")]
    //readonly GitVersion GitVersion;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath PackagesDirectory => RootDirectory / "obj" / "packages";
    AbsolutePath LibBuiltDirectory => RootDirectory / "lib/built";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() => {
            SourceDirectory
                .GlobFiles("**/*.csproj")
                .ForEach(path => {
                    DeleteDirectory(path.Parent / "bin");
                    DeleteDirectory(path.Parent / "obj");
                });

            DeleteDirectory(RootDirectory / "obj");
        });

    Target Restore => _ => _
        .Executes(() => {
            DotNetRestore(s => s
                .SetProjectFile(Solution)
                .SetVerbosity(DotNetVerbosity));
        });

    Target CleanBinary => _ => _
        .Before(CompileBinary)
        .Executes(() => {
            DeleteDirectory(LibBuiltDirectory);
        });

    Target CompileBinary => _ => _
         .DependsOn(Restore)
         .Executes(() => {
             Solution.AllProjects.ForEach(project => {
                 var isLocalBinary = project.GetMSBuildProject().GetPropertyValue("LocalBinary");

                 if (string.IsNullOrEmpty(isLocalBinary) || isLocalBinary != "true") {
                     return;
                 }

                 var binaryDirectory = LibBuiltDirectory / project.Name;

                 DotNetPublish(s => s
                     .SetProject(project)
                     .SetConfiguration(Configuration.Release)
                     .SetOutput(binaryDirectory)
                     // We assume that every local binary project supports net472.
                     .SetFramework("net472")
                     .EnableNoRestore());
             });
         });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() => {
            DotNetBuild(dotNetBuildSettings => {
                return dotNetBuildSettings
                    .SetProjectFile(Solution)
                    .SetConfiguration(Configuration)
                    // See above comment.
                    //.SetAssemblyVersion(GitVersion.AssemblySemVer)
                    //.SetFileVersion(GitVersion.AssemblySemFileVer)
                    //.SetInformationalVersion(GitVersion.InformationalVersion)
                    .EnableNoRestore()
                    .SetVerbosity(DotNetVerbosity)
                    .SetProcessArgumentConfigurator(arguments => arguments.Add("-nodeReuse:false"));
            });
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() => {
            SourceDirectory.GlobFiles("**/*.Test.csproj", "**/*.E2ETest.csproj")
                .ForEach(testProjectPath => {
                    DotNetTest(s => s
                        .SetProjectFile(testProjectPath)
                        .SetConfiguration(Configuration)
                        .EnableNoRestore()
                        //.SetProcessArgumentConfigurator(arguments =>
                        //    arguments.Add("--filter FullyQualifiedName!=Teronis.Microsoft.JSInterop.Facades.JSFacadeTests", IsServerBuild))
                        );
                });
        });

    Target Pack => _ => _
        .DependsOn(Compile)
        .Executes(() => {
            DotNetPack(s => s
                .SetProject(PublishSolution)
                // See above comment.
                //.SetAssemblyVersion(GitVersion.AssemblySemVer)
                //.SetFileVersion(GitVersion.AssemblySemFileVer)
                //.SetInformationalVersion(GitVersion.InformationalVersion)
                .SetConfiguration(Configuration)
                .SetVerbosity(DotNetVerbosity)
                .SetProcessArgumentConfigurator(arguments => arguments.Add("-nodeReuse:false")));

            PublishSolution.Projects.ForEach(project => {
                project.Directory.GlobFiles("**/*.nupkg")
                    .ForEach(packagePath => MoveFileToDirectory(packagePath, PackagesDirectory, createDirectories: true));
            });
        });
}
