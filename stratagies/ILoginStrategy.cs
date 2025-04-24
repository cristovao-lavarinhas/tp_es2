using esii.Models;

namespace esii.stratagies
{
    public interface ILoginStrategy
    {
        Task<bool> LoginAsync(HttpContext httpContext, loginviewmodel viewModel);
    }
}