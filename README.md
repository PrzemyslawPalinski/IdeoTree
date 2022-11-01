# IdeoTree
To launch this app you first need to create SQL Server Database with name "TreeApp" on your local mashine, next step is applying migrations using command "dotnet ef database update" while having IdeoTreeAPI folder opened in console. Then you need to install Vue CLI.
<br /><br />
After that you should be able to launch both asp.net app using "dotnet run" command in IdeoTreeAPI folder and "npm run serve" in IdeoTreeClient folder.
<br /><br />
Tree should populate on its own when client app starts and tries to load tree for first time. If page shows loading after you launch both apps you need to refresh it.
<br /><br />
When running correctly app should look like this:
![image](https://user-images.githubusercontent.com/23617264/199296528-12fb4cd8-ab74-40ad-88e9-3c4210ee6020.png)

