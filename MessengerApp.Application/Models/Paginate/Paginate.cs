using MessengerApp.Application.RequestResponseModels.RequestModels;

namespace MessengerApp.Application.RequestResponseModels;

public class Paginate<T> : IPaginate<T>
{
    public int From { get; set; }
    public int Index { get; set; }
    public int Size { get; set; }
    public int Count { get; set; }
    public int Pages { get; set; }
    public IList<T> Items { get; set; }

    public bool HasPrevious => Index - From > 0;
    public bool HasNext => Index - From + 1 < Pages;


    public Paginate(IQueryable<T> source, QueryParameter queryParameter)
    {
        if (queryParameter.From > queryParameter.Index)
            throw new ArgumentException($"From: {queryParameter.From} > Index: {queryParameter.Index}, must From <= Index");
        
        Index = queryParameter.Index;
        Size = queryParameter.Size;
        From = queryParameter.From;
        Count = source.Count();
        Pages = (int) Math.Ceiling(Count / (double) Size);
        Items = source.Skip((Index - From) * Size).Take(Size).ToList();
    }
}