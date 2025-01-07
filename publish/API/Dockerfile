# 使用 .NET 8 运行时镜像
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
EXPOSE 5003

# 安装 libgdiplus
RUN apt-get update && \
    apt-get install -y libgdiplus && \
    rm -rf /var/lib/apt/lists/*

# 将发布的文件复制到容器中
COPY ./publish/ ./

# 设置入口点
ENTRYPOINT ["dotnet", "ZSN.AgentBrook.API.dll"]
