namespace DomainDrivers.SmartSchedule.Planning.Parallelization;

public class StageParallelization
{
    public ParallelStagesList Of(ISet<Stage> allStages)
    {
        var steps = ParallelStagesList.Empty();
        steps = AddFirstStep(allStages, steps);
        ISet<Stage> lastStages = steps.All.Last().Stages;
        
        steps = AddNextStep(lastStages, steps, allStages);
        return steps;
    }

    private static ParallelStagesList AddFirstStep(ISet<Stage> stages, ParallelStagesList steps)
    {
        var noDepsStages = stages.Where(x => !x.Dependencies.Any()).ToHashSet();
        return AddStep(noDepsStages, steps);
    }

    private static ParallelStagesList AddStep(ISet<Stage> stages, ParallelStagesList steps)
    {
        return steps.Add(new ParallelStages(new HashSet<Stage>(stages)));
    }

    private ParallelStagesList AddNextStep(ISet<Stage> precedingStages, ParallelStagesList steps, ISet<Stage> remaining)
    {
        var stagesDependent = StagesDependentOn(precedingStages, remaining);
        if (!stagesDependent.Any())
        {
            return steps;
        }
        steps = AddStep(stagesDependent, steps);

        steps = AddNextStep(stagesDependent, steps, remaining);
        return steps;
    }

    private HashSet<Stage> StagesDependentOn(ISet<Stage> precedingStages, ISet<Stage> remainingStages)
    {
        return remainingStages.Where(stage => stage.Dependencies
                .Any(dependency => precedingStages
                    .Any(preceding => preceding.StageName == dependency.StageName)))
            .ToHashSet();
    }
}