/*
 Navicat Premium Data Transfer

 Source Server         : MySQL
 Source Server Type    : MySQL
 Source Server Version : 50726 (5.7.26-log)
 Source Host           : 192.168.18.28:3306
 Source Schema         : xinling_model

 Target Server Type    : MySQL
 Target Server Version : 50726 (5.7.26-log)
 File Encoding         : 65001

 Date: 04/01/2025 08:18:51
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

USE xinling_model;

-- ----------------------------
-- Table structure for tb_knowledge_base_file_chunk_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_knowledge_base_file_chunk_info`;
CREATE TABLE `tb_knowledge_base_file_chunk_info` (
  `ChunkID` varchar(64) NOT NULL COMMENT '分块编号，唯一GUID',
  `KnowledgeBaseID` varchar(64) NOT NULL COMMENT '所属知识库',
  `FileID` varchar(64) NOT NULL COMMENT '所属文件',
  `FileChunkIndex` int(11) NOT NULL COMMENT '文件块索引，文件中的第几块分块',
  `Thumbnail` varchar(256) NOT NULL COMMENT '快照图片，识别快照图片存储地址',
  `TokenNumber` int(11) NOT NULL COMMENT 'Token数量',
  `Content` text NOT NULL COMMENT '分块内容',
  `CreateTime` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`ChunkID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='知识库文件分块信息';

-- ----------------------------
-- Records of tb_knowledge_base_file_chunk_info
-- ----------------------------
BEGIN;
COMMIT;

-- ----------------------------
-- Table structure for tb_knowledge_base_file_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_knowledge_base_file_info`;
CREATE TABLE `tb_knowledge_base_file_info` (
  `FileID` varchar(64) NOT NULL COMMENT '文件编号，文件MD5',
  `KnowledgeBaseID` varchar(64) DEFAULT NULL COMMENT '所属知识库编号',
  `FileName` varchar(256) NOT NULL COMMENT '文件名',
  `FilePath` varchar(512) NOT NULL COMMENT '存放路径',
  `Type` varchar(128) NOT NULL COMMENT '文件类型',
  `ParserConfig` json NOT NULL COMMENT '存储转换时的配置信息',
  `DataCount` int(11) NOT NULL DEFAULT '0' COMMENT '分块数',
  `CreateTime` datetime NOT NULL COMMENT '创建时间',
  `SystemStatus` int(11) NOT NULL COMMENT '系统状态，屏蔽=-1，分析失败=-2，等待分析=0，分析完成=1',
  PRIMARY KEY (`FileID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='知识库文件信息';

-- ----------------------------
-- Records of tb_knowledge_base_file_info
-- ----------------------------
BEGIN;
INSERT INTO `tb_knowledge_base_file_info` (`FileID`, `KnowledgeBaseID`, `FileName`, `FilePath`, `Type`, `ParserConfig`, `DataCount`, `CreateTime`, `SystemStatus`) VALUES ('36b77383f249571468eb56dc23350b2c', 'eb4d02f672407fac58f648681df7c9ce', '7.用户手册 - 知数能AgentBrook（AI Agent）快速开发框架系统.pdf', 'W:/AI/ZSN.AI/publish/files/2024/12/31/20/268ecb47ac020792c22d5f8b7433df2b.pdf', 'application/pdf', '{}', 11, '2024-12-31 20:51:01', 2);
INSERT INTO `tb_knowledge_base_file_info` (`FileID`, `KnowledgeBaseID`, `FileName`, `FilePath`, `Type`, `ParserConfig`, `DataCount`, `CreateTime`, `SystemStatus`) VALUES ('4d71689d5490fc2e2e234505c678eec1', 'eb4d02f672407fac58f648681df7c9ce', '7.用户手册 - 知数能AgentBrook（AI Agent）快速开发框架系统.docx', 'W:/AI/ZSN.AI/publish/files/2024/12/31/20/933e7463f9607b90173e8a880f614725.docx', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document', '{}', 8, '2024-12-31 21:01:04', 2);
COMMIT;

-- ----------------------------
-- Table structure for tb_knowledge_base_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_knowledge_base_info`;
CREATE TABLE `tb_knowledge_base_info` (
  `KnowledgeBaseID` varchar(64) NOT NULL COMMENT '知识库编号',
  `Name` varchar(128) NOT NULL COMMENT '名称',
  `DicIDList` varchar(512) DEFAULT NULL COMMENT '类型标签ID',
  `DicNameList` varchar(1024) DEFAULT NULL COMMENT '类型标签名称',
  `Description` varchar(512) DEFAULT NULL COMMENT '说明',
  `PreprocessModelID` int(11) DEFAULT NULL COMMENT '预处理模型',
  `PreprocessModelName` varchar(128) DEFAULT NULL COMMENT '预处理模型名称',
  `VectorModelID` int(11) DEFAULT NULL COMMENT '向量模型',
  `VectorModelName` varchar(128) DEFAULT NULL COMMENT '向量模型名称',
  `ParagraphSlice` int(11) DEFAULT NULL COMMENT '段落切片（token）',
  `LineSliceCount` int(11) DEFAULT NULL COMMENT '行切片数（token）',
  `OverlapSection` int(11) DEFAULT NULL COMMENT '重叠部分（token）',
  `SystemStatus` int(11) NOT NULL COMMENT '系统状态',
  `MemberID` varchar(64) DEFAULT NULL COMMENT '所属用户编号',
  `ChargeType` int(11) NOT NULL COMMENT '收费类型',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间',
  `LastUpdateTime` datetime DEFAULT NULL COMMENT '最后更新时间',
  PRIMARY KEY (`KnowledgeBaseID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of tb_knowledge_base_info
-- ----------------------------
BEGIN;
INSERT INTO `tb_knowledge_base_info` (`KnowledgeBaseID`, `Name`, `DicIDList`, `DicNameList`, `Description`, `PreprocessModelID`, `PreprocessModelName`, `VectorModelID`, `VectorModelName`, `ParagraphSlice`, `LineSliceCount`, `OverlapSection`, `SystemStatus`, `MemberID`, `ChargeType`, `CreateTime`, `LastUpdateTime`) VALUES ('eb4d02f672407fac58f648681df7c9ce', 'AgentBrook使用说明', '15', '行业知识', NULL, 6, '千问2.5-14B', 1, 'nomic-embed-text', 1000, 1000, 20, 1, NULL, 0, '2024-07-22 12:38:37', '2024-07-22 12:38:07');
COMMIT;

-- ----------------------------
-- Table structure for tb_large_model_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_large_model_info`;
CREATE TABLE `tb_large_model_info` (
  `LargeModelID` int(11) NOT NULL AUTO_INCREMENT COMMENT '模型编号',
  `Name` varchar(50) DEFAULT NULL COMMENT '名称',
  `ModelName` varchar(50) DEFAULT NULL COMMENT '模型名称',
  `ModelKey` varchar(128) DEFAULT NULL COMMENT '模型接口秘钥',
  `MICON` varchar(64) DEFAULT NULL COMMENT '图标',
  `TypeCode` int(11) DEFAULT NULL COMMENT '类型编号',
  `TypeName` varchar(50) DEFAULT NULL COMMENT '类型名称',
  `EndPoint` varchar(255) DEFAULT NULL COMMENT '模型地址',
  `MConfig` varchar(512) DEFAULT NULL COMMENT '配置信息Json',
  `Description` varchar(512) DEFAULT NULL COMMENT '描述',
  `SystemStatus` int(11) DEFAULT NULL COMMENT '系统状态',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间',
  `UpdateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `ModelOrganizationID` int(11) DEFAULT NULL COMMENT '模型机构类型编号',
  `ModelOrganizationName` varchar(128) DEFAULT NULL COMMENT '模型机构类型名称',
  PRIMARY KEY (`LargeModelID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of tb_large_model_info
-- ----------------------------
BEGIN;
INSERT INTO `tb_large_model_info` (`LargeModelID`, `Name`, `ModelName`, `ModelKey`, `MICON`, `TypeCode`, `TypeName`, `EndPoint`, `MConfig`, `Description`, `SystemStatus`, `CreateTime`, `UpdateTime`, `ModelOrganizationID`, `ModelOrganizationName`) VALUES (1, 'nomic-embed-text', 'nomic-embed-text', '123', 'd1f207638a894c7ccd4d67db711b5dd4', 2, '向量模型', 'http://host.docker.internal:11434', '{}', '{}', 0, '2024-07-22 12:49:15', '2024-07-22 12:48:54', 11, 'Ollama Embedding');
INSERT INTO `tb_large_model_info` (`LargeModelID`, `Name`, `ModelName`, `ModelKey`, `MICON`, `TypeCode`, `TypeName`, `EndPoint`, `MConfig`, `Description`, `SystemStatus`, `CreateTime`, `UpdateTime`, `ModelOrganizationID`, `ModelOrganizationName`) VALUES (2, 'llama3.3-70B', 'llama3.3', '123', '1', 1, '会话模型', 'http://host.docker.internal:11434/', '{}', NULL, 0, '2024-07-22 14:56:17', '2024-07-22 14:56:04', 10, 'Ollama');
INSERT INTO `tb_large_model_info` (`LargeModelID`, `Name`, `ModelName`, `ModelKey`, `MICON`, `TypeCode`, `TypeName`, `EndPoint`, `MConfig`, `Description`, `SystemStatus`, `CreateTime`, `UpdateTime`, `ModelOrganizationID`, `ModelOrganizationName`) VALUES (3, '千问QwQ-32B', 'qwq', '123', '#', 1, '会话模型', 'http://host.docker.internal:11434/', '{}', NULL, 0, '2024-12-20 12:18:13', '2024-12-20 12:17:13', 10, 'Ollama');
INSERT INTO `tb_large_model_info` (`LargeModelID`, `Name`, `ModelName`, `ModelKey`, `MICON`, `TypeCode`, `TypeName`, `EndPoint`, `MConfig`, `Description`, `SystemStatus`, `CreateTime`, `UpdateTime`, `ModelOrganizationID`, `ModelOrganizationName`) VALUES (4, 'llama3-8B', 'llama3:8b', '123', '#', 1, '会话模型', 'http://host.docker.internal:11434/', '{}', '不支持Function Calling', 0, '2024-12-20 14:08:49', '2024-12-20 14:08:13', 10, 'Ollama');
INSERT INTO `tb_large_model_info` (`LargeModelID`, `Name`, `ModelName`, `ModelKey`, `MICON`, `TypeCode`, `TypeName`, `EndPoint`, `MConfig`, `Description`, `SystemStatus`, `CreateTime`, `UpdateTime`, `ModelOrganizationID`, `ModelOrganizationName`) VALUES (5, '千问2.5-32B', 'qwen2.5:32b', '123456', '#', 1, '会话模型', 'http://host.docker.internal:11434/', '{}', NULL, 0, '2024-12-31 16:48:02', '2024-12-31 16:46:25', 10, 'Ollama');
INSERT INTO `tb_large_model_info` (`LargeModelID`, `Name`, `ModelName`, `ModelKey`, `MICON`, `TypeCode`, `TypeName`, `EndPoint`, `MConfig`, `Description`, `SystemStatus`, `CreateTime`, `UpdateTime`, `ModelOrganizationID`, `ModelOrganizationName`) VALUES (6, '千问2.5-14B', 'qwen2.5:14b', '123456', '#', 1, '会话模型', 'http://host.docker.internal:11434/', '{}', NULL, 0, '2024-12-31 16:49:05', '2024-12-31 16:48:25', 10, 'Ollama');
COMMIT;

-- ----------------------------
-- Table structure for tb_plugins_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_plugins_info`;
CREATE TABLE `tb_plugins_info` (
  `PluginsID` int(11) NOT NULL AUTO_INCREMENT COMMENT '插件编号',
  `Name` varchar(50) DEFAULT NULL COMMENT '名称',
  `Description` varchar(512) DEFAULT NULL COMMENT '描述',
  `Namespace` varchar(255) DEFAULT NULL COMMENT '命名空间',
  `ClassName` varchar(128) DEFAULT NULL COMMENT '类名',
  `EntryFunctionName` varchar(128) DEFAULT NULL COMMENT '入口函数名',
  `ReturnValueDescription` varchar(2048) DEFAULT NULL COMMENT '返回值描述',
  `SystemStatus` int(11) DEFAULT NULL COMMENT '系统状态',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间',
  `LastUpdateTime` datetime DEFAULT NULL COMMENT '最后更新时间',
  `PluginsType` int(11) DEFAULT NULL COMMENT '类型',
  `Config` text COMMENT '参数配置',
  PRIMARY KEY (`PluginsID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of tb_plugins_info
-- ----------------------------
BEGIN;
INSERT INTO `tb_plugins_info` (`PluginsID`, `Name`, `Description`, `Namespace`, `ClassName`, `EntryFunctionName`, `ReturnValueDescription`, `SystemStatus`, `CreateTime`, `LastUpdateTime`, `PluginsType`, `Config`) VALUES (3, '基础函数', 'get_date_time:获取当前日期与时间\ndate_to_chinese_traditional_calendar:将日期转换为农历', 'ZSN.AI.Plugins', 'BasePlugin', NULL, NULL, 0, '2024-08-29 16:23:07', '2024-08-29 16:22:57', 2, NULL);
COMMIT;

-- ----------------------------
-- Procedure structure for CommonPagenation
-- ----------------------------
DROP PROCEDURE IF EXISTS `CommonPagenation`;
delimiter ;;
CREATE PROCEDURE `CommonPagenation`(IN tableName VARCHAR ( 255 ),
	IN showFName VARCHAR ( 5000 ),
	IN selectWhere VARCHAR ( 5000 ),
	IN selectOrder VARCHAR ( 200 ),
	IN pageNo INT,
	IN pageSize INT)
BEGIN
	
	SET @startrow = ( pageNo - 1 ) * pageSize;
	
	SET @pagesize = pageSize;
	
	SET @rowindex = 0;
	
	SET @strsql = CONCAT(
		'select SQL_CALC_FOUND_ROWS ',
		showFName,
		' from ',
		tableName,
		CASE
				IFNULL( selectWhere, '' ) 
				WHEN '' THEN
				'' ELSE CONCAT ( ' where ', selectWhere ) 
			END,
		CASE
				IFNULL( selectOrder, '' ) 
				WHEN '' THEN
				'' ELSE CONCAT( ' order by ', selectOrder ) 
			END,
			' limit ',
			@startRow,
			',',
			@pagesize 
		);
	
	SET @countsql = "SELECT FOUND_ROWS()";
	PREPARE strsql 
	FROM
		@strsql;
	EXECUTE strsql;
	PREPARE countsql 
	FROM
		@countsql;
	EXECUTE countsql;

END
;;
delimiter ;

SET FOREIGN_KEY_CHECKS = 1;
