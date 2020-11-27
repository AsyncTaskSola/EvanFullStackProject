using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using EvanBackstageApi.Entity.JwtAuthorizeInfo.Root;
using EvanBackstageApi.IRepository;
using EvanBackstageApi.IRepository.IJwtAuthorizeInfoRepository;
using EvanBackstageApi.IService.IJwtAuthorizeInfoService;
using SqlSugar;

namespace EvanBackstageApi.Service.JwtAuthorizeInfoService
{
    public class MenuServices : BaseService<Menu>, IMenuService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBaseRepository<Menu> _dal;
        private readonly IUser _user;

        public MenuServices(IUnitOfWork unitOfWork, IMapper mapper, IBaseRepository<Menu> dal,IUser user)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _dal = dal;
            _user = user;
            BaseDal = _dal;
        }

        /// <summary>
        /// 获取需要权限的菜单
        /// </summary>
        /// <returns></returns>
        public List<Menu> GAuthorMenus ()
        {
            var db = _unitOfWork.GetDbClient();
            var roleGuidlist = db.Queryable<RoleMenu, Menu>((rm, m) => new object[]
            {
                JoinType.Left, m.Id == rm.MenuId
            }).Select(rm => rm.RoleId).ToList();
            var rolesid = roleGuidlist.Distinct().ToArray();//角色权限
            //这里要考虑一个用户多个角色
            var role = _user.GetClaimValueByType(ClaimTypes.Role).ToArray();//开发,普通人员
            var resultrole = new List<Guid>();
            var listmenu = new List<Menu>();
            db.Queryable<Role>().ToList().ForEach(x=>{
                if (role.Any(o => o == x.Code))
                {   
                    resultrole.Add(x.Id);
                }
            });
            var menu= rolesid.Intersect(resultrole).ToList();
            db.Queryable<RoleMenu>().ToList().ForEach(x =>
            {
                menu.ForEach(o =>
                {
                    if (o == x.RoleId)
                    {
                        var menuid = x.MenuId;
                        var menuFirst = db.Queryable<Menu>().Where(w => w.Id == menuid && !w.IsDeleted).First();
                        listmenu.Add(menuFirst);
                    }
                });
            });
            return listmenu;
        }

        /// <summary>
        /// 一维数组组装成树结构
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<Menu> FormatMenu(List<Menu> list)
        {
            var mens = new List<Menu>();
            if (list.Count == 0) return mens;
            //排序
            list = list.OrderBy(x => x.Index).ToList();
            var map = new Dictionary<Guid, Menu>();
            list.ForEach(m => map.Add(m.Id, m));
            list.ForEach(m =>
            {
                if (m.Pid != Guid.Empty)
                {

                    var parent = map[m.Pid];
                    if (parent.Children == null)
                    {
                        parent.Children = new List<Menu>();
                    }
                    parent.Children.Add(m);
                }
                else
                {
                    mens.Add(m);
                }
            });
            return mens;

            #region 另外一种写法
            //var menslist = new List<Menu>();
            //list = list.OrderBy(x => x.Index).ToList();
            //var parentlist = list.Where(x => x.Pid == Guid.Empty).ToList();
            //var result = list.Where(x => x.Pid != Guid.Empty).ToList();
            //parentlist.ForEach(x =>
            //{
            //    result.ForEach(y =>
            //    {
            //        if (y.Pid == x.Id)
            //        {
            //            if (x.Children == null)
            //            {
            //                x.Children = new List<Menu>();
            //            }
            //            x.Children.Add(y); ;
            //        }
            //    });
            //    menslist.Add(x);
            //});
            #endregion
        }
    }
}
