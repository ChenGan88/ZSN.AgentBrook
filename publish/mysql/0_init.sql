-- 创建多个数据库
CREATE DATABASE IF NOT EXISTS xinling_base;
CREATE DATABASE IF NOT EXISTS xinling_member;
CREATE DATABASE IF NOT EXISTS xinling_log;
CREATE DATABASE IF NOT EXISTS xinling_model;
CREATE DATABASE IF NOT EXISTS xinling_app;
CREATE DATABASE IF NOT EXISTS xinling_agent;
CREATE DATABASE IF NOT EXISTS xinling_workflow;
CREATE DATABASE IF NOT EXISTS xinling_chat;
CREATE DATABASE IF NOT EXISTS xinling_job;
CREATE DATABASE IF NOT EXISTS xinling_object;

-- 创建用户并设置密码
CREATE USER 'xinling_base'@'%' IDENTIFIED BY 'wFzMsiDrTApCKT7N';
CREATE USER 'xinling_member'@'%' IDENTIFIED BY 'H8YfpmHPLkDwJ5Kn';
CREATE USER 'xinling_log'@'%' IDENTIFIED BY 'HGN4k7wYYPx5tiGB';
CREATE USER 'xinling_model'@'%' IDENTIFIED BY 'ZsW3JHjpAaBByWAN';
CREATE USER 'xinling_app'@'%' IDENTIFIED BY 'aPC6swiiaJca2dEj';
CREATE USER 'xinling_agent'@'%' IDENTIFIED BY '4pJYrAZcNR22trXr';
CREATE USER 'xinling_workflow'@'%' IDENTIFIED BY 'CEJtB7DKsSp6WwbB';
CREATE USER 'xinling_chat'@'%' IDENTIFIED BY '6ZBaaC6yCM36sShM';
CREATE USER 'xinling_job'@'%' IDENTIFIED BY '7wENkx4Hawhyk6sL';
CREATE USER 'xinling_object'@'%' IDENTIFIED BY 'nNWeeGj8Z2B6nJdM';

-- 为用户授予权限
GRANT ALL PRIVILEGES ON xinling_base.* TO 'xinling_base'@'%';
GRANT ALL PRIVILEGES ON xinling_member.* TO 'xinling_member'@'%';
GRANT ALL PRIVILEGES ON xinling_log.* TO 'xinling_log'@'%';
GRANT ALL PRIVILEGES ON xinling_model.* TO 'xinling_model'@'%';
GRANT ALL PRIVILEGES ON xinling_app.* TO 'xinling_app'@'%';
GRANT ALL PRIVILEGES ON xinling_agent.* TO 'xinling_agent'@'%';
GRANT ALL PRIVILEGES ON xinling_workflow.* TO 'xinling_workflow'@'%';
GRANT ALL PRIVILEGES ON xinling_chat.* TO 'xinling_chat'@'%';
GRANT ALL PRIVILEGES ON xinling_job.* TO 'xinling_job'@'%';
GRANT ALL PRIVILEGES ON xinling_object.* TO 'xinling_object'@'%';

-- 刷新权限，使修改生效
FLUSH PRIVILEGES;