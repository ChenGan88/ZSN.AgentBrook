/*
 Navicat Premium Data Transfer

 Source Server         : MySQL
 Source Server Type    : MySQL
 Source Server Version : 50726 (5.7.26-log)
 Source Host           : 192.168.18.28:3306
 Source Schema         : xinling_agent

 Target Server Type    : MySQL
 Target Server Version : 50726 (5.7.26-log)
 File Encoding         : 65001

 Date: 04/01/2025 08:16:06
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

USE xinling_agent;

-- ----------------------------
-- Table structure for tb_agent_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_agent_info`;
CREATE TABLE `tb_agent_info` (
  `AgentID` varchar(64) NOT NULL COMMENT '系统编号，唯一',
  `Name` varchar(50) DEFAULT NULL COMMENT '名称',
  `AICON` varchar(64) DEFAULT NULL COMMENT '图标',
  `MemberID` varchar(64) DEFAULT NULL COMMENT '所属会员编号',
  `MemberName` varchar(128) DEFAULT NULL COMMENT '所属会员名称',
  `DicIDList` varchar(512) DEFAULT NULL COMMENT '类型标签ID，多选，逗号间隔',
  `DicNameList` varchar(1024) DEFAULT NULL COMMENT '类型标签名称，多选，逗号间隔',
  `Description` varchar(512) DEFAULT NULL COMMENT '说明',
  `SessionModelID` int(11) DEFAULT NULL COMMENT '会话模型ID',
  `SessionModelName` varchar(128) DEFAULT NULL COMMENT '会话模型名称',
  `Prompt` text COMMENT '提示词',
  `TemperatureCoefficient` decimal(10,2) DEFAULT NULL COMMENT '温度系数',
  `TopPCoefficient` decimal(10,2) DEFAULT NULL COMMENT '采用系数',
  `SystemStatus` int(11) DEFAULT NULL COMMENT '系统状态，屏蔽=-1，未审核=0，正常=1',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间',
  `LastUpdateTime` datetime DEFAULT NULL COMMENT '最后更新时间',
  PRIMARY KEY (`AgentID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tb_agent_info
-- ----------------------------
BEGIN;
INSERT INTO `tb_agent_info` (`AgentID`, `Name`, `AICON`, `MemberID`, `MemberName`, `DicIDList`, `DicNameList`, `Description`, `SessionModelID`, `SessionModelName`, `Prompt`, `TemperatureCoefficient`, `TopPCoefficient`, `SystemStatus`, `CreateTime`, `LastUpdateTime`) VALUES ('020db83fc1d39990448074b97e938b52', 'AgentBrook系统使用说明', '21', NULL, NULL, '22', '客服', 'AgentBrook系统使用说明\n包括系统开发的目的，背景，描述，支持的开发语言、数据库、设计结构、系统模块、部署环境的介绍，以及各个模块的操作说明，系统使用的注意事项。', 6, '千问2.5-14B', '你是一个很棒的客服！', 50.00, 80.00, 1, '2024-08-23 20:21:05', '2024-08-23 20:20:53');
COMMIT;

-- ----------------------------
-- Table structure for tb_agent_knowledge_base_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_agent_knowledge_base_info`;
CREATE TABLE `tb_agent_knowledge_base_info` (
  `AgentKnowledgeBaseID` int(11) NOT NULL AUTO_INCREMENT COMMENT '系统编号，唯一自动',
  `AgentID` varchar(64) NOT NULL COMMENT '应用编号',
  `KnowledgeBaseID` varchar(64) NOT NULL COMMENT '知识库编号',
  `Weight` int(11) NOT NULL DEFAULT '0' COMMENT '权重值，默认=0，值越大表示输出结果越倾向该库',
  PRIMARY KEY (`AgentKnowledgeBaseID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tb_agent_knowledge_base_info
-- ----------------------------
BEGIN;
INSERT INTO `tb_agent_knowledge_base_info` (`AgentKnowledgeBaseID`, `AgentID`, `KnowledgeBaseID`, `Weight`) VALUES (5, '532953076b44e67c80a5e24e01346dbe', '8b8512b07b18cce3f66cc03d5a9bf5d0', 0);
INSERT INTO `tb_agent_knowledge_base_info` (`AgentKnowledgeBaseID`, `AgentID`, `KnowledgeBaseID`, `Weight`) VALUES (6, '532953076b44e67c80a5e24e01346dbe', 'eb4d02f672407fac58f648681df7c9ce', 1);
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
