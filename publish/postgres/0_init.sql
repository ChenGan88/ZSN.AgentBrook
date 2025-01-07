-- 创建数据库
CREATE DATABASE xinling_knowledge_base OWNER  postgres;

-- 切换到新数据库
\c xinling_knowledge_base;

-- 添加pgvector插件
CREATE EXTENSION vector;