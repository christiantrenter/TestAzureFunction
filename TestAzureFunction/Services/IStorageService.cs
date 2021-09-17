using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TestAzureFunction.Services
{
    public interface IStorageService
    {
        Task ConfigureStorageAndUploadFiles(IFormFileCollection fileCollection);
    }
}