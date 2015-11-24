// Referenz zu FAKE herstellen
#r @"../packages/FAKE/tools/FakeLib.dll"

open Fake
open Fake.AssemblyInfoFile

let name = "Fluent-CQRS"
let title = "Fluent CQRS Framework"
let description = "The next generation of CQRS Framework."
let id = "41246c8a-9b02-4be4-83ec-c88a26ee6054"
let authors = ["Jan Fellien"; "Carsten Koenig"]
let tags = "CQRS, Event Sourcing, Event Sore, DDD, Domain Driven Design"

let buildOutput = "./build/"
let testBuildOutput = "./build/for_tests/"
let publishDir = "./build/publish/"
let sourceProjectDir = "src/Fluent-CQRS/"
let testsProjectDir = "src/Fluent-CQRS.Tests/"
let builtAssembly = name + ".dll"
let toolsPath = "tools/"
let nugetPath = toolsPath + "NuGet/NuGet.exe"
let nunitPath = @".\packages\NUnit.Runners\tools\"
let nunitExecutable = "nunit-console.exe"

let version =
  match buildServer with
  | TeamCity -> buildVersion
  | _ -> "0.0.1"

RestorePackages

// Targets
Target "Clean" (fun _ ->
    CleanDir buildOutput
    CleanDir testBuildOutput
)

Target "Build" (fun _ ->

    CreateCSharpAssemblyInfo ("./src/" + name + "/Properties/AssemblyInfo.cs")
        [Attribute.Title title
         Attribute.Description description
         Attribute.Company ""
         Attribute.Copyright "Copyright © Jan Fellien"
         Attribute.Guid id
         Attribute.Product name
         Attribute.Version version
         Attribute.FileVersion version]

    !! (sourceProjectDir + "*.csproj")
        |> MSBuildRelease buildOutput "Build"
        |> Log "AppBuild-Output: "

    !! (testsProjectDir + "*.csproj")
        |> MSBuildRelease testBuildOutput "Build"
        |> Log "TestBuild-Output: "
)

Target "Clean BuildResults" (fun _ ->
    !!(buildOutput + "*.pdb")
        |> DeleteFiles 

    !!(testBuildOutput + "*.pdb")
        |> DeleteFiles 
)

Target "Run Tests" (fun _ ->
    !! (testBuildOutput + "/*.Tests.dll")
        |> NUnit (fun p -> 
            {p with
                ToolPath = nunitPath
                ToolName = nunitExecutable
                DisableShadowCopy = true; 
                OutputFile = testBuildOutput + "/TestResults.xml"})


)

Target "Package" (fun _ ->
    CopyFiles publishDir !! (buildOutput @@ builtAssembly)

    NuGet (fun p ->
        {p with
            ToolPath = nugetPath
            Project = name
            Authors = authors
            Description = description
            Summary = description
            Tags = tags
            Version = version
            OutputPath = publishDir
            WorkingDir = publishDir
            Files = [builtAssembly, Some "lib/portable-net45+netcore45+wpa81", None] })
            "src/package.nuspec"
)

// Dependencies
"Clean"
    ==> "Build"
    ==> "Clean BuildResults"
    ==> "Run Tests"
       ==> "Package"

RunTargetOrDefault "Package"