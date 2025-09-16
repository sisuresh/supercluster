// Copyright 2019 Stellar Development Foundation and contributors. Licensed
// under the Apache License, Version 2.0. See the COPYING file at the root
// of this distribution or at http://www.apache.org/licenses/LICENSE-2.0

module MissionMaxTPSSACApply

open StellarCoreSet
open StellarMissionContext
open StellarSupercluster
open StellarNetworkData
open StellarJobExec

let maxTPSSACApplyTests (context: MissionContext) =

    let applyLoadSettings =
        { SimulatedLedgers = 1000
          WriteFrequency = 1000
          BatchSize = 1000
          LastBatchLedgers = 300
          LastBatchSize = 100 }

    let context =
        { context with
              coreResources = AcceptanceTestResources
              applyLoadSettings = Some applyLoadSettings }

    let opts =
        { PubnetCoreSetOptions context.image with
              localHistory = false
              invariantChecks = AllInvariantsExceptBucketConsistencyChecksAndEvents
              initialization = CoreSetInitialization.OnlyNewDb }

    context.ExecuteJobs
        (Some(opts))
        None
        (fun formation ->
            formation.RunSingleJob [| "apply-load"; "--mode"; "max_sac_tps" |] context.image true
            |> formation.CheckAllJobsSucceeded)
