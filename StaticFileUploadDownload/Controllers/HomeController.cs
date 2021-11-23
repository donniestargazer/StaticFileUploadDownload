using FileSignatures;//對照檔案標頭
using FileSignatures.Formats;//對照檔案類型(內含一些檔案類型)
using Microsoft.AspNetCore.Hosting;//IWebHostEnvironment
using Microsoft.AspNetCore.Http;//IFormFile
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StaticFileUploadDownload.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;//Directory ,FileMode ,FileStream ,Path
using System.Threading.Tasks;
using System.Web;


namespace StaticFileUploadDownload.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private IWebHostEnvironment _environment;// 為了 wwwroot 資料夾

        private readonly long _fileSizeLimit;// 限制檔案大小

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment Environment, IConfiguration config)
        {
            _logger = logger;
            _environment = Environment;// 為了 wwwroot 資料夾
            _fileSizeLimit = config.GetValue<long>("FileSizeLimit");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult wwwrootFolder()
        {
            List<VMFiles> fileList =
                new List<VMFiles>() {
                    new VMFiles() { Name = "ACEU 最佳移動教學.docx" , Path = "/wwwDownload/115d43f2-18b9-4828-958a-f997857fa8e9.docx" , Type = ".docx" , UploadDate = DateTime.Parse("2021/10/23 下午 07:28:45") } ,
                    new VMFiles() { Name = "What is cyberpunk(什麼是電馭叛客).docx" , Path = "/wwwDownload/4a74dd35-6cdd-4a41-9f1a-1680d2457f53.docx" , Type = ".docx" , UploadDate = DateTime.Parse("2021/10/23 下午 07:45:18") } ,
                    new VMFiles() { Name = "如何將粉絲群一分為二 (重寫最後生還者2).docx" , Path = "/wwwDownload/6e6cbe76-11e0-4c30-9f5e-46342721ad99.docx" , Type = ".docx" , UploadDate = DateTime.Parse("2021/10/23 下午 07:50:56") }
                };
            return View(fileList);
        }

        public string GetFileType(IFormFile file)
        {
            //回傳檔案類型的函數，會去比對跟輸出網際網路媒體型式，雖然並不比 List<string> 比對更好
            //並未用到，僅作參考資料
            string contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(file.FileName, out contentType);
            return contentType ?? "application/octet-stream";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> wwwrootFolder(IFormFile file)
        {
            string path;
            //_environment.WebRootPath 取得 wwwroot 的實體路徑 D:\電腦備份\電腦備份文件\程式設計\Visual Studio IDE\C#\StaticFileUploadDownload\StaticFileUploadDownload\wwwroot
            string wwwPath = _environment.WebRootPath;
            string contentPath = _environment.ContentRootPath;//一樣能取得 wwwroot 上一層的實體路徑，但這裡不使用

            try
            {
                if (file.Length > 0)//取得檔案的長度（以位元組為單位）
                {
                    //Guid.NewGuid() 產生亂碼檔名
                    //Path.GetExtension(file.FileName) 取得副檔名
                    string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);

                    //Path.Combine() 將實體路徑加上檔案資料夾
                    //Path.GetFullPath() 取得絕對路徑
                    path = Path.GetFullPath(Path.Combine(wwwPath, "wwwUpload"));

                    //FileMode.Create 指定作業系統應該建立新檔案。 若此檔案已經存在，其將會覆寫該檔案。
                    //Path.Combine 結合絕對路徑與檔案名稱
                    //FileStream 於該路徑建立檔案 (使用指定的路徑和建立模式初始化 FileStream 類別的新執行個體。)
                    using (var filestream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                    {
                        //CopyToAsync() 以非同步的方式從目前資料流讀取所有位元組，並將其寫入另一個資料流中
                        await file.CopyToAsync(filestream);
                    }
                    TempData["msg"] = "File Uploaded successfully.";
                        //"File Uploaded successfully.";
                }
            }
            catch (Exception)
            {
                throw;
            }

            List<VMFiles> fileList =
                new List<VMFiles>() {
                    new VMFiles() { Name = "ACEU 最佳移動教學.docx" , Path = "/wwwDownload/115d43f2-18b9-4828-958a-f997857fa8e9.docx" , Type = ".docx" , UploadDate = DateTime.Parse("2021/10/23 下午 07:28:45") } ,
                    new VMFiles() { Name = "What is cyberpunk(什麼是電馭叛客).docx" , Path = "/wwwDownload/4a74dd35-6cdd-4a41-9f1a-1680d2457f53.docx" , Type = ".docx" , UploadDate = DateTime.Parse("2021/10/23 下午 07:45:18") } ,
                    new VMFiles() { Name = "如何將粉絲群一分為二 (重寫最後生還者2).docx" , Path = "/wwwDownload/6e6cbe76-11e0-4c30-9f5e-46342721ad99.docx" , Type = ".docx" , UploadDate = DateTime.Parse("2021/10/23 下午 07:50:56") }
                };
            return View(fileList);
        }

        public IActionResult DifferentFolder()
        {
            List<VMFiles> fileList =
                new List<VMFiles>() {
                    new VMFiles() { Name = "ACEU 最佳移動教學.docx" , Path = "/app-download/115d43f2-18b9-4828-958a-f997857fa8e9.docx" , Type = ".docx" , UploadDate = DateTime.Parse("2021/10/23 下午 07:28:45") } ,
                    new VMFiles() { Name = "What is cyberpunk(什麼是電馭叛客).docx" , Path = "/app-download/4a74dd35-6cdd-4a41-9f1a-1680d2457f53.docx" , Type = ".docx" , UploadDate = DateTime.Parse("2021/10/23 下午 07:45:18") } ,
                    new VMFiles() { Name = "如何將粉絲群一分為二 (重寫最後生還者2).docx" , Path = "/app-download/6e6cbe76-11e0-4c30-9f5e-46342721ad99.docx" , Type = ".docx" , UploadDate = DateTime.Parse("2021/10/23 下午 07:50:56") }
                };
            return View(fileList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DifferentFolder(IFormFile file)
        {
            string path;
            try
            {
                if (file.Length > 0)//取得檔案的長度（以位元組為單位）
                {
                    //Guid.NewGuid() 產生亂碼檔名
                    //Path.GetExtension(file.FileName) 取得副檔名
                    string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    

                    //Directory.GetCurrentDirectory() 取得實體路徑 D:\電腦備份\電腦備份文件\程式設計\Visual Studio IDE\C#\StaticFileUploadDownload\StaticFileUploadDownload
                    //Path.Combine() 將實體路徑加上檔案資料夾
                    //Path.GetFullPath() 取得絕對路徑
                    path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Upload"));

                    //FileMode.Create 指定作業系統應該建立新檔案。 若此檔案已經存在，其將會覆寫該檔案。
                    //Path.Combine 結合絕對路徑與檔案名稱
                    //FileStream 於該路徑建立檔案 (使用指定的路徑和建立模式初始化 FileStream 類別的新執行個體。)
                    using (var filestream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                    {
                        //CopyToAsync() 以非同步的方式從目前資料流讀取所有位元組，並將其寫入另一個資料流中
                        await file.CopyToAsync(filestream);
                    }
                    TempData["msg"] = "File Uploaded successfully.";
                }
            }
            catch (Exception)
            {
                throw;
            }

            List<VMFiles> fileList =
                new List<VMFiles>() {
                    new VMFiles() { Name = "ACEU 最佳移動教學.docx" , Path = "/app-download/115d43f2-18b9-4828-958a-f997857fa8e9.docx" , Type = ".docx" , UploadDate = DateTime.Parse("2021/10/23 下午 07:28:45") } ,
                    new VMFiles() { Name = "What is cyberpunk(什麼是電馭叛客).docx" , Path = "/app-download/4a74dd35-6cdd-4a41-9f1a-1680d2457f53.docx" , Type = ".docx" , UploadDate = DateTime.Parse("2021/10/23 下午 07:45:18") } ,
                    new VMFiles() { Name = "如何將粉絲群一分為二 (重寫最後生還者2).docx" , Path = "/app-download/6e6cbe76-11e0-4c30-9f5e-46342721ad99.docx" , Type = ".docx" , UploadDate = DateTime.Parse("2021/10/23 下午 07:50:56") }
                };
            return View(fileList);
        }

        public IActionResult Sucurity()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sucurity(IFormFile file)
        {
            //Path.GetExtension(file.FileName) 取得副檔名
            string path, DataType = Path.GetExtension(file.FileName);
            //副檔名的白名單列表
            List<string> DataTypeWhiteList = new List<string>() { ".docx", ".pptx", ".xlsx", ".doc", ".ppt", ".xls" };

            var inspector = new FileFormatInspector();
            var mediaType = inspector.DetermineFileFormat(file.OpenReadStream());
            var extension = inspector.DetermineFileFormat(file.OpenReadStream()).Extension;
            Debug.WriteLine("副檔名：" + extension + "，網際網路媒體型式：" + mediaType);

            //if (mediaType is Pdf)
            //{
            //    Debug.WriteLine("Is PDF");
            //}
            //else if (mediaType is OfficeOpenXml)
            //{
            //    Debug.WriteLine("Is OfficeOpenXml");
            //}
            //else if (mediaType is Image)
            //{
            //    Debug.WriteLine("Is Image");
            //}
            //else if (mediaType is RichTextFormat)
            //{
            //    Debug.WriteLine("Is RichTextFormat");
            //}

            try
            {
                //比對副檔名(這是沒有辦法載入其他套件的最佳作法)
                //if (file.Length > 0 && DataTypeWhiteList.Contains(DataType))
                //取得檔案的長度（以位元組為單位）
                //大於 0 且小於指定長度(數值寫於 appsettings)
                //比對網際網路媒體型式是否符合 openxmlformats(目前有 .docx、.pptx、xlsx)
                if (file.Length > 0 && file.Length < _fileSizeLimit && mediaType is OfficeOpenXml)
                {
                    //Guid.NewGuid() 產生亂碼檔名
                    string filename = Guid.NewGuid() + "." + extension;

                    //Directory.GetCurrentDirectory() 取得實體路徑 D:\電腦備份\電腦備份文件\程式設計\Visual Studio IDE\C#\StaticFileUploadDownload\StaticFileUploadDownload
                    //Path.Combine() 將實體路徑加上檔案資料夾
                    //Path.GetFullPath() 取得絕對路徑
                    path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Upload"));

                    //FileMode.Create 指定作業系統應該建立新檔案。 若此檔案已經存在，其將會覆寫該檔案。
                    //Path.Combine 結合絕對路徑與檔案名稱
                    //FileStream 於該路徑建立檔案 (使用指定的路徑和建立模式初始化 FileStream 類別的新執行個體。)
                    using (var filestream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                    {
                        //CopyToAsync() 以非同步的方式從目前資料流讀取所有位元組，並將其寫入另一個資料流中
                        await file.CopyToAsync(filestream);
                    }

                    TempData["msg"] = "File Uploaded successfully.";
                }
            }
            catch (Exception)
            {
                throw;
            }

            return View();
        }

        public IActionResult MultipleFiles()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MultipleFiles(List<IFormFile> fileList)
        {
            string path;
            try
            {
                foreach (var file in fileList)
                {
                    var inspector = new FileFormatInspector();
                    var mediaType = inspector.DetermineFileFormat(file.OpenReadStream());
                    var extension = inspector.DetermineFileFormat(file.OpenReadStream()).Extension;
                    Debug.WriteLine("副檔名：" + extension + "，網際網路媒體型式：" + mediaType);
                    if (file.Length > 0)//取得檔案的長度（以位元組為單位）
                    {
                        //Guid.NewGuid() 產生亂碼檔名
                        //Path.GetExtension(file.FileName) 取得副檔名
                        string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);


                        //Directory.GetCurrentDirectory() 取得實體路徑 D:\電腦備份\電腦備份文件\程式設計\Visual Studio IDE\C#\StaticFileUploadDownload\StaticFileUploadDownload
                        //Path.Combine() 將實體路徑加上檔案資料夾
                        //Path.GetFullPath() 取得絕對路徑
                        path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Upload"));

                        //FileMode.Create 指定作業系統應該建立新檔案。 若此檔案已經存在，其將會覆寫該檔案。
                        //Path.Combine 結合絕對路徑與檔案名稱
                        //FileStream 於該路徑建立檔案 (使用指定的路徑和建立模式初始化 FileStream 類別的新執行個體。)
                        using (var filestream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                        {
                            //CopyToAsync() 以非同步的方式從目前資料流讀取所有位元組，並將其寫入另一個資料流中
                            await file.CopyToAsync(filestream);
                        }
                    }
                }
                        TempData["msg"] = "File Uploaded successfully.";
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }

        public IActionResult AjaxUpload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AjaxUpload(IList<IFormFile> files)
        {
            foreach (var file in files)
            {
                var inspector = new FileFormatInspector();
                var mediaType = inspector.DetermineFileFormat(file.OpenReadStream());
                var extension = inspector.DetermineFileFormat(file.OpenReadStream()).Extension;
                Debug.WriteLine("副檔名：" + extension + "，網際網路媒體型式：" + mediaType);
            }
            return this.View();
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MultipleFilesWithSecurity(List<IFormFile> fileList)
        {
            string path;
            try
            {
                foreach (var file in fileList)
                {
                    var inspector = new FileFormatInspector();
                    var mediaType = inspector.DetermineFileFormat(file.OpenReadStream());
                    var extension = inspector.DetermineFileFormat(file.OpenReadStream()).Extension;
                    Debug.WriteLine("副檔名：" + extension + "，網際網路媒體型式：" + mediaType);
                    if (file.Length > 0)//取得檔案的長度（以位元組為單位）
                    {
                        //Guid.NewGuid() 產生亂碼檔名
                        //Path.GetExtension(file.FileName) 取得副檔名
                        string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);


                        //Directory.GetCurrentDirectory() 取得實體路徑 D:\電腦備份\電腦備份文件\程式設計\Visual Studio IDE\C#\StaticFileUploadDownload\StaticFileUploadDownload
                        //Path.Combine() 將實體路徑加上檔案資料夾
                        //Path.GetFullPath() 取得絕對路徑
                        path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Upload"));

                        //FileMode.Create 指定作業系統應該建立新檔案。 若此檔案已經存在，其將會覆寫該檔案。
                        //Path.Combine 結合絕對路徑與檔案名稱
                        //FileStream 於該路徑建立檔案 (使用指定的路徑和建立模式初始化 FileStream 類別的新執行個體。)
                        using (var filestream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                        {
                            //CopyToAsync() 以非同步的方式從目前資料流讀取所有位元組，並將其寫入另一個資料流中
                            await file.CopyToAsync(filestream);
                        }
                    }
                }
                TempData["msg"] = "File Uploaded successfully.";
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }

        public IActionResult DownloadRename()
        {
            List<VMFiles> fileList =
                new List<VMFiles>() {
                    new VMFiles() { Name = "ACEU 最佳移動教學.docx" , Path = "/app-download/115d43f2-18b9-4828-958a-f997857fa8e9.docx" , Type = ".docx" , UploadDate = DateTime.Parse("2021/10/23 下午 07:28:45") } ,
                    new VMFiles() { Name = "What is cyberpunk(什麼是電馭叛客).docx" , Path = "/app-download/4a74dd35-6cdd-4a41-9f1a-1680d2457f53.docx" , Type = ".docx" , UploadDate = DateTime.Parse("2021/10/23 下午 07:45:18") } ,
                    new VMFiles() { Name = "如何將粉絲群一分為二 (重寫最後生還者2).docx" , Path = "/app-download/6e6cbe76-11e0-4c30-9f5e-46342721ad99.docx" , Type = ".docx" , UploadDate = DateTime.Parse("2021/10/23 下午 07:50:56") }
                };
            return View(fileList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DownloadRename(IFormFile file)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
