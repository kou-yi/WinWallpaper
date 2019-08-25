# Windows 动态壁纸

---

参考了[Yinmany/WinWallpaper](https://github.com/Yinmany/WinWallpaper)  
使用了cefSharp，可以浏览网页  
加入了鼠标的Hook，判断鼠标是否在桌面再给cef传递事件  
网页地址在项目的/WallPaperTest/MainWindow.xaml中可修改  
可使用Live2d Web SDK来渲染Live2d模型

> 注：编译时请先恢复Nuget包然后选择32或64位平台（因为cef不支持Any CPU）  

---

已知bug：
1. 窗口拖动也会识别为鼠标在桌面上而发生事件  
2. 右键菜单会同时弹出windows的和cef的  
3. 暂时只在win10测试，在不同版本Windows上可能不能识别鼠标是否在桌面上，如果遇到这样的问题希望提一下issue


因为平时写Java所以不太会C#，代码可能比较乱请见谅，不习惯写注释XD，比较简单应该看得懂