## 練習使用 .NET Core 製作上傳與下載檔案

至 2021/11/24 實作內容有

<ol>
   <li>上傳與下載檔案於 wwwroot 的資料夾</li>
   <li>上傳與下載檔案於 wwwroot 以外的資料夾(DifferentFolder)</li>
   <li>在 DifferentFolder 的基礎上實作安全性
      <ol style="list-style-type: lower-alpha;">
         <li>只允許應用程式設計規格的核准副檔名。</li>
         <li>在伺服器上執行用戶端檢查。</li>
         <li>以套件比對檔案的位元組與檔案類型</li>
         <li>伺服器和應用程式設定，限制檔案上傳大小，資料夾不可執行(non-executable)</li>
      </ol>
   </li>
   <li>多筆檔案上傳</li>
</ol>
<hr/>

2021/12/26

<p>由於 Ajax 檔案上傳是針對 API 設計，目前不予實作</p>
<p>實作多筆檔案上傳的安全性</p>
<p>目前不打算繼續研究資料庫的部分，因為已經會資料庫，打算將時間優先放在學習 Spring Boot MVC 與 Laravel 上面</p>
<hr/>

2022/01/01
<p>完成重新命名並下載檔案的功能</p>