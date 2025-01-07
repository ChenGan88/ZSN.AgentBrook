/*
 Navicat Premium Data Transfer

 Source Server         : MySQL
 Source Server Type    : MySQL
 Source Server Version : 50726 (5.7.26-log)
 Source Host           : 192.168.18.28:3306
 Source Schema         : xinling_job

 Target Server Type    : MySQL
 Target Server Version : 50726 (5.7.26-log)
 File Encoding         : 65001

 Date: 04/01/2025 08:17:56
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

USE xinling_job;

-- ----------------------------
-- Table structure for tb_task_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_task_info`;
CREATE TABLE `tb_task_info` (
  `TaskID` varchar(64) NOT NULL COMMENT '对话记录编号，唯一标识每个任务，自动生成',
  `TaskType` int(11) NOT NULL COMMENT '任务类型，表示不同种类的异步操作',
  `TaskConfig` json NOT NULL COMMENT '任务参数，以JSON格式存储，定义任务配置参数',
  `CreateTime` datetime NOT NULL COMMENT '创建时间，记录任务的创建时间',
  `UpdateTime` datetime NOT NULL COMMENT '最后更新时间，记录任务的最后更新时间',
  `State` int(11) NOT NULL COMMENT '任务状态，0=等待中，1=处理中，2=完成，-1=失败',
  `Results` json DEFAULT NULL COMMENT '处理结果，以JSON格式存储任务处理结果',
  `LoopType` int(11) NOT NULL DEFAULT '0' COMMENT '重复周期类型\n0=不重复\n1=秒\n2=天\n3=每周几\n4=每月第几天 ',
  `IntervalValue` json DEFAULT NULL COMMENT 'IntervalValue不等于0时启用',
  `RepeatValue` int(11) NOT NULL DEFAULT '0' COMMENT '默认=0，无限次\r\nInterval等于0时表示任务失败重试次数',
  `RedoCount` int(11) NOT NULL DEFAULT '0',
  `FromTaskID` varchar(64) DEFAULT NULL COMMENT '下发任务标识',
  `FromMainTaskID` varchar(64) DEFAULT '' COMMENT '主任务标识',
  PRIMARY KEY (`TaskID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='异步任务信息表，支持任务创建、状态更新以及结果存储';

-- ----------------------------
-- Records of tb_task_info
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
