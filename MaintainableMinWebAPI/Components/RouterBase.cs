namespace MaintainableMinWebAPI.Components;

public class RouterBase
{
    public string urlFragment;
    protected ILogger logger;

    public virtual void AddRoutes(WebApplication app)
    {
        
    }
}