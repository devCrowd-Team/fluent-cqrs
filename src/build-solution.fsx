// Referenz zu FAKE herstellen
#r @"packages/FAKE/tools/FakeLib.dll"

open Fake
open Fake.AssemblyInfoFile

let buildOutput = "./build/"
let testBuildOutput = "./build/for_tests/"

let sourceProjectDir = "Fluent-CQRS/"
let testsProjectDir = "Fluent-CQRS.Tests/"

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

    CreateCSharpAssemblyInfo "./Fluent-CQRS/Properties/AssemblyInfo.cs"
        [Attribute.Title "Fluent CQRS Framework"
         Attribute.Description "The next generation of CQRS Framework."
         Attribute.Company ""
         Attribute.Copyright "Copyright © Jan Fellien"
         Attribute.Guid "41246c8a-9b02-4be4-83ec-c88a26ee6054"
         Attribute.Product "Fluent-CQRS"
         Attribute.Version version
         Attribute.FileVersion version]

    !! (sourceProjectDir + "*.csproj")
        |> MSBuildRelease buildOutput "Build"
        |> Log "AppBuild-Output: "
)

Target "Build" (fun _ ->

    !! (sourceProjectDir + "*.csproj")
        |> MSBuildRelease buildOutput "Build"
        |> Log "AppBuild-Output: "
)

Target "Clean BuildResults" (fun _ ->
    !!(buildOutput + "*.pdb")
        |> DeleteFiles 
)

//Target "Build Tests" (fun _ ->
//
//    !! (testsProjectDir + "*.csproj")
//        |> MSBuildRelease testBuildOutput "Build"
//        |> Log "AppBuild-Output: "
//)

//Target "Run Tests" (fun _ ->

//    !! (testBuildOutput + "*Tests.dll")
//      |> MSpec (fun p -> {p with ToolPath = mspecExePath})
//)

Target "Last Step" (fun _ ->
    printf "Build with FAKE\r\n";
)

// Dependencies
"Clean"
    ==> "Build"
    ==> "Clean BuildResults"
    ==> "Last Step"

RunTargetOrDefault "Last Step"