import * as types from './types';
import api from '@/api'

const actions = {
    async [types.LOGIN]({ commit }, form) {
        const params = {
            Account: form.account,
            Password: form.password
        };
        var res = await api.user.login(params);
        console.log("登陆成功", res);
        if (res.state === "success") {
            //保存token 反回来的是jwt  当刷新页面（这里的刷新页面指的是 --> F5刷新,属于清除内存了）时vuex存储的值会丢失，sessionstorage页面关闭后就清除掉了，localstorage不会。
            sessionStorage.setItem("Authorization", res.data.jwttoken);
            localStorage.setItem("Account", params.Account);
            commit(types.SET_USER_INFO, res.data.vuser);
            return true;
        }
        return false;
    },
    //登出
    async [types.LOGOUT]({ commit }, id) {
        console.log("登出", id);
        var res = await api.user.logout(id);
        commit(types.LOGOUT);
    },
    //重置密码
    async [types.SET_USER_RESETPASSWORD]({ commit }, name) {
        console.log("name",name);
        await api.user.setUserResetPassword(name);
        commit(types.SET_USER_RESETPASSWORD);
    }

}

export default actions;