namespace MessengerApp.Application.RequestResponseModels.RequestModels;

public class QueryParameter
{
    public int Size { get; set; } = 20;
    public int From { get; set; }
    public int Index { get; set; }
}

public class QueryParameter<T> : QueryParameter
    where T : class
{
    public T ModelDto { get; set; }
}