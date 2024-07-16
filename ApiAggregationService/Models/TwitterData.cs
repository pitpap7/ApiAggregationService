namespace ApiAggregationService.Models;

public class TwitterData
{
    public List<TwitterUser> Data { get; set; }
    public TwitterMeta Meta { get; set; }
}

public class TwitterUser
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
}

public class TwitterMeta
{
    public int ResultCount { get; set; }
    public string NextToken { get; set; }
}