namespace DomainDrivers.SmartSchedule.Planning.Parallelization;

public class StageParallelization
{
    public ParallelStagesList Of(ISet<Stage> stages)
    {
        return ParallelStagesList.Empty()
            .Add(new ParallelStages(new HashSet<Stage>(stages)));
    }
}