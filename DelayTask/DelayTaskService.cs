﻿using DelayTask.Model;
using NetworkSocket;
using NetworkSocket.Fast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelayTask
{
    /// <summary>
    /// 延时任务服务
    /// </summary>
    public class DelayTaskService : FastApiService
    {
        /// <summary>
        /// 任务调度列表
        /// </summary>
        private TaskSheduler taskSheduler = new TaskSheduler();

        /// <summary>
        /// 延时任务服务
        /// </summary>
        public DelayTaskService()
        {
            this.taskSheduler.StartSheduler();
        }

        /// <summary>
        /// 释放内存前
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            this.taskSheduler.TaskList.Dispose();
            base.Dispose(disposing);
        }


        /// <summary>
        /// 获取任务的最近错误信息
        /// </summary>       
        /// <param name="id">任务id</param>
        /// <returns></returns>       
        public string GetTaskLastError(Guid id)
        {
            return LastErrors.GetLastError(id);
        }

        /// <summary>
        /// 添加或设置Sql任务
        /// </summary>       
        /// <param name="taskConfig">任务配置</param>      
        /// <param name="connectingString">连接字符串</param>
        /// <param name="sql">SQL语句</param>        
        public bool SetSqlTask(TaskBaseConfig taskConfig, string connectingString, string sql)
        {
            var task = new SqlTask(taskConfig, connectingString, sql);
            return this.taskSheduler.TaskList.SetTask(task);
        }

        /// <summary>
        /// 添加或设置Http任务
        /// </summary>      
        /// <param name="taskConfig">任务配置</param>
        /// <param name="url">请求的URL</param>
        /// <param name="param">请求的参数</param>        
        public bool SetHttpTask(TaskBaseConfig taskConfig, string url, string param)
        {
            var task = new HttpTask(taskConfig, url, param);
            return this.taskSheduler.TaskList.SetTask(task);
        }

        /// <summary>
        /// 删除任务
        /// </summary>       
        /// <param name="id">任务id</param>
        /// <returns></returns>        
        public bool RemoveTask(Guid id)
        {
            return this.taskSheduler.TaskList.RemoveTask(id);
        }

        /// <summary>
        /// 继续延长任务的执行时间
        /// </summary>      
        /// <param name="id">任务id</param>
        /// <param name="delaySeconds">延长秒数</param>       
        public bool AddTaskDelaySeconds(Guid id, int delaySeconds)
        {
            return this.taskSheduler.TaskList.AddTaskDelaySeconds(id, delaySeconds);
        }


        /// <summary>
        /// 获取失败的任务分页
        /// </summary>        
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="sourceId">原始任务的id(Empty表示所有sourceId)</param>
        /// <param name="taskType">任务类型(为空则所有类型)</param>
        /// <returns></returns>       
        public TaskBasePage GetFailureTaskPage(int pageIndex, int pageSize, Guid sourceId, string taskType)
        {
            return this.taskSheduler.TaskList.GetFailureTaskPage(pageIndex, pageSize, sourceId, taskType);
        }


        /// <summary>
        /// 执行一个失败的任务
        /// 执行成功则自动从失败列表中移除
        /// </summary>      
        /// <param name="id">任务id</param>
        /// <returns></returns>      
        public bool ExecuteFailureTask(Guid id)
        {
            return this.taskSheduler.TaskList.ExecuteFailureTask(id);
        }

        /// <summary>
        /// 获取待运行的任务分页
        /// </summary>      
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="taskType">任务类型(为空则所有类型)</param>
        /// <returns></returns>       
        public TaskBasePage GeTaskPage(int pageIndex, int pageSize, string taskType)
        {
            return this.taskSheduler.TaskList.GetTaskPage(pageIndex, pageSize, taskType);
        }
    }
}
