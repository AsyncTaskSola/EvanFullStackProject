import axios from './axios.config'

//获取所有权限
const rolesresult=(pageindex,pageSize)=>axios.get(`/UserInfo/Role/GetRole?pageSize=${pageSize}&pageindex=${pageindex}&oderyFont=`).then(res=>{
    return res.data
});

//改变当前用户的最高角色
const ChangeRole=userRole=>axios.post(`/UserInfo/Role/UpdateURInfo`,userRole).then(res=>{
    console.log('改变当前用户的最高角色',res.data);
    
})

const role={
    rolesresult,
    ChangeRole
}
export default role;