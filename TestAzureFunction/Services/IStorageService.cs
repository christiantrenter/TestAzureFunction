using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestAzureFunction.Services
{
    public interface IStorageService
    {
        Task<ObjectResult> UploadFile(IFormFileCollection fileCollection);
    }
}