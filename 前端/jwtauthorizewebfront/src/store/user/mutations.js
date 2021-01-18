import services from '../../api';
import * as types from './types'  //注释 export
import userJpg from '@/assets/images/user.jpeg'
import state from '../global/state';

const mutations={
	//记录登陆时的用户
	[types.SET_USER_INFO](state,userInfo){
		sessionStorage.setItem("userInfo",JSON.stringify(userInfo));
		state.userInfo=userInfo;
	},

	//登出
	[types.LOGOUT](state)
	{
		sessionStorage.removeItem("Authorization");
		sessionStorage.removeItem("userInfo");
		state.userInfo={};
	},
	//重设密码
	[types.SET_USER_RESETPASSWORD](state)
	{
		state.currentUser = {};
	},
	//获取所有用户信息
	[types.Get_UESERS](state,result)
	{
		state.users=result;
	},
	//获取当前用户信息
	[types.Get_UESER](state,user)
	{
		console.log("user",user.data);
		user.avatar = user.avatar ? user.avatar : userJpg;
		state.currentUser = user.data;
	},
	//关闭，重新赋值
	[types.RESET_CURRENTUSER](state)
	{
		state.currentUser={};
	}
}


export default mutations;
