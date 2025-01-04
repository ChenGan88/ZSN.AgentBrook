/*
 Navicat Premium Data Transfer

 Source Server         : MySQL
 Source Server Type    : MySQL
 Source Server Version : 50726 (5.7.26-log)
 Source Host           : 192.168.18.28:3306
 Source Schema         : xinling_workflow

 Target Server Type    : MySQL
 Target Server Version : 50726 (5.7.26-log)
 File Encoding         : 65001

 Date: 04/01/2025 08:19:45
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

USE xinling_workflow;

-- ----------------------------
-- Table structure for tb_workflow_edge_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_workflow_edge_info`;
CREATE TABLE `tb_workflow_edge_info` (
  `EdgeID` varchar(64) NOT NULL COMMENT '系统编号，唯一',
  `WorkflowID` varchar(64) NOT NULL COMMENT '所属工作流编号',
  `SourceNodeId` varchar(64) DEFAULT NULL COMMENT '源节点ID',
  `TargetNodeId` varchar(64) DEFAULT NULL COMMENT '目标节点ID',
  `Config` json DEFAULT NULL COMMENT '配置信息',
  `ConditionConfig` json DEFAULT NULL COMMENT '流向条件',
  `LName` varchar(50) DEFAULT NULL COMMENT '名称',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间',
  `LastUpdateTime` datetime DEFAULT NULL COMMENT '最后更新时间',
  PRIMARY KEY (`EdgeID`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tb_workflow_edge_info
-- ----------------------------
BEGIN;
INSERT INTO `tb_workflow_edge_info` (`EdgeID`, `WorkflowID`, `SourceNodeId`, `TargetNodeId`, `Config`, `ConditionConfig`, `LName`, `CreateTime`, `LastUpdateTime`) VALUES ('42475eaf-ea12-46cf-9992-a646833ae7d0', '2852be70-f006-4af3-bde0-ff6910a2f214', '56c828c8-6169-428f-85cd-bf79f8c49771', 'a97f9a14-8ede-4d90-9ffe-54e481a33ee7', '{\"id\": \"42475eaf-ea12-46cf-9992-a646833ae7d0\", \"data\": {}, \"type\": \"default\", \"label\": \"\", \"style\": \"stroke-width: 3;stroke:#3e47de;\", \"source\": \"56c828c8-6169-428f-85cd-bf79f8c49771\", \"target\": \"a97f9a14-8ede-4d90-9ffe-54e481a33ee7\", \"sourceX\": 81.24941672025142, \"sourceY\": -170.95296751984074, \"targetX\": 135.7019127807788, \"targetY\": 473.38566389369146, \"markerEnd\": \"arrowclosed\", \"sourceHandle\": null, \"targetHandle\": null}', NULL, '', '2025-01-03 15:19:05', '2025-01-03 15:19:05');
INSERT INTO `tb_workflow_edge_info` (`EdgeID`, `WorkflowID`, `SourceNodeId`, `TargetNodeId`, `Config`, `ConditionConfig`, `LName`, `CreateTime`, `LastUpdateTime`) VALUES ('7ada2552-bab4-4721-855e-05d99437f4f9', '2852be70-f006-4af3-bde0-ff6910a2f214', '9d4ce7c8-d928-46f3-b05a-c51f9e90d2f7', '56c828c8-6169-428f-85cd-bf79f8c49771', '{\"id\": \"7ada2552-bab4-4721-855e-05d99437f4f9\", \"data\": {}, \"type\": \"default\", \"label\": \"\", \"style\": \"stroke-width: 3;stroke:#3e47de;\", \"source\": \"9d4ce7c8-d928-46f3-b05a-c51f9e90d2f7\", \"target\": \"56c828c8-6169-428f-85cd-bf79f8c49771\", \"sourceX\": -745.3528383805084, \"sourceY\": -55.92481748964423, \"targetX\": -467.25056293469646, \"targetY\": -170.95296751984074, \"markerEnd\": \"arrowclosed\", \"sourceHandle\": null, \"targetHandle\": null}', NULL, '', '2025-01-03 15:19:05', '2025-01-03 15:19:05');
INSERT INTO `tb_workflow_edge_info` (`EdgeID`, `WorkflowID`, `SourceNodeId`, `TargetNodeId`, `Config`, `ConditionConfig`, `LName`, `CreateTime`, `LastUpdateTime`) VALUES ('a89d547c-6ce0-4860-89e0-beb84779826e', 'c1ee8b6b-e475-4a64-a538-76492b28fc94', '208cc381-4abf-4aaf-bda0-cd8331e598ac', 'e3cace41-c61a-4b28-ac80-6f148a656659', '{\"id\": \"a89d547c-6ce0-4860-89e0-beb84779826e\", \"data\": {}, \"type\": \"default\", \"label\": \"\", \"style\": \"stroke-width: 3;stroke:#3e47de;\", \"source\": \"208cc381-4abf-4aaf-bda0-cd8331e598ac\", \"target\": \"e3cace41-c61a-4b28-ac80-6f148a656659\", \"sourceX\": 733.2996849794465, \"sourceY\": 163.04525907035423, \"targetX\": 782.7914500058611, \"targetY\": 251.38946147717928, \"markerEnd\": \"arrowclosed\", \"sourceHandle\": null, \"targetHandle\": null}', NULL, '', '2025-01-03 19:53:54', '2025-01-03 19:53:54');
INSERT INTO `tb_workflow_edge_info` (`EdgeID`, `WorkflowID`, `SourceNodeId`, `TargetNodeId`, `Config`, `ConditionConfig`, `LName`, `CreateTime`, `LastUpdateTime`) VALUES ('b855ebb6-a6f5-49c0-b8a4-013039b5f88c', 'c1ee8b6b-e475-4a64-a538-76492b28fc94', '5f4b0471-03c6-4270-8c2e-3690fa15bd95', '208cc381-4abf-4aaf-bda0-cd8331e598ac', '{\"id\": \"b855ebb6-a6f5-49c0-b8a4-013039b5f88c\", \"data\": {}, \"type\": \"default\", \"label\": \"\", \"style\": \"stroke-width: 3;stroke:#3e47de;\", \"source\": \"5f4b0471-03c6-4270-8c2e-3690fa15bd95\", \"target\": \"208cc381-4abf-4aaf-bda0-cd8331e598ac\", \"sourceX\": 24.3105161432635, \"sourceY\": 301.6210607695998, \"targetX\": 76.29971549702464, \"targetY\": 163.04525907035423, \"markerEnd\": \"arrowclosed\", \"sourceHandle\": null, \"targetHandle\": null}', NULL, '', '2025-01-03 19:53:54', '2025-01-03 19:53:54');
INSERT INTO `tb_workflow_edge_info` (`EdgeID`, `WorkflowID`, `SourceNodeId`, `TargetNodeId`, `Config`, `ConditionConfig`, `LName`, `CreateTime`, `LastUpdateTime`) VALUES ('bd26231a-393e-40b9-becc-bb1249657253', 'b714a2f5-f12b-4c6e-940c-e0d95057597d', 'bda1cb30-e292-4490-8527-3708d92c16be', '24d40d54-9227-4abb-943a-79e2f8bbb941', '{\"id\": \"bd26231a-393e-40b9-becc-bb1249657253\", \"data\": {}, \"type\": \"default\", \"label\": \"\", \"style\": \"stroke-width: 3;stroke:#3e47de;\", \"source\": \"bda1cb30-e292-4490-8527-3708d92c16be\", \"target\": \"24d40d54-9227-4abb-943a-79e2f8bbb941\", \"sourceX\": -284, \"sourceY\": 397.75, \"targetX\": -6.999981013721569, \"targetY\": 287.75009103039906, \"markerEnd\": \"arrowclosed\", \"sourceHandle\": null, \"targetHandle\": \"target_1\"}', NULL, '', '2024-12-30 11:28:18', '2024-12-30 11:28:18');
INSERT INTO `tb_workflow_edge_info` (`EdgeID`, `WorkflowID`, `SourceNodeId`, `TargetNodeId`, `Config`, `ConditionConfig`, `LName`, `CreateTime`, `LastUpdateTime`) VALUES ('c1abe5ab-c143-4d47-be21-91dbdaebc825', '2852be70-f006-4af3-bde0-ff6910a2f214', 'a97f9a14-8ede-4d90-9ffe-54e481a33ee7', '56c828c8-6169-428f-85cd-bf79f8c49771', '{\"id\": \"c1abe5ab-c143-4d47-be21-91dbdaebc825\", \"data\": {}, \"type\": \"default\", \"label\": \"\", \"style\": \"stroke-width: 3;stroke:#3e47de;\", \"source\": \"a97f9a14-8ede-4d90-9ffe-54e481a33ee7\", \"target\": \"56c828c8-6169-428f-85cd-bf79f8c49771\", \"sourceX\": 792.7019432983569, \"sourceY\": 473.38566389369146, \"targetX\": -467.25056293469646, \"targetY\": -170.95296751984074, \"markerEnd\": \"arrowclosed\", \"sourceHandle\": null, \"targetHandle\": null}', NULL, '', '2025-01-03 15:19:05', '2025-01-03 15:19:05');
INSERT INTO `tb_workflow_edge_info` (`EdgeID`, `WorkflowID`, `SourceNodeId`, `TargetNodeId`, `Config`, `ConditionConfig`, `LName`, `CreateTime`, `LastUpdateTime`) VALUES ('f9fd4d81-a197-478c-a50f-798b202ad68b', '2852be70-f006-4af3-bde0-ff6910a2f214', '56c828c8-6169-428f-85cd-bf79f8c49771', '57d6a003-ea3d-4a55-bef1-b09fc25ece05', '{\"id\": \"f9fd4d81-a197-478c-a50f-798b202ad68b\", \"data\": {}, \"type\": \"default\", \"label\": \"\", \"style\": \"stroke-width: 3;stroke:#3e47de;\", \"source\": \"56c828c8-6169-428f-85cd-bf79f8c49771\", \"target\": \"57d6a003-ea3d-4a55-bef1-b09fc25ece05\", \"sourceX\": 81.24941672025142, \"sourceY\": -170.95296751984074, \"targetX\": 554.5487509716493, \"targetY\": -263.18401654891693, \"markerEnd\": \"arrowclosed\", \"sourceHandle\": null, \"targetHandle\": null}', NULL, '', '2025-01-03 15:19:05', '2025-01-03 15:19:05');
COMMIT;

-- ----------------------------
-- Table structure for tb_workflow_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_workflow_info`;
CREATE TABLE `tb_workflow_info` (
  `WorkflowID` varchar(64) NOT NULL COMMENT '系统编号，唯一',
  `MainType` int(11) DEFAULT NULL COMMENT '挂载主体类型，APP=1，Agent=2',
  `MainID` varchar(64) DEFAULT NULL COMMENT '挂载主体ID',
  `WorkflowName` varchar(128) DEFAULT NULL COMMENT '名称',
  `Description` varchar(512) DEFAULT NULL COMMENT '说明',
  `SystemStatus` int(11) DEFAULT NULL COMMENT '系统状态，屏蔽=-1，未发布=0，发布=1',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间',
  `LastUpdateTime` datetime DEFAULT NULL COMMENT '最后更新时间',
  PRIMARY KEY (`WorkflowID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tb_workflow_info
-- ----------------------------
BEGIN;
INSERT INTO `tb_workflow_info` (`WorkflowID`, `MainType`, `MainID`, `WorkflowName`, `Description`, `SystemStatus`, `CreateTime`, `LastUpdateTime`) VALUES ('2852be70-f006-4af3-bde0-ff6910a2f214', 1, '15c5fb785f4ce8a99717ea85f29690c9', '应用 工作流', '', 0, '2024-12-30 10:23:35', '2025-01-03 15:18:59');
INSERT INTO `tb_workflow_info` (`WorkflowID`, `MainType`, `MainID`, `WorkflowName`, `Description`, `SystemStatus`, `CreateTime`, `LastUpdateTime`) VALUES ('c1ee8b6b-e475-4a64-a538-76492b28fc94', 2, '020db83fc1d39990448074b97e938b52', 'AgentBrook使用说明工作流', '本工作流通过知识库的可以解答用户提出的关于AgentBrook的功能、使用、部署、开发语言、注意事项等问题。', 0, '2024-12-30 11:31:32', '2025-01-02 19:30:31');
COMMIT;

-- ----------------------------
-- Table structure for tb_workflow_node_excution_record_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_workflow_node_excution_record_info`;
CREATE TABLE `tb_workflow_node_excution_record_info` (
  `RecordID` varchar(64) NOT NULL COMMENT '系统编号，唯一',
  `SessionID` varchar(64) NOT NULL COMMENT '执行任务标识，工作流执行时的会话统一标识',
  `ProcessesID` varchar(64) NOT NULL COMMENT '任务编号',
  `WorkflowID` varchar(64) NOT NULL COMMENT '所属工作流编号',
  `NodeID` varchar(64) NOT NULL COMMENT '所属节点编号',
  `NodeName` varchar(64) NOT NULL DEFAULT '' COMMENT '所属节点名称',
  `NextNodeID` varchar(64) NOT NULL COMMENT '下一节点编号，结束=End',
  `StartTime` datetime DEFAULT NULL COMMENT '开始时间',
  `EndTime` datetime DEFAULT NULL COMMENT '结束时间',
  `Status` int(11) NOT NULL COMMENT '执行状态，失败=-1，运行中=0，成功=1',
  `Inputs` json DEFAULT NULL COMMENT '入参值',
  `Outputs` json DEFAULT NULL COMMENT '出参值',
  `Logs` json DEFAULT NULL COMMENT '运行记录',
  PRIMARY KEY (`RecordID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tb_workflow_node_excution_record_info
-- ----------------------------
BEGIN;
COMMIT;

-- ----------------------------
-- Table structure for tb_workflow_node_info
-- ----------------------------
DROP TABLE IF EXISTS `tb_workflow_node_info`;
CREATE TABLE `tb_workflow_node_info` (
  `NodeID` varchar(64) NOT NULL COMMENT '系统编号，唯一',
  `WorkflowID` varchar(64) NOT NULL COMMENT '所属工作流编号',
  `NodeType` varchar(50) NOT NULL COMMENT '节点类型，字典定义：开始节点、普通任务节点、决策节点、结束节点、Agent',
  `NodeName` varchar(50) NOT NULL COMMENT '节点名称',
  `Description` varchar(512) DEFAULT NULL COMMENT '节点描述',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间',
  `LastUpdateTime` datetime DEFAULT NULL COMMENT '最后更新时间',
  `Config` json DEFAULT NULL COMMENT '配置信息json格式',
  PRIMARY KEY (`NodeID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tb_workflow_node_info
-- ----------------------------
BEGIN;
INSERT INTO `tb_workflow_node_info` (`NodeID`, `WorkflowID`, `NodeType`, `NodeName`, `Description`, `CreateTime`, `LastUpdateTime`, `Config`) VALUES ('0f8a2f21-3e07-4951-b211-b45611ac0682', 'f9170c56-2d45-47bd-97c3-0bc90e42943c', 'Selector', 'Selector', 'Selector', '2024-12-30 11:31:08', '2024-12-30 11:31:08', '{\"id\": \"0f8a2f21-3e07-4951-b211-b45611ac0682\", \"data\": {\"label\": \"Selector\", \"inputs\": [{\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"input\", \"sourceId\": null}], \"output\": [{\"txt\": \"\", \"type\": \"string\", \"varname\": \"input\", \"sourceId\": null, \"displayText\": \"input (Unknown)\"}], \"selector\": []}, \"name\": null, \"type\": \"Selector\", \"mainid\": \"020db83fc1d39990448074b97e938b52\", \"position\": {\"x\": 0, \"y\": 0}, \"workflowid\": \"cef41fe3-b318-4ba0-8e7c-8d64b01f4635\", \"initialized\": false}');
INSERT INTO `tb_workflow_node_info` (`NodeID`, `WorkflowID`, `NodeType`, `NodeName`, `Description`, `CreateTime`, `LastUpdateTime`, `Config`) VALUES ('208cc381-4abf-4aaf-bda0-cd8331e598ac', 'c1ee8b6b-e475-4a64-a538-76492b28fc94', 'KnowledgeBase', 'Knowledge Base', 'Knowledge Base', '2025-01-03 19:53:54', '2025-01-03 19:53:54', '{\"id\": \"208cc381-4abf-4aaf-bda0-cd8331e598ac\", \"data\": {\"topp\": 80, \"label\": \"Knowledge Base\", \"model\": {\"Name\": \"\", \"MICON\": \"\", \"MConfig\": \"\", \"EndPoint\": \"\", \"ModelKey\": \"\", \"TypeCode\": 0, \"TypeName\": \"\", \"ModelName\": \"\", \"CreateTime\": \"2024-12-31T16:56:08\", \"UpdateTime\": \"2024-12-31T16:56:08\", \"Description\": \"\", \"LargeModelID\": 6, \"SystemStatus\": 0, \"ModelOrganizationID\": 0, \"ModelOrganizationName\": \"\"}, \"inputs\": [{\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"prompt\", \"sourceId\": \"5f4b0471-03c6-4270-8c2e-3690fa15bd95_prompt\"}], \"output\": [{\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"results\"}], \"prompt\": \"{{5f4b0471-03c6-4270-8c2e-3690fa15bd95_prompt}}\", \"relevance\": 70, \"sourceNodes\": {\"5f4b0471-03c6-4270-8c2e-3690fa15bd95\": {\"label\": \"Start\", \"nodeId\": \"5f4b0471-03c6-4270-8c2e-3690fa15bd95\", \"output\": [{\"id\": \"5f4b0471-03c6-4270-8c2e-3690fa15bd95_prompt\", \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"prompt\", \"displayText\": \"prompt\"}, {\"id\": \"5f4b0471-03c6-4270-8c2e-3690fa15bd95_currentTime\", \"txt\": \"当前时间\", \"type\": \"DateTime\", \"value\": \"{{currentTime}}\", \"varname\": \"currentTime\", \"displayText\": \"currentTime\"}]}}, \"temperature\": 70, \"knowledgeBase\": [{\"Name\": \"AgentBrook使用说明\", \"MemberID\": \"\", \"DicIDList\": \"15\", \"ChargeType\": 0, \"CreateTime\": \"2024-07-22T12:38:37\", \"Description\": \"\", \"DicNameList\": \"行业知识\", \"SystemStatus\": 0, \"VectorModelID\": 1, \"LastUpdateTime\": \"2024-07-22T12:38:07\", \"LineSliceCount\": 1000, \"OverlapSection\": 20, \"ParagraphSlice\": 1000, \"KnowledgeBaseID\": \"eb4d02f672407fac58f648681df7c9ce\", \"VectorModelName\": \"nomic-embed-text\", \"PreprocessModelID\": 2, \"PreprocessModelName\": \"llama3.3-70B\"}], \"NativeFunction\": [], \"SemanticFunction\": []}, \"name\": null, \"type\": \"KnowledgeBase\", \"mainid\": \"020db83fc1d39990448074b97e938b52\", \"position\": {\"x\": 83.29971549702464, \"y\": -347.25471855008846}, \"workflowid\": \"c1ee8b6b-e475-4a64-a538-76492b28fc94\", \"initialized\": false}');
INSERT INTO `tb_workflow_node_info` (`NodeID`, `WorkflowID`, `NodeType`, `NodeName`, `Description`, `CreateTime`, `LastUpdateTime`, `Config`) VALUES ('24d40d54-9227-4abb-943a-79e2f8bbb941', 'b714a2f5-f12b-4c6e-940c-e0d95057597d', 'Selector', 'Selector', 'Selector', '2024-12-30 11:28:18', '2024-12-30 11:28:18', '{\"id\": \"24d40d54-9227-4abb-943a-79e2f8bbb941\", \"data\": {\"label\": \"Selector\", \"inputs\": [{\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"currentTime\", \"sourceId\": \"bda1cb30-e292-4490-8527-3708d92c16be_currentTime\"}, {\"txt\": \"\", \"type\": \"string\", \"varname\": \"prompt\", \"sourceId\": \"bda1cb30-e292-4490-8527-3708d92c16be_prompt\"}], \"output\": [{\"txt\": \"\", \"type\": \"string\", \"nodeId\": \"bda1cb30-e292-4490-8527-3708d92c16be\", \"varname\": \"currentTime\", \"sourceId\": \"bda1cb30-e292-4490-8527-3708d92c16be_currentTime\", \"displayText\": \"currentTime (Start)\"}, {\"txt\": \"\", \"type\": \"string\", \"nodeId\": \"bda1cb30-e292-4490-8527-3708d92c16be\", \"varname\": \"prompt\", \"sourceId\": \"bda1cb30-e292-4490-8527-3708d92c16be_prompt\", \"displayText\": \"prompt (Start)\"}], \"selector\": [], \"sourceNodes\": {\"bda1cb30-e292-4490-8527-3708d92c16be\": {\"label\": \"Start\", \"nodeId\": \"bda1cb30-e292-4490-8527-3708d92c16be\", \"output\": [{\"id\": \"bda1cb30-e292-4490-8527-3708d92c16be_prompt\", \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"prompt\", \"displayText\": \"prompt\"}, {\"id\": \"bda1cb30-e292-4490-8527-3708d92c16be_currentTime\", \"txt\": \"当前时间\", \"type\": \"DateTime\", \"value\": \"{{currentTime}}\", \"varname\": \"currentTime\", \"displayText\": \"currentTime\"}]}}}, \"name\": null, \"type\": \"Selector\", \"mainid\": \"020db83fc1d39990448074b97e938b52\", \"position\": {\"x\": 0, \"y\": 0}, \"workflowid\": \"cef41fe3-b318-4ba0-8e7c-8d64b01f4635\", \"initialized\": false}');
INSERT INTO `tb_workflow_node_info` (`NodeID`, `WorkflowID`, `NodeType`, `NodeName`, `Description`, `CreateTime`, `LastUpdateTime`, `Config`) VALUES ('3cdcaf67-714e-4f87-a337-a6f6ad1dcda9', 'f9170c56-2d45-47bd-97c3-0bc90e42943c', 'AgentEnd', 'End', 'End', '2024-12-30 11:31:08', '2024-12-30 11:31:08', '{\"id\": \"3cdcaf67-714e-4f87-a337-a6f6ad1dcda9\", \"data\": {\"label\": \"End\", \"inputs\": [{\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"input\", \"sourceId\": null}], \"output\": [{\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"results\"}, {\"id\": null, \"txt\": \"当前时间\", \"type\": \"DateTime\", \"value\": \"{{currentTime}}\", \"varname\": \"currentTime\"}, {\"id\": null, \"txt\": \"\", \"type\": \"String\", \"value\": \"{{agentName}}\", \"varname\": \"agentName\"}], \"prompt\": \"\"}, \"name\": null, \"type\": \"AgentEnd\", \"mainid\": \"020db83fc1d39990448074b97e938b52\", \"position\": {\"x\": 1230, \"y\": 80}, \"workflowid\": \"f9170c56-2d45-47bd-97c3-0bc90e42943c\", \"initialized\": false}');
INSERT INTO `tb_workflow_node_info` (`NodeID`, `WorkflowID`, `NodeType`, `NodeName`, `Description`, `CreateTime`, `LastUpdateTime`, `Config`) VALUES ('53aed7e2-db6e-4061-806b-1010b4ae61c1', 'ce51b905-44ec-4496-8813-27f91062964e', 'AgentEnd', 'End', 'End', '2024-12-30 11:30:06', '2024-12-30 11:30:06', '{\"id\": \"53aed7e2-db6e-4061-806b-1010b4ae61c1\", \"data\": {\"label\": \"End\", \"inputs\": [{\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"input\", \"sourceId\": null}], \"output\": [{\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"results\"}, {\"id\": null, \"txt\": \"当前时间\", \"type\": \"DateTime\", \"value\": \"{{currentTime}}\", \"varname\": \"currentTime\"}, {\"id\": null, \"txt\": \"\", \"type\": \"String\", \"value\": \"{{agentName}}\", \"varname\": \"agentName\"}], \"prompt\": \"\"}, \"name\": null, \"type\": \"AgentEnd\", \"mainid\": \"020db83fc1d39990448074b97e938b52\", \"position\": {\"x\": 1230, \"y\": 80}, \"workflowid\": \"ce51b905-44ec-4496-8813-27f91062964e\", \"initialized\": false}');
INSERT INTO `tb_workflow_node_info` (`NodeID`, `WorkflowID`, `NodeType`, `NodeName`, `Description`, `CreateTime`, `LastUpdateTime`, `Config`) VALUES ('56c828c8-6169-428f-85cd-bf79f8c49771', '2852be70-f006-4af3-bde0-ff6910a2f214', 'MainAI', 'Main AI', 'Main AI', '2025-01-03 15:19:05', '2025-01-03 15:19:05', '{\"id\": \"56c828c8-6169-428f-85cd-bf79f8c49771\", \"data\": {\"topp\": \"10\", \"label\": \"Main AI\", \"model\": {\"Name\": \"\", \"MICON\": \"\", \"MConfig\": \"\", \"EndPoint\": \"\", \"ModelKey\": \"\", \"TypeCode\": 0, \"TypeName\": \"\", \"ModelName\": \"\", \"CreateTime\": \"2024-12-30T10:23:35\", \"UpdateTime\": \"2024-12-30T10:23:35\", \"Description\": \"\", \"LargeModelID\": 6, \"SystemStatus\": 0, \"ModelOrganizationID\": 0, \"ModelOrganizationName\": \"\"}, \"inputs\": [{\"txt\": \"\", \"type\": \"string\", \"varname\": \"prompt\", \"sourceId\": \"9d4ce7c8-d928-46f3-b05a-c51f9e90d2f7_prompt\"}, {\"txt\": \"\", \"type\": \"string\", \"varname\": \"currentTime\", \"sourceId\": \"9d4ce7c8-d928-46f3-b05a-c51f9e90d2f7_currentTime\"}], \"output\": [{\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"results\"}], \"prompt\": \"你的名字叫：AgentBrook助手。\\n你是一个系统使用助手，是一个热情又专业的助手。\\n你的主要工作是向正在使用AgentBrook系统的用户提供帮助。\\n\\n#比如：用户会向你提问（关于系统的具体功能的使用），你可以调用知识库的方式取得答案，并组织反馈给用户。如果没有找到相应的内容请不要随意回答，你可以直接回答（这个问题知识库中还没找到，我将记录下来。）\\n\\n#注意：你的回答仅限于本系统的功能说明，操作步骤与解释。超过该范围的问题，都不能回答。\\n#注意：每次都需要快速思考，可以从对话上下文中寻找已经回答的内容；回答内容需要简洁、专业。\\n#注意:必须使用中文回答。\\n\\n#当前时间#\\n{{9d4ce7c8-d928-46f3-b05a-c51f9e90d2f7_currentTime}}\\n\\n#用户的输入信息#\\n{{9d4ce7c8-d928-46f3-b05a-c51f9e90d2f7_prompt}}\\n\\n\", \"sourceNodes\": {\"56e2719c-a654-499b-8e4f-90e88f5dd1cf\": {\"label\": \"Agent\", \"nodeId\": \"56e2719c-a654-499b-8e4f-90e88f5dd1cf\", \"output\": [{\"id\": \"56e2719c-a654-499b-8e4f-90e88f5dd1cf_results\", \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"results\", \"displayText\": \"results\"}]}, \"9d4ce7c8-d928-46f3-b05a-c51f9e90d2f7\": {\"label\": \"Start\", \"nodeId\": \"9d4ce7c8-d928-46f3-b05a-c51f9e90d2f7\", \"output\": [{\"id\": \"9d4ce7c8-d928-46f3-b05a-c51f9e90d2f7_prompt\", \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"prompt\", \"displayText\": \"prompt\"}, {\"id\": \"9d4ce7c8-d928-46f3-b05a-c51f9e90d2f7_currentTime\", \"txt\": \"当前时间\", \"type\": \"DateTime\", \"value\": \"{{currentTime}}\", \"varname\": \"currentTime\", \"displayText\": \"currentTime\"}]}, \"a97f9a14-8ede-4d90-9ffe-54e481a33ee7\": {\"label\": \"Agent\", \"nodeId\": \"a97f9a14-8ede-4d90-9ffe-54e481a33ee7\", \"output\": [{\"id\": \"a97f9a14-8ede-4d90-9ffe-54e481a33ee7_results\", \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"results\", \"displayText\": \"results\"}, {\"id\": \"a97f9a14-8ede-4d90-9ffe-54e481a33ee7_currentTime\", \"txt\": \"\", \"type\": \"DateTime\", \"value\": \"\", \"varname\": \"currentTime\", \"displayText\": \"currentTime\"}, {\"id\": \"a97f9a14-8ede-4d90-9ffe-54e481a33ee7_agentName\", \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"agentName\", \"displayText\": \"agentName\"}]}}, \"temperature\": \"10\", \"NativeFunction\": [], \"SemanticFunction\": []}, \"name\": null, \"type\": \"MainAI\", \"mainid\": \"15c5fb785f4ce8a99717ea85f29690c9\", \"position\": {\"x\": -460.25056293469646, \"y\": -769.7529349677574}, \"workflowid\": \"2852be70-f006-4af3-bde0-ff6910a2f214\", \"initialized\": false}');
INSERT INTO `tb_workflow_node_info` (`NodeID`, `WorkflowID`, `NodeType`, `NodeName`, `Description`, `CreateTime`, `LastUpdateTime`, `Config`) VALUES ('57d6a003-ea3d-4a55-bef1-b09fc25ece05', '2852be70-f006-4af3-bde0-ff6910a2f214', 'End', 'End', 'End', '2025-01-03 15:19:05', '2025-01-03 15:19:05', '{\"id\": \"57d6a003-ea3d-4a55-bef1-b09fc25ece05\", \"data\": {\"label\": \"End\", \"inputs\": [{\"txt\": \"\", \"type\": \"string\", \"varname\": \"results\", \"sourceId\": \"56c828c8-6169-428f-85cd-bf79f8c49771_results\"}], \"prompt\": \"{{56c828c8-6169-428f-85cd-bf79f8c49771_results}}\", \"sourceNodes\": {\"56c828c8-6169-428f-85cd-bf79f8c49771\": {\"label\": \"Main AI\", \"nodeId\": \"56c828c8-6169-428f-85cd-bf79f8c49771\", \"output\": [{\"id\": \"56c828c8-6169-428f-85cd-bf79f8c49771_results\", \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"results\", \"displayText\": \"results\"}]}}}, \"name\": null, \"type\": \"End\", \"mainid\": \"15c5fb785f4ce8a99717ea85f29690c9\", \"position\": {\"x\": 561.5487509716493, \"y\": -513.4840246869378}, \"workflowid\": \"2852be70-f006-4af3-bde0-ff6910a2f214\", \"initialized\": false}');
INSERT INTO `tb_workflow_node_info` (`NodeID`, `WorkflowID`, `NodeType`, `NodeName`, `Description`, `CreateTime`, `LastUpdateTime`, `Config`) VALUES ('5f4b0471-03c6-4270-8c2e-3690fa15bd95', 'c1ee8b6b-e475-4a64-a538-76492b28fc94', 'AgentStart', 'Start', 'Start', '2025-01-03 19:53:54', '2025-01-03 19:53:54', '{\"id\": \"5f4b0471-03c6-4270-8c2e-3690fa15bd95\", \"data\": {\"label\": \"Start\", \"inputs\": [{\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"input\", \"sourceId\": null}], \"output\": [{\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"prompt\"}, {\"id\": null, \"txt\": \"当前时间\", \"type\": \"DateTime\", \"value\": \"{{currentTime}}\", \"varname\": \"currentTime\"}], \"prompt\": \"{{input}}\"}, \"name\": null, \"type\": \"AgentStart\", \"mainid\": \"020db83fc1d39990448074b97e938b52\", \"position\": {\"x\": -625.6894736842105, \"y\": -20.678947368421007}, \"workflowid\": \"c1ee8b6b-e475-4a64-a538-76492b28fc94\", \"initialized\": false}');
INSERT INTO `tb_workflow_node_info` (`NodeID`, `WorkflowID`, `NodeType`, `NodeName`, `Description`, `CreateTime`, `LastUpdateTime`, `Config`) VALUES ('8353a33d-8794-4c00-8655-11bec02dff33', '2852be70-f006-4af3-bde0-ff6910a2f214', 'TimeTrigger', 'TimeTrigger', 'TimeTrigger', '2025-01-03 15:19:05', '2025-01-03 15:19:05', '{\"id\": \"8353a33d-8794-4c00-8655-11bec02dff33\", \"data\": {\"label\": \"TimeTrigger\", \"output\": [{\"id\": null, \"txt\": \"当前时间\", \"type\": \"DateTime\", \"value\": \"{{currentTime}}\", \"varname\": \"currentTime\"}], \"prompt\": \"\", \"timeTrigger\": {\"id\": \"f57c2bbf-149d-421e-8289-c606219579ae\", \"top\": 0, \"Repeat\": 0, \"Interval\": 3600, \"LoopType\": null, \"StartTime\": \"2024-12-30T10:23:35\", \"LastRunTime\": \"2024-12-30T10:23:35\"}}, \"name\": null, \"type\": \"TimeTrigger\", \"mainid\": \"15c5fb785f4ce8a99717ea85f29690c9\", \"position\": {\"x\": -624.063290580316, \"y\": 515.5521062009016}, \"workflowid\": \"2852be70-f006-4af3-bde0-ff6910a2f214\", \"initialized\": false}');
INSERT INTO `tb_workflow_node_info` (`NodeID`, `WorkflowID`, `NodeType`, `NodeName`, `Description`, `CreateTime`, `LastUpdateTime`, `Config`) VALUES ('911490b3-a947-4a45-89ff-0642877a79db', '2852be70-f006-4af3-bde0-ff6910a2f214', 'Reporter', 'Reporter', 'Reporter', '2025-01-03 15:19:05', '2025-01-03 15:19:05', '{\"id\": \"911490b3-a947-4a45-89ff-0642877a79db\", \"data\": {\"topp\": 80, \"label\": \"Reporter\", \"model\": {\"Name\": \"\", \"MICON\": \"\", \"MConfig\": \"\", \"EndPoint\": \"\", \"ModelKey\": \"\", \"TypeCode\": 0, \"TypeName\": \"\", \"ModelName\": \"\", \"CreateTime\": \"2024-12-30T10:23:35\", \"UpdateTime\": \"2024-12-30T10:23:35\", \"Description\": \"\", \"LargeModelID\": 6, \"SystemStatus\": 0, \"ModelOrganizationID\": 0, \"ModelOrganizationName\": \"\"}, \"enable\": true, \"inputs\": [], \"output\": [], \"prompt\": \"你是一个谈话记录员,负责将对话内容进行整理,按角色分别提取关键点,并有条理得整理成Json格式，总结后的内容方便你再一次提取理解会话过程,有效避免会话上下文太长导致你的记忆丢失的问题。\\r\\n\\n\\r例子:\\n{\\n\\\"User\\\":\\\"User的内容的总结\\\",\\n\\\"+\\\"Assistant\\\":\\\"Assistant的内容的总结\\\"\\n\\\"Tool\\\":\\\"Tool的内容的总结\\\",\\n\\\"+}\\n\", \"temperature\": 80, \"recordslength\": 50, \"NativeFunction\": [], \"SemanticFunction\": []}, \"name\": null, \"type\": \"Reporter\", \"mainid\": \"15c5fb785f4ce8a99717ea85f29690c9\", \"position\": {\"x\": -1580.2408102189877, \"y\": 417.6171530606336}, \"workflowid\": \"2852be70-f006-4af3-bde0-ff6910a2f214\", \"initialized\": false}');
INSERT INTO `tb_workflow_node_info` (`NodeID`, `WorkflowID`, `NodeType`, `NodeName`, `Description`, `CreateTime`, `LastUpdateTime`, `Config`) VALUES ('91806ce0-9fb9-4a87-8ea0-f12b960203f5', 'ce51b905-44ec-4496-8813-27f91062964e', 'AgentStart', 'Start', 'Start', '2024-12-30 11:30:06', '2024-12-30 11:30:06', '{\"id\": \"91806ce0-9fb9-4a87-8ea0-f12b960203f5\", \"data\": {\"label\": \"Start\", \"inputs\": [{\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"input\", \"sourceId\": null}], \"output\": [{\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"prompt\"}, {\"id\": null, \"txt\": \"当前时间\", \"type\": \"DateTime\", \"value\": \"{{currentTime}}\", \"varname\": \"currentTime\"}], \"prompt\": \"\"}, \"name\": null, \"type\": \"AgentStart\", \"mainid\": \"020db83fc1d39990448074b97e938b52\", \"position\": {\"x\": -808, \"y\": 80}, \"workflowid\": \"ce51b905-44ec-4496-8813-27f91062964e\", \"initialized\": false}');
INSERT INTO `tb_workflow_node_info` (`NodeID`, `WorkflowID`, `NodeType`, `NodeName`, `Description`, `CreateTime`, `LastUpdateTime`, `Config`) VALUES ('9d4ce7c8-d928-46f3-b05a-c51f9e90d2f7', '2852be70-f006-4af3-bde0-ff6910a2f214', 'Start', 'Start', 'Start', '2025-01-03 15:19:05', '2025-01-03 15:19:05', '{\"id\": \"9d4ce7c8-d928-46f3-b05a-c51f9e90d2f7\", \"data\": {\"label\": \"Start\", \"inputs\": [{\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"input\", \"sourceId\": null}], \"output\": [{\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"prompt\"}, {\"id\": null, \"txt\": \"当前时间\", \"type\": \"DateTime\", \"value\": \"{{currentTime}}\", \"varname\": \"currentTime\"}], \"prompt\": \"{{input}}\"}, \"name\": null, \"type\": \"Start\", \"mainid\": \"15c5fb785f4ce8a99717ea85f29690c9\", \"position\": {\"x\": -1403.3529197607168, \"y\": -378.224805282613}, \"workflowid\": \"2852be70-f006-4af3-bde0-ff6910a2f214\", \"initialized\": false}');
INSERT INTO `tb_workflow_node_info` (`NodeID`, `WorkflowID`, `NodeType`, `NodeName`, `Description`, `CreateTime`, `LastUpdateTime`, `Config`) VALUES ('a97f9a14-8ede-4d90-9ffe-54e481a33ee7', '2852be70-f006-4af3-bde0-ff6910a2f214', 'Agent', 'AgentBrook系统使用说明', 'AgentBrook系统使用说明', '2025-01-03 15:19:05', '2025-01-03 15:19:05', '{\"id\": \"a97f9a14-8ede-4d90-9ffe-54e481a33ee7\", \"data\": {\"agent\": {\"Name\": \"\", \"AICON\": \"\", \"Prompt\": \"\", \"AgentID\": \"020db83fc1d39990448074b97e938b52\", \"MemberID\": \"\", \"DicIDList\": \"\", \"CreateTime\": \"2024-12-30T20:28:40\", \"MemberName\": \"\", \"Description\": \"\", \"DicNameList\": \"\", \"SystemStatus\": 0, \"KnowledgeBases\": [], \"LastUpdateTime\": \"2024-12-30T20:28:40\", \"SessionModelID\": null, \"TopPCoefficient\": 0, \"SessionModelName\": \"\", \"TemperatureCoefficient\": 0}, \"label\": \"AgentBrook系统使用说明\", \"inputs\": [{\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"results\", \"sourceId\": \"56c828c8-6169-428f-85cd-bf79f8c49771_results\"}], \"output\": [{\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"results\"}, {\"id\": null, \"txt\": \"\", \"type\": \"DateTime\", \"value\": \"\", \"varname\": \"currentTime\"}, {\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"agentName\"}], \"prompt\": \"这是AgentBrook的系统说明Agent，可以查找关于AgentBrook的系统的各个模块的使用说明，以及功能介绍，注意事项等。\\n\\n#上一节点的处理结果#\\n{{56c828c8-6169-428f-85cd-bf79f8c49771_results}}\", \"sourceNodes\": {\"56c828c8-6169-428f-85cd-bf79f8c49771\": {\"label\": \"Main AI\", \"nodeId\": \"56c828c8-6169-428f-85cd-bf79f8c49771\", \"output\": [{\"id\": \"56c828c8-6169-428f-85cd-bf79f8c49771_results\", \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"results\", \"displayText\": \"results\"}]}}}, \"name\": null, \"type\": \"Agent\", \"mainid\": \"15c5fb785f4ce8a99717ea85f29690c9\", \"position\": {\"x\": 142.7019127807788, \"y\": 71.7856781352279}, \"workflowid\": \"2852be70-f006-4af3-bde0-ff6910a2f214\", \"initialized\": false}');
INSERT INTO `tb_workflow_node_info` (`NodeID`, `WorkflowID`, `NodeType`, `NodeName`, `Description`, `CreateTime`, `LastUpdateTime`, `Config`) VALUES ('ac7a3266-909e-4460-8b35-7232b3f13ae3', 'b714a2f5-f12b-4c6e-940c-e0d95057597d', 'AgentEnd', 'End', 'End', '2024-12-30 11:28:18', '2024-12-30 11:28:18', '{\"id\": \"ac7a3266-909e-4460-8b35-7232b3f13ae3\", \"data\": {\"label\": \"End\", \"inputs\": [{\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"input\", \"sourceId\": null}], \"output\": [{\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"results\"}, {\"id\": null, \"txt\": \"当前时间\", \"type\": \"DateTime\", \"value\": \"{{currentTime}}\", \"varname\": \"currentTime\"}, {\"id\": null, \"txt\": \"\", \"type\": \"String\", \"value\": \"{{agentName}}\", \"varname\": \"agentName\"}], \"prompt\": \"\"}, \"name\": null, \"type\": \"AgentEnd\", \"mainid\": \"020db83fc1d39990448074b97e938b52\", \"position\": {\"x\": 693.7411986293649, \"y\": 18.51809920591444}, \"workflowid\": \"b714a2f5-f12b-4c6e-940c-e0d95057597d\", \"initialized\": false}');
INSERT INTO `tb_workflow_node_info` (`NodeID`, `WorkflowID`, `NodeType`, `NodeName`, `Description`, `CreateTime`, `LastUpdateTime`, `Config`) VALUES ('bda1cb30-e292-4490-8527-3708d92c16be', 'b714a2f5-f12b-4c6e-940c-e0d95057597d', 'AgentStart', 'Start', 'Start', '2024-12-30 11:28:18', '2024-12-30 11:28:18', '{\"id\": \"bda1cb30-e292-4490-8527-3708d92c16be\", \"data\": {\"label\": \"Start\", \"inputs\": [{\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"input\", \"sourceId\": null}], \"output\": [{\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"prompt\"}, {\"id\": null, \"txt\": \"当前时间\", \"type\": \"DateTime\", \"value\": \"{{currentTime}}\", \"varname\": \"currentTime\"}], \"prompt\": \"\"}, \"name\": null, \"type\": \"AgentStart\", \"mainid\": \"020db83fc1d39990448074b97e938b52\", \"position\": {\"x\": -808, \"y\": 80}, \"workflowid\": \"b714a2f5-f12b-4c6e-940c-e0d95057597d\", \"initialized\": false}');
INSERT INTO `tb_workflow_node_info` (`NodeID`, `WorkflowID`, `NodeType`, `NodeName`, `Description`, `CreateTime`, `LastUpdateTime`, `Config`) VALUES ('c59afb64-1118-462d-b0b0-85d7b8b77b03', 'f9170c56-2d45-47bd-97c3-0bc90e42943c', 'AgentStart', 'Start', 'Start', '2024-12-30 11:31:08', '2024-12-30 11:31:08', '{\"id\": \"c59afb64-1118-462d-b0b0-85d7b8b77b03\", \"data\": {\"label\": \"Start\", \"inputs\": [{\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"input\", \"sourceId\": null}], \"output\": [{\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"prompt\"}, {\"id\": null, \"txt\": \"当前时间\", \"type\": \"DateTime\", \"value\": \"{{currentTime}}\", \"varname\": \"currentTime\"}], \"prompt\": \"\"}, \"name\": null, \"type\": \"AgentStart\", \"mainid\": \"020db83fc1d39990448074b97e938b52\", \"position\": {\"x\": -808, \"y\": 80}, \"workflowid\": \"f9170c56-2d45-47bd-97c3-0bc90e42943c\", \"initialized\": false}');
INSERT INTO `tb_workflow_node_info` (`NodeID`, `WorkflowID`, `NodeType`, `NodeName`, `Description`, `CreateTime`, `LastUpdateTime`, `Config`) VALUES ('e3cace41-c61a-4b28-ac80-6f148a656659', 'c1ee8b6b-e475-4a64-a538-76492b28fc94', 'AgentEnd', 'End', 'End', '2025-01-03 19:53:54', '2025-01-03 19:53:54', '{\"id\": \"e3cace41-c61a-4b28-ac80-6f148a656659\", \"data\": {\"label\": \"End\", \"inputs\": [{\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"results\", \"sourceId\": \"208cc381-4abf-4aaf-bda0-cd8331e598ac_results\"}], \"output\": [{\"id\": null, \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"results\"}, {\"id\": null, \"txt\": \"当前时间\", \"type\": \"DateTime\", \"value\": \"{{currentTime}}\", \"varname\": \"currentTime\"}, {\"id\": null, \"txt\": \"\", \"type\": \"String\", \"value\": \"{{agentName}}\", \"varname\": \"agentName\"}], \"prompt\": \"{{208cc381-4abf-4aaf-bda0-cd8331e598ac_results}}\", \"sourceNodes\": {\"208cc381-4abf-4aaf-bda0-cd8331e598ac\": {\"label\": \"Knowledge Base\", \"nodeId\": \"208cc381-4abf-4aaf-bda0-cd8331e598ac\", \"output\": [{\"id\": \"208cc381-4abf-4aaf-bda0-cd8331e598ac_results\", \"txt\": \"\", \"type\": \"string\", \"value\": \"\", \"varname\": \"results\", \"displayText\": \"results\"}]}, \"3163ad38-3ef4-44d5-8560-6e5da3b99602\": {\"label\": \"Selector\", \"nodeId\": \"3163ad38-3ef4-44d5-8560-6e5da3b99602\", \"output\": [{\"id\": \"3163ad38-3ef4-44d5-8560-6e5da3b99602_input\", \"txt\": \"\", \"type\": \"string\", \"varname\": \"input\", \"sourceId\": null, \"displayText\": \"input (Unknown)\"}]}}}, \"name\": null, \"type\": \"AgentEnd\", \"mainid\": \"020db83fc1d39990448074b97e938b52\", \"position\": {\"x\": 789.7914500058611, \"y\": 1.0894736842105246}, \"workflowid\": \"c1ee8b6b-e475-4a64-a538-76492b28fc94\", \"initialized\": false}');
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
