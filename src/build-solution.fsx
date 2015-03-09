// Referenz zu FAKE herstellen
#r @"packages/FAKE/tools/FakeLib.dll"

open Fake

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