@echo off
cd /d %~dp0

rem 运行 AgentBrook

rem 进入 Web.Manage 文件夹并运行程序
cd "Web.Manage/publish"
start "" ZSN.AgentBrook.Web.Manage.exe
cd /d %~dp0

rem 进入 LLMServer 文件夹并运行程序
cd "LLMServer/publish"
start "" ZSN.AI.LLMServer.exe
cd /d %~dp0

rem 进入 AutoJob 文件夹并运行程序
cd "AutoJob/publish"
start "" ZSN.AgentBrook.AutoJob.exe
cd /d %~dp0

rem 进入 API 文件夹并运行程序
cd "API/publish"
start "" ZSN.AgentBrook.API.exe
cd /d %~dp0