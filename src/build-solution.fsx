// Referenz zu FAKE herstellen
#r @"packages/FAKE/tools/FakeLib.dll"

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
let sourceProjectDir = "Fluent-CQRS/"
let testsProjectDir = "Fluent-CQRS.Tests/"
let builtAssembly = name + ".dll"

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

    CreateCSharpAssemblyInfo ("./" + name + "/Properties/AssemblyInfo.cs")
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
)

Target "Clean BuildResults" (fun _ ->
    !!(buildOutput + "*.pdb")
        |> DeleteFiles 
)

Target "Package" (fun _ ->
    CopyFiles publishDir !! (buildOutput @@ builtAssembly)

    NuGet (fun p ->
        {p with
            Project = name
            Authors = authors
            Description = description
            Summary = description
            Tags = tags
            Version = version
            OutputPath = publishDir
            WorkingDir = publishDir
            Files = [builtAssembly, Some "lib/portable-net40+sl50+win+wpa81+wp80", None] })
            "package.nuspec"
)

Target "Last Step" (fun _ ->
    printf "Build with FAKE\r\n";
)

// Dependencies
"Clean"
    ==> "Build"
    ==> "Clean BuildResults"
    ==> "Package"
    ==> "Last Step"

RunTargetOrDefault "Last Step"