import axios from './axios.config'
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
const user={
    login,
    logout,
    setUserResetPassword
}
export default user