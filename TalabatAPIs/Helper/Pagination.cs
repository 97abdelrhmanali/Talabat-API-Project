using TalabatAPIs.DTOs;

namespace TalabatAPIs.Helper
{
    public class Pagination<T>
    {
        public int PageIndex { get; set; }
        public int Pagesize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }
        public Pagination(int pageIndex, int pageSize ,int count , IReadOnlyList<T> data)
        {
            PageIndex = pageIndex;
            Pagesize = pageSize;
            Count = count;
            Data = data;
        }
    }
}
