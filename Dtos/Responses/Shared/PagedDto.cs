using WebApiNHibernateCrudPagination.Models;

namespace WebApiNHibernateCrudPagination.Dtos.Responses.Shared
{
    public abstract class PagedDto : SuccessResponse
    {
        public PageMeta PageMeta { get; set; }
    }
}