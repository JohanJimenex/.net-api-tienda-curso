
namespace API.Helpers;

public class QueryParams {

    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 10;

    public QueryParams() { }

    public QueryParams(int pageIndex, int pageSize) {
        PageIndex = pageIndex;
        PageSize = pageSize;
    }

}
