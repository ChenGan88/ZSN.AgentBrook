# 使用官方 PostgreSQL 镜像作为基础镜像
FROM postgres:16.6

# 设置环境变量以初始化数据库
ENV PGVECTOR_VERSION=0.8.0

# 设置PostgreSQL的管理员密码
ENV POSTGRES_PASSWORD=1q2w3e


# 更新包列表并安装构建依赖项
RUN apt-get update && \
    apt-get install -y \
    postgresql-16-pgvector \
    && rm -rf /var/lib/apt/lists/*

# 创建自定义初始化脚本，用于安装扩展并重启服务
COPY ./init.sh /docker-entrypoint-initdb.d/

# 在容器启动时，运行自定义初始化脚本（如果有）
COPY ./0_init.sql /docker-entrypoint-initdb.d/

# 暴露 PostgreSQL 默认端口
EXPOSE 5432
