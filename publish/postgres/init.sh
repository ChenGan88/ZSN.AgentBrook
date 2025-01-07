#!/bin/bash
# init.sh

# 使 PostgreSQL 在启动后重启服务
echo "Loading pgvector extension..."
# 通过执行 SQL 脚本来创建扩展
psql -U postgres -c "CREATE EXTENSION IF NOT EXISTS vector;"

# 确保 PostgreSQL 启动时加载新的扩展
pg_ctl reload
