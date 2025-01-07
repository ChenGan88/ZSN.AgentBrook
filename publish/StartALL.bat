@echo off
cd /d %~dp0

rem 运行 AgentBrook

rem 进入 Web.Manage 文件夹并运行程序
cd Web.Manage
start "" ZSN.AgentBrook.Web.Manage.exe
cd ..

rem 进入 LLMServer 文件夹并运行程序
cd LLMServer
start "" ZSN.AI.LLMServer.exe
cd ..

rem 进入 AutoJob 文件夹并运行程序
cd AutoJob
start "" ZSN.AgentBrook.AutoJob.exe
cd ..

rem 进入 API 文件夹并运行程序
cd API
start "" ZSN.AgentBrook.API.exe
cd ..