using EvanBackstageApi.Basic;
using EvanBackstageApi.Entity;
using EvanBackstageApi.IService;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EvanBackstageApi.Controllers
{
    /// <summary>
    /// 基本参考 （部分延伸条件不写出）
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private static ILog log = LogManager.GetLogger(Startup.Repository.Name, "StudentController");
        private readonly IStudentService _iStudentService;

        public StudentController(IStudentService iStudentService)
        {
            _iStudentService = iStudentService;
            log.Info("进入到控制器StudentController");
        }

        /// <summary>
        /// 单体添加
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Add")]
        public async Task<ResultModel<Student>> Add([FromBody] Student student)
        {
            try
            {
                var result = await _iStudentService.Add(student);
                if (result != 0)
                {
                    return new ResultModel<Student> { State = ResultType.Success.ToString(), Message = "添加成功", Data = student };
                }
            }
            catch (Exception e)
            {
                return new ResultModel<Student> { State = ResultType.Error.ToString(), Message = "添加失败", Data = student };
            }
            return new ResultModel<Student> { State = ResultType.Error.ToString(), Message = "添加失败", Data = student };
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="students"></param>
        /// <returns></returns>
        [HttpPost("AddList")]
        public async Task<ResultModel<List<Student>>> AddList([FromBody] List<Student> students)
        {
            try
            {
                var result = await _iStudentService.Add(students);
                if (result != 0)
                {
                    return new ResultModel<List<Student>> { State = ResultType.Success.ToString(), Message = "添加成功", Data = students };
                }
            }
            catch (Exception e)
            {
                return new ResultModel<List<Student>> { State = ResultType.Error.ToString(), Message = "添加失败", Data = students };
            }
            return new ResultModel<List<Student>> { State = ResultType.Error.ToString(), Message = "添加失败", Data = students };
        }
        /// <summary>
        /// 单体删除
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpGet("Delete")]
        public async Task<ResultModel<Student>> Delete([FromBody] Student student)
        {
            try
            {
                var result = await _iStudentService.Delete(student);
                if (result)
                {
                    return new ResultModel<Student> { State = ResultType.Success.ToString(), Message = "删除成功", Data = student };
                }
            }
            catch (Exception e)
            {
                return new ResultModel<Student> { State = ResultType.Error.ToString(), Message = "删除成功", Data = student };
            }
            return new ResultModel<Student> { State = ResultType.Error.ToString(), Message = "删除成功", Data = student };
        }
        /// <summary>
        /// 根据id去删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("DeleteById")]
        public async Task<ResultModel<Student>> DeleteById(int id)
        {
            try
            {
                var result = await _iStudentService.DeleteById(id);
                if (result)
                {
                    return new ResultModel<Student> { State = ResultType.Success.ToString(), Message = "删除成功" };
                }
            }
            catch (Exception e)
            {
                return new ResultModel<Student> { State = ResultType.Error.ToString(), Message = "删除成功" };
            }
            return new ResultModel<Student> { State = ResultType.Error.ToString(), Message = "删除成功" };
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet("DeleteByIds")]
        public async Task<ResultModel<Student>> DeleteByIds(object[] ids)
        {
            try
            {
                var result = await _iStudentService.DeleteByIds(ids);
                if (result)
                {
                    return new ResultModel<Student> { State = ResultType.Success.ToString(), Message = "删除成功" };
                }
            }
            catch (Exception e)
            {
                return new ResultModel<Student> { State = ResultType.Error.ToString(), Message = "删除成功" };
            }
            return new ResultModel<Student> { State = ResultType.Error.ToString(), Message = "删除成功" };
        }
        /// <summary>
        /// 单体更新
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpGet("Update")]
        public async Task<ResultModel<Student>> Update([FromBody] Student student)
        {
            try
            {
                var result = await _iStudentService.Update(student);
                if (result)
                {
                    return new ResultModel<Student> { State = ResultType.Success.ToString(), Message = "更新成功" };
                }
            }
            catch (Exception e)
            {
                return new ResultModel<Student> { State = ResultType.Error.ToString(), Message = "更新成功" };
            }
            return new ResultModel<Student> { State = ResultType.Error.ToString(), Message = "更新成功" };
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="students"></param>
        /// <returns></returns>
        [HttpGet("UpdateList")]
        public async Task<ResultModel<Student>> UpdateList([FromBody] List<Student> students)
        {
            try
            {
                var result = await _iStudentService.Update(students);
                if (result)
                {
                    return new ResultModel<Student> { State = ResultType.Success.ToString(), Message = "更新成功" };
                }
            }
            catch (Exception e)
            {
                return new ResultModel<Student> { State = ResultType.Error.ToString(), Message = "更新成功" };
            }
            return new ResultModel<Student> { State = ResultType.Error.ToString(), Message = "更新成功" };
        }
        /// <summary>
        /// 查询 翻页+排序
        /// </summary>
        /// <param name="pageSize">显示条数（需要前端定的）</param>
        /// <param name="pageindex">第几页</param>
        /// <returns></returns>
        [HttpGet("Query")]
        public ResultModel<List<Student>> Query(int pageSize, int pageindex)
        {
            int t = 0;
            Expression<Func<Student, bool>> exp = c => true;
            List<Student> result = _iStudentService.Query(exp, pageindex, pageSize, "", out t).ToList();
            return new ResultModel<List<Student>> { State = ResultType.Success.ToString(), Message = "查询成功", Data = result, Rows = t };
        }
        /// <summary>
        /// 指定 id条件查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetToEdit")]
        public async Task<ResultModel<Student>> GetToEdit(int id)
        {
            var result = await _iStudentService.Query(x => x.Tid == id);
            return new ResultModel<Student> { State = ResultType.Success.ToString(), Message = "查询成功" };
        }
    }
}
