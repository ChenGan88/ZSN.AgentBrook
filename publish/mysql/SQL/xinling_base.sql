/*
 Navicat Premium Data Transfer

 Source Server         : MySQL
 Source Server Type    : MySQL
 Source Server Version : 50726 (5.7.26-log)
 Source Host           : 192.168.18.28:3306
 Source Schema         : xinling_base

 Target Server Type    : MySQL
 Target Server Version : 50726 (5.7.26-log)
 File Encoding         : 65001

 Date: 04/01/2025 08:16:46
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

USE xinling_base;

-- ----------------------------
-- Table structure for base_dictionary_info
-- ----------------------------
DROP TABLE IF EXISTS `base_dictionary_info`;
CREATE TABLE `base_dictionary_info` (
  `DicId` int(11) NOT NULL AUTO_INCREMENT COMMENT '系统编号',
  `DicName` varchar(255) DEFAULT NULL COMMENT '名称',
  `DicTitle` varchar(255) DEFAULT NULL COMMENT 'Title',
  `DicValue` varchar(255) DEFAULT NULL COMMENT 'Value',
  `DicRemark` text COMMENT '备注',
  `Remark` varchar(512) DEFAULT NULL COMMENT '配置备注信息',
  `Status` int(11) DEFAULT NULL COMMENT '状态',
  `Sort` int(11) DEFAULT NULL COMMENT '排序',
  `Pid` int(11) DEFAULT NULL COMMENT '父级ID',
  `Cid` int(11) DEFAULT NULL COMMENT '保留字段',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间',
  `UpdateTime` datetime DEFAULT NULL COMMENT '最后更新时间',
  PRIMARY KEY (`DicId`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of base_dictionary_info
-- ----------------------------
BEGIN;
INSERT INTO `base_dictionary_info` (`DicId`, `DicName`, `DicTitle`, `DicValue`, `DicRemark`, `Remark`, `Status`, `Sort`, `Pid`, `Cid`, `CreateTime`, `UpdateTime`) VALUES (1, '模型类型', NULL, 'ModelType', NULL, NULL, 0, 0, 0, NULL, '2024-07-12 15:59:11', '2024-07-22 13:38:39');
INSERT INTO `base_dictionary_info` (`DicId`, `DicName`, `DicTitle`, `DicValue`, `DicRemark`, `Remark`, `Status`, `Sort`, `Pid`, `Cid`, `CreateTime`, `UpdateTime`) VALUES (3, '图片模型', '', 'Picture', '', '', 0, 2, 1, 0, '2024-07-12 16:20:37', '2024-07-22 13:31:06');
INSERT INTO `base_dictionary_info` (`DicId`, `DicName`, `DicTitle`, `DicValue`, `DicRemark`, `Remark`, `Status`, `Sort`, `Pid`, `Cid`, `CreateTime`, `UpdateTime`) VALUES (4, '会话模型', '', 'Chat', '', '', 0, 0, 1, 0, '2024-07-22 13:30:23', '2024-07-22 13:31:03');
INSERT INTO `base_dictionary_info` (`DicId`, `DicName`, `DicTitle`, `DicValue`, `DicRemark`, `Remark`, `Status`, `Sort`, `Pid`, `Cid`, `CreateTime`, `UpdateTime`) VALUES (5, '向量模型', '', 'Embedding', '', '', 0, 1, 1, 0, '2024-07-22 13:30:55', '2024-07-22 13:31:04');
INSERT INTO `base_dictionary_info` (`DicId`, `DicName`, `DicTitle`, `DicValue`, `DicRemark`, `Remark`, `Status`, `Sort`, `Pid`, `Cid`, `CreateTime`, `UpdateTime`) VALUES (6, 'Rerank排序模型', '', 'Rerank', '', '', 0, 3, 1, 0, '2024-07-22 13:31:26', '2024-07-22 13:31:30');
INSERT INTO `base_dictionary_info` (`DicId`, `DicName`, `DicTitle`, `DicValue`, `DicRemark`, `Remark`, `Status`, `Sort`, `Pid`, `Cid`, `CreateTime`, `UpdateTime`) VALUES (7, '模型机构', '', 'ModelOrganization', '', '', 0, 1, 0, NULL, '2024-07-22 13:38:32', '2024-07-22 13:38:45');
INSERT INTO `base_dictionary_info` (`DicId`, `DicName`, `DicTitle`, `DicValue`, `DicRemark`, `Remark`, `Status`, `Sort`, `Pid`, `Cid`, `CreateTime`, `UpdateTime`) VALUES (8, 'OpenAI', NULL, 'OpenAI', NULL, NULL, 0, NULL, 7, 0, '2024-07-22 13:39:13', '2024-07-22 13:39:13');
INSERT INTO `base_dictionary_info` (`DicId`, `DicName`, `DicTitle`, `DicValue`, `DicRemark`, `Remark`, `Status`, `Sort`, `Pid`, `Cid`, `CreateTime`, `UpdateTime`) VALUES (9, 'SparkDesk', NULL, 'SparkDesk', NULL, NULL, 0, NULL, 7, 0, '2024-07-22 13:39:46', '2024-07-22 13:39:46');
INSERT INTO `base_dictionary_info` (`DicId`, `DicName`, `DicTitle`, `DicValue`, `DicRemark`, `Remark`, `Status`, `Sort`, `Pid`, `Cid`, `CreateTime`, `UpdateTime`) VALUES (10, 'DashScope', NULL, 'DashScope', NULL, NULL, 0, NULL, 7, 0, '2024-07-22 13:40:04', '2024-07-22 13:40:04');
INSERT INTO `base_dictionary_info` (`DicId`, `DicName`, `DicTitle`, `DicValue`, `DicRemark`, `Remark`, `Status`, `Sort`, `Pid`, `Cid`, `CreateTime`, `UpdateTime`) VALUES (11, 'LLamaFactory', NULL, 'LLamaFactory', NULL, NULL, 0, NULL, 7, 0, '2024-07-22 13:40:19', '2024-07-22 13:40:19');
INSERT INTO `base_dictionary_info` (`DicId`, `DicName`, `DicTitle`, `DicValue`, `DicRemark`, `Remark`, `Status`, `Sort`, `Pid`, `Cid`, `CreateTime`, `UpdateTime`) VALUES (12, 'Ollama', NULL, 'Ollama', NULL, NULL, 0, NULL, 7, 0, '2024-07-22 13:40:38', '2024-07-22 13:40:38');
INSERT INTO `base_dictionary_info` (`DicId`, `DicName`, `DicTitle`, `DicValue`, `DicRemark`, `Remark`, `Status`, `Sort`, `Pid`, `Cid`, `CreateTime`, `UpdateTime`) VALUES (13, '知识库类型', '', 'KnowledgeBaseType', '', '', 0, 2, 0, NULL, '2024-07-22 14:27:16', '2024-07-22 14:27:24');
INSERT INTO `base_dictionary_info` (`DicId`, `DicName`, `DicTitle`, `DicValue`, `DicRemark`, `Remark`, `Status`, `Sort`, `Pid`, `Cid`, `CreateTime`, `UpdateTime`) VALUES (14, '泛娱乐', NULL, NULL, NULL, NULL, 0, NULL, 13, 0, '2024-07-22 14:29:06', '2024-07-22 14:29:43');
INSERT INTO `base_dictionary_info` (`DicId`, `DicName`, `DicTitle`, `DicValue`, `DicRemark`, `Remark`, `Status`, `Sort`, `Pid`, `Cid`, `CreateTime`, `UpdateTime`) VALUES (15, '行业知识', NULL, NULL, NULL, NULL, 0, NULL, 13, 0, '2024-07-22 14:29:16', '2024-07-22 14:29:31');
INSERT INTO `base_dictionary_info` (`DicId`, `DicName`, `DicTitle`, `DicValue`, `DicRemark`, `Remark`, `Status`, `Sort`, `Pid`, `Cid`, `CreateTime`, `UpdateTime`) VALUES (16, '专业知识', NULL, NULL, NULL, NULL, 0, NULL, 13, 0, '2024-07-22 14:29:51', '2024-07-22 14:29:51');
INSERT INTO `base_dictionary_info` (`DicId`, `DicName`, `DicTitle`, `DicValue`, `DicRemark`, `Remark`, `Status`, `Sort`, `Pid`, `Cid`, `CreateTime`, `UpdateTime`) VALUES (17, '应用类型', NULL, 'AppType', NULL, NULL, 0, NULL, 0, NULL, '2024-07-22 15:15:39', '2024-07-22 15:15:39');
INSERT INTO `base_dictionary_info` (`DicId`, `DicName`, `DicTitle`, `DicValue`, `DicRemark`, `Remark`, `Status`, `Sort`, `Pid`, `Cid`, `CreateTime`, `UpdateTime`) VALUES (18, '心里健康', NULL, NULL, NULL, NULL, 0, NULL, 17, 0, '2024-07-22 15:16:00', '2024-07-22 15:16:00');
INSERT INTO `base_dictionary_info` (`DicId`, `DicName`, `DicTitle`, `DicValue`, `DicRemark`, `Remark`, `Status`, `Sort`, `Pid`, `Cid`, `CreateTime`, `UpdateTime`) VALUES (19, '文献查找', NULL, NULL, NULL, NULL, 0, NULL, 17, 0, '2024-07-22 15:16:10', '2024-07-22 15:16:10');
INSERT INTO `base_dictionary_info` (`DicId`, `DicName`, `DicTitle`, `DicValue`, `DicRemark`, `Remark`, `Status`, `Sort`, `Pid`, `Cid`, `CreateTime`, `UpdateTime`) VALUES (20, '企业知识库', NULL, NULL, NULL, NULL, 0, NULL, 17, 0, '2024-07-22 15:16:18', '2024-07-22 15:16:18');
INSERT INTO `base_dictionary_info` (`DicId`, `DicName`, `DicTitle`, `DicValue`, `DicRemark`, `Remark`, `Status`, `Sort`, `Pid`, `Cid`, `CreateTime`, `UpdateTime`) VALUES (21, '智能体类型', NULL, 'AgentType', NULL, NULL, 0, NULL, 0, NULL, '2024-08-07 15:29:51', '2024-08-07 15:30:13');
INSERT INTO `base_dictionary_info` (`DicId`, `DicName`, `DicTitle`, `DicValue`, `DicRemark`, `Remark`, `Status`, `Sort`, `Pid`, `Cid`, `CreateTime`, `UpdateTime`) VALUES (22, '客服', NULL, NULL, NULL, NULL, 0, NULL, 21, 0, '2024-08-07 15:31:34', '2024-08-07 15:31:34');
INSERT INTO `base_dictionary_info` (`DicId`, `DicName`, `DicTitle`, `DicValue`, `DicRemark`, `Remark`, `Status`, `Sort`, `Pid`, `Cid`, `CreateTime`, `UpdateTime`) VALUES (23, '助理', '', '', '', '', 0, NULL, 21, 0, '2025-01-04 08:11:14', '2025-01-04 08:11:30');
COMMIT;

-- ----------------------------
-- Table structure for tb_menu_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_menu_info`;
CREATE TABLE `tb_menu_info` (
  `ID` varchar(128) NOT NULL COMMENT '系统编号',
  `ParentID` varchar(128) DEFAULT NULL COMMENT '上级编号',
  `Url` varchar(255) DEFAULT NULL COMMENT '链接',
  `Title` varchar(128) DEFAULT NULL COMMENT '显示标题',
  `Params` varchar(512) DEFAULT NULL COMMENT '预留参数',
  `Ico` varchar(50) DEFAULT NULL COMMENT '图标',
  `Sort` int(11) DEFAULT NULL COMMENT '显示排序',
  `IcoColor` varchar(50) DEFAULT NULL COMMENT '颜色',
  `MState` int(11) DEFAULT NULL COMMENT '系统状态',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='菜单信息';

-- ----------------------------
-- Records of tb_menu_info
-- ----------------------------
BEGIN;
INSERT INTO `tb_menu_info` (`ID`, `ParentID`, `Url`, `Title`, `Params`, `Ico`, `Sort`, `IcoColor`, `MState`) VALUES ('02cfcbeb-e1fe-4702-a537-99e8e58fddd8', '0fe9d070-9a80-400c-a150-9c7bd427fd10', '/Manage/SystemLog/LogMarkList', '日志锚点', 'menu', '', 9, '', 0);
INSERT INTO `tb_menu_info` (`ID`, `ParentID`, `Url`, `Title`, `Params`, `Ico`, `Sort`, `IcoColor`, `MState`) VALUES ('0fe9d070-9a80-400c-a150-9c7bd427fd10', '9BBEBABB-ACCD-4E2E-9AAE-61D072553C93', '', '开发日志', 'menu', '', 7, '', 0);
INSERT INTO `tb_menu_info` (`ID`, `ParentID`, `Url`, `Title`, `Params`, `Ico`, `Sort`, `IcoColor`, `MState`) VALUES ('28a4627c-1fcc-4526-8f2b-ea33fdf437ff', '9BBEBABB-ACCD-4E2E-9AAE-61D072553C93', '', '设置', 'menu', '', 6, '', 0);
INSERT INTO `tb_menu_info` (`ID`, `ParentID`, `Url`, `Title`, `Params`, `Ico`, `Sort`, `IcoColor`, `MState`) VALUES ('2a38653f-33f3-4f3d-a407-f2dd17b388ab', '28a4627c-1fcc-4526-8f2b-ea33fdf437ff', '', '工作流管理', NULL, '', 9, '', 0);
INSERT INTO `tb_menu_info` (`ID`, `ParentID`, `Url`, `Title`, `Params`, `Ico`, `Sort`, `IcoColor`, `MState`) VALUES ('3203f760-31cb-46fd-8cdf-3881b15745b4', '28a4627c-1fcc-4526-8f2b-ea33fdf437ff', '/Manage/Dictionary/DictionaryList', '字典管理', 'menu', '', 6, '', 0);
INSERT INTO `tb_menu_info` (`ID`, `ParentID`, `Url`, `Title`, `Params`, `Ico`, `Sort`, `IcoColor`, `MState`) VALUES ('36283e50-64e2-4a7f-942d-b9a4747a9c59', '9BBEBABB-ACCD-4E2E-9AAE-61D072553C93', '', '会员管理', 'menu', '', 5, '', 0);
INSERT INTO `tb_menu_info` (`ID`, `ParentID`, `Url`, `Title`, `Params`, `Ico`, `Sort`, `IcoColor`, `MState`) VALUES ('4ecd232e-45be-419e-9485-6d02a52aaa7b', '28a4627c-1fcc-4526-8f2b-ea33fdf437ff', '/Manage/KnowledgeBase/index', '知识库管理', 'menu', '', 2, '', 0);
INSERT INTO `tb_menu_info` (`ID`, `ParentID`, `Url`, `Title`, `Params`, `Ico`, `Sort`, `IcoColor`, `MState`) VALUES ('69bdae79-4c45-4642-aa3a-dcd8c4aae41c', '0fe9d070-9a80-400c-a150-9c7bd427fd10', '/Manage/SystemLog/LogRecordList', '日志记录', 'menu', '', 10, '', 0);
INSERT INTO `tb_menu_info` (`ID`, `ParentID`, `Url`, `Title`, `Params`, `Ico`, `Sort`, `IcoColor`, `MState`) VALUES ('6f690227-a3a2-49bf-a599-12f0728d1321', '28a4627c-1fcc-4526-8f2b-ea33fdf437ff', '/Manage/App/index', '应用管理', 'menu', '', 5, '', 0);
INSERT INTO `tb_menu_info` (`ID`, `ParentID`, `Url`, `Title`, `Params`, `Ico`, `Sort`, `IcoColor`, `MState`) VALUES ('7b7f00c1-e505-4a52-a035-a182e70bf79f', '28a4627c-1fcc-4526-8f2b-ea33fdf437ff', '/Manage/Plugins/index', '插件管理', 'menu', '', 3, '', 0);
INSERT INTO `tb_menu_info` (`ID`, `ParentID`, `Url`, `Title`, `Params`, `Ico`, `Sort`, `IcoColor`, `MState`) VALUES ('9BBEBABB-ACCD-4E2E-9AAE-61D072553C93', '00000000-0000-0000-0000-000000000000', NULL, '菜单目录', NULL, NULL, 0, NULL, 0);
INSERT INTO `tb_menu_info` (`ID`, `ParentID`, `Url`, `Title`, `Params`, `Ico`, `Sort`, `IcoColor`, `MState`) VALUES ('b0d9564e-ae5b-460e-962f-0e3505b8a951', '0fe9d070-9a80-400c-a150-9c7bd427fd10', '/Manage/SystemLog/LogLevelList', '日志等级', 'menu', '', 7, '', 0);
INSERT INTO `tb_menu_info` (`ID`, `ParentID`, `Url`, `Title`, `Params`, `Ico`, `Sort`, `IcoColor`, `MState`) VALUES ('cc2724ab-0d38-4b4f-8b60-2e4f673789e0', '9BBEBABB-ACCD-4E2E-9AAE-61D072553C93', '/Manage/Main', '主页面', NULL, '', 4, '', 0);
INSERT INTO `tb_menu_info` (`ID`, `ParentID`, `Url`, `Title`, `Params`, `Ico`, `Sort`, `IcoColor`, `MState`) VALUES ('d30966d3-198f-4ba2-8950-aac7918cc2b1', '28a4627c-1fcc-4526-8f2b-ea33fdf437ff', '/Manage/LargeModel/index', '模型管理', 'menu', '', 1, '', 0);
INSERT INTO `tb_menu_info` (`ID`, `ParentID`, `Url`, `Title`, `Params`, `Ico`, `Sort`, `IcoColor`, `MState`) VALUES ('e18ae3d5-c80c-4242-9ed9-e4ae0e484b51', '28a4627c-1fcc-4526-8f2b-ea33fdf437ff', '/Manage/Agent/index', 'Agent任务管理', 'menu', '', 4, '', 0);
INSERT INTO `tb_menu_info` (`ID`, `ParentID`, `Url`, `Title`, `Params`, `Ico`, `Sort`, `IcoColor`, `MState`) VALUES ('e417f4d3-f638-4f76-9ade-8359a1f086e5', '28a4627c-1fcc-4526-8f2b-ea33fdf437ff', '/Manage/UserManage/index', '系统用户', 'menu', '', 8, '', 0);
INSERT INTO `tb_menu_info` (`ID`, `ParentID`, `Url`, `Title`, `Params`, `Ico`, `Sort`, `IcoColor`, `MState`) VALUES ('eccec41e-972a-447b-bdc7-17050082c978', '28a4627c-1fcc-4526-8f2b-ea33fdf437ff', '/Manage/Menu/index', '菜单管理', 'menu', '', 7, '', 0);
INSERT INTO `tb_menu_info` (`ID`, `ParentID`, `Url`, `Title`, `Params`, `Ico`, `Sort`, `IcoColor`, `MState`) VALUES ('fa2227b9-cfb1-49c1-86fd-953e865e6018', '36283e50-64e2-4a7f-942d-b9a4747a9c59', '/Manage/MemberManage/index', '会员信息', 'menu', '', 2, '', 0);
COMMIT;

-- ----------------------------
-- Table structure for tb_user_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_user_info`;
CREATE TABLE `tb_user_info` (
  `UserID` int(11) NOT NULL AUTO_INCREMENT COMMENT '用户编号',
  `uName` varchar(50) DEFAULT NULL COMMENT '用户名',
  `uPWD` varchar(32) DEFAULT NULL COMMENT '密码',
  `PermissionCode` varchar(2048) DEFAULT NULL COMMENT '权限代码',
  `uState` int(11) NOT NULL COMMENT '系统状态',
  `uAppendTime` datetime DEFAULT NULL COMMENT '创建时间',
  PRIMARY KEY (`UserID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of tb_user_info
-- ----------------------------
BEGIN;
INSERT INTO `tb_user_info` (`UserID`, `uName`, `uPWD`, `PermissionCode`, `uState`, `uAppendTime`) VALUES (1, 'admin', '02867ae092c92e1191ddf827a0b6b038', '9BBEBABB-ACCD-4E2E-9AAE-61D072553C93,', 0, '2024-07-12 11:45:50');
COMMIT;

-- ----------------------------
-- Table structure for tb_vcode_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_vcode_info`;
CREATE TABLE `tb_vcode_info` (
  `VCodeID` int(11) NOT NULL AUTO_INCREMENT COMMENT '验证码编号',
  `PhoneNumber` varchar(20) NOT NULL COMMENT '验证手机号码',
  `VCode` char(6) NOT NULL COMMENT '验证码',
  `vAppendTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `vState` int(11) NOT NULL COMMENT '系统状态',
  PRIMARY KEY (`VCodeID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of tb_vcode_info
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
