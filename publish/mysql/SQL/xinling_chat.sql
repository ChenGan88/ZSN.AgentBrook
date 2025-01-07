/*
 Navicat Premium Data Transfer

 Source Server         : MySQL
 Source Server Type    : MySQL
 Source Server Version : 50726 (5.7.26-log)
 Source Host           : 192.168.18.28:3306
 Source Schema         : xinling_chat

 Target Server Type    : MySQL
 Target Server Version : 50726 (5.7.26-log)
 File Encoding         : 65001

 Date: 04/01/2025 08:17:25
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

USE xinling_chat;

-- ----------------------------
-- Table structure for tb_app_chat_log_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_app_chat_log_info`;
CREATE TABLE `tb_app_chat_log_info` (
  `ChatLogID` varchar(64) NOT NULL COMMENT '对话记录编号',
  `ChatSessionID` varchar(64) DEFAULT NULL COMMENT '会话编号',
  `AppID` varchar(64) DEFAULT NULL COMMENT '应用ID',
  `Direction` int(11) DEFAULT NULL COMMENT '对话方向',
  `Role` varchar(50) DEFAULT NULL COMMENT '角色',
  `LargeModelID` int(11) DEFAULT NULL COMMENT 'AI模型编号',
  `Content` text COMMENT '内容',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间',
  `LogOrder` int(11) NOT NULL DEFAULT '0' COMMENT '会话内排序',
  PRIMARY KEY (`ChatLogID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of tb_app_chat_log_info
-- ----------------------------
BEGIN;
COMMIT;

-- ----------------------------
-- Table structure for tb_app_chat_session_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_app_chat_session_info`;
CREATE TABLE `tb_app_chat_session_info` (
  `ChatSessionID` varchar(64) NOT NULL COMMENT '对话记录编号',
  `AppID` varchar(64) DEFAULT NULL COMMENT '应用ID',
  `MemberID` varchar(64) NOT NULL COMMENT '用户编号',
  `DesensitizedName` varchar(255) DEFAULT NULL COMMENT '脱敏名称',
  `IsCoCreate` int(11) DEFAULT NULL COMMENT '是否参与共创',
  `SystemStatus` int(11) DEFAULT NULL COMMENT '系统状态',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间',
  PRIMARY KEY (`ChatSessionID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of tb_app_chat_session_info
-- ----------------------------
BEGIN;
COMMIT;

-- ----------------------------
-- Table structure for tb_app_chat_summary_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_app_chat_summary_info`;
CREATE TABLE `tb_app_chat_summary_info` (
  `SummaryID` varchar(64) NOT NULL COMMENT '总结编号',
  `AppID` varchar(64) DEFAULT NULL COMMENT '应用ID',
  `ChatSessionID` varchar(64) NOT NULL COMMENT '会话编号',
  `Content` json NOT NULL COMMENT '内容',
  `CreateTime` datetime NOT NULL COMMENT '创建时间',
  `ChatLogIDList` json NOT NULL COMMENT '对话记录总结范围',
  PRIMARY KEY (`SummaryID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='应用对话记录总结信息表，存储对话记录的总结信息';

-- ----------------------------
-- Records of tb_app_chat_summary_info
-- ----------------------------
BEGIN;
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
