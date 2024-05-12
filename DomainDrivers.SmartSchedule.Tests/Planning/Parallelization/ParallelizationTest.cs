using DomainDrivers.SmartSchedule.Planning.Parallelization;

namespace DomainDrivers.SmartSchedule.Tests.Planning.Parallelization;

public class ParallelizationTest
{
    private static readonly StageParallelization StageParallelization = new StageParallelization();

    [Fact]
    public void EverythingCanBeDoneInParallelWhenThereAreNoDependencies()
    {
        var stage1 = new Stage("Stage1");
        var stage2 = new Stage("Stage2");

        var sortedStages = StageParallelization.Of(new HashSet<Stage>() { stage1, stage2 });

        Assert.Equal(1, sortedStages.All.Count);
        Assert.Equal("Stage1, Stage2", sortedStages.Print());
    }

    [Fact]
    public void TestSimpleDependencies()
    {
        var stage1 = new Stage("Stage1");
        var stage2 = new Stage("Stage2");
        var stage3 = new Stage("Stage3");
        var stage4 = new Stage("Stage4");
        stage2.DependsOn(stage1);
        stage3.DependsOn(stage1);
        stage4.DependsOn(stage2);

        var sortedStages = StageParallelization.Of(new HashSet<Stage> { stage1, stage2, stage3, stage4 });

        Assert.Equal("Stage1 | Stage2, Stage3 | Stage4", sortedStages.Print());
    }

    [Fact]
    public void CantAddCycleDependency()
    {
        var stage1 = new Stage("Stage1");
        var stage2 = new Stage("Stage2");
        stage2.DependsOn(stage1);


        Assert.ThrowsAny<Exception>(() => stage1.DependsOn(stage2));
    }
}