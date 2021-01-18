import axios from './axios.config'
import qs from 'qs';
//登陆
const login = params => axios.post("/LoginInfo/Login", params).then(res => {
    console.log("登陆状态", res);
    return res.data;
});
//登出
const logout=Id=>axios.post(`/LoginInfo/Logout?Id=${Id}`).then(res=>{
    return res.data;
})
//重置密码
const setUserResetPassword = name => axios.post(`/LoginInfo/ResetPassword?name=${name}`).then(res => {
    console.log("状态", res);
    return res.data;
});

//更新用户信息|（密码）
const update=params=>{
    let fromData = new FormData();
	for (let key in params) {
        console.log('key',key);       
		if (params[key]) {
			fromData.append(key, params[key]);
		}
    }
    return axios.post("/LoginInfo/update",fromData,{
        headers: { 'Content-Type': 'multipart/form-data' }
    }).then(res=>{
        console.log("更改用户信息", res);
        return res.data;
    });
}

//获取所有用户信息
const getusers=(pageindex,pageSize)=>axios.get(`/LoginInfo/GetUsers?pageSize=${pageSize}&pageindex=${pageindex}&oderyFont=`).then(res=>{
    console.log("获取所有用户信息", res.data);
    return res.data
});
//获取当前用户信息
const getuser=id=>axios.get(`/LoginInfo/GetUserById?userid=${id}`).then(res=>{
    console.log("获取当前用户信息", res);
    return res.data
});
//删除用户（根据id）
const deletedId=id=>axios.post(`/LoginInfo/Delete?Id=${id}`).then(res=>{
    return res.data;
});
//禁用
const setdisble=id=>axios.post(`/LoginInfo/Disable?userid=${id}`).then(res=>{
    return res.data;
});

//添加
const Add=V_user=>{
    //[FromFrom]
    // let fromData = new FormData();
    // fromData=V_user;
    // axios.post("/LoginInfo/Add", qs.stringify(fromData),{
    //     headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
    // }).then(res=>{
    //     return res.data;
    // })

    axios.post("/LoginInfo/Add",V_user).then(res=>{
        return res.data;
    })
    
}
const user={
    login,
    logout,
    setUserResetPassword,
    update,
    getusers,
    getuser,
    deletedId,
    setdisble,
    Add
}
export default user