# 使用 .NET 8 运行时镜像
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
EXPOSE 5006

# 将发布的文件复制到容器中
COPY ./publish/ ./

# 设置入口点
ENTRYPOINT ["dotnet", "ZSN.AI.LLMServer.dll"]
