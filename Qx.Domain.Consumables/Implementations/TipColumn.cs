namespace Qx.Domain.Consumables.Implementations;

public sealed class TipColumn(IReadOnlyList<Tip> tips)
{
    IReadOnlyList<Tip> Tips => tips;
    
    // - don't keep track if a tip column is used or new since the tip itself tracks that
    // - don't need to track which tips are where since the tips themselves know what kind of tips they are
    // - need a way to ensure an orchestrator can determine which tips can be used on the fly for a run when
    //   the run is scheduled and for scheduling future runs while a run is in progress or starting a run
    //   during a run that is in progress
}