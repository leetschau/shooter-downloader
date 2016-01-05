自己寫的小工具，使用射手網開放的網頁API尋找並下載指定目錄中的影片檔的字幕。字幕下載的方法參考了射手影音播放器的原始碼，然後以C#重新寫過。整個程式基於.Net Framework 2.0開發。執行本程式前須先安裝.Net Framework 2.0執行環境。

射手網字幕下載工具的安裝程式以及程式碼可以在專案網頁下載。程式原始碼以GPLv3的方式開放。

射手網字幕下載工具的程式主畫面如下:

![http://shooter-downloader.googlecode.com/svn/wiki/ShooterDownloaderOverview.attach/main_form.png](http://shooter-downloader.googlecode.com/svn/wiki/ShooterDownloaderOverview.attach/main_form.png)

使用方法很簡單，只有三個步驟
1. 選擇影片所在目錄。
2. 勾選欲下載字幕的影片。
3. 按下"開始下載"按鈕。

然後靜待下載完成，下載的字幕會跟影片放在一起。

使用者可以自行設定一些程式選項，按下"設定"按鈕便可以打開設定畫面。

![http://shooter-downloader.googlecode.com/svn/wiki/ShooterDownloaderOverview.attach/settings_form.png](http://shooter-downloader.googlecode.com/svn/wiki/ShooterDownloaderOverview.attach/settings_form.png)

以下是各選項的說明:

最大同時下載數: 設定最多可同時下載幾個字幕，預設值為1，最多可同時下載3個字幕。

電影檔副檔名: 設定電影檔的副檔名有哪些，程式會自動勾選有這些副檔名的檔案。

啟動日誌功能: 設定是否輸出日誌 (log)，預設是關閉。

自動將簡體字幕轉成繁體: 如果抓到的是簡體字幕就自動轉成繁體，預設開啟。

啟用/停用右鍵選單功能<font color='#FF0000'>(1.1.0.41版新增)</font>: 啟用或停用在檔案總管用右鍵選單下載字幕的功能。需管理者權限才能更改此設定。

自動將簡體字幕轉成繁體有時會有誤判而導致轉換失敗的情況，另外簡體字幕僅限GB編碼的字幕，UTF8編碼的無法轉換。

在此特別感謝射手網提供的射手影音播放器及字幕下載API，沒有這些優秀的軟體的話本工具便無法實現。