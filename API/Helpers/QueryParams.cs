
namespace API.Helpers;

public class QueryParams {

    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }

    public QueryParams() { }

    public QueryParams(int pageIndex, int pageSize, string search) {
        PageIndex = pageIndex;
        PageSize = pageSize;
        this.Search = search;
    }

}
